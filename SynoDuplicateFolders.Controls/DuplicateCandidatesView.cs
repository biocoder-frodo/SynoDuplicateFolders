using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Extensions;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.IO;
using System.ComponentModel;
using SynoDuplicateFolders.Data.ComponentModel;

namespace SynoDuplicateFolders.Controls
{
    public partial class DuplicateCandidatesView : UserControl
    {
        public delegate void DuplicateCandidatesItemOpenHandler(object sender, ItemOpenedEventArgs e);
        public delegate void DuplicateCandidatesItemCompareHandler(object sender, ItemsComparedEventArgs e);
        public delegate void DuplicateCandidatesItemStatusUpdate(object sender, string status);

        private enum FsType
        {
            fsFile,
            fsFolder
        }
        private SynoReportDuplicateCandidates src = null;
        private TreeNode context_node = null;

        private FileInfo _context_file = null;
        private object _context_file_control = null;

        private FsType _checked_items = FsType.fsFile;
        private List<string> _checked = new List<string>();
        private int _maximum_comparable = 3;

        public event DuplicateCandidatesItemOpenHandler OnItemOpen;
        public event DuplicateCandidatesItemCompareHandler OnItemCompare;
        public event DuplicateCandidatesItemStatusUpdate OnItemStatusUpdate;

        public DuplicateCandidatesView()
        {
            InitializeComponent();
        }

        public string HostName { get; set; }

        public int MaximumComparable
        {
            set
            {
                if (value < 2)
                {
                    _maximum_comparable = 2;
                }
                else
                {
                    _maximum_comparable = value;
                }
            }
            get
            {
                return _maximum_comparable;
            }
        }

        public SynoReportDuplicateCandidates DataSource
        {
            set
            {
                src = value;

                Candidates.Nodes.Clear();
                Files.Items.Clear();
                Where.Nodes.Clear();
                _checked.Clear();
                if (src != null)
                {
                    foreach (var f in src.Folders.Values)
                    {
                        Candidates.Add(f);
                    }
                }
                SortOrderManager.SetSortOrder<IDuplicateFileInfo>("Size", ListSortDirection.Descending);
                dataGridView1.setDataSource<IDuplicateFileInfo>(value);

            }
        }

        private void Candidates_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Files.Items.Clear();
            Where.Nodes.Clear();
            _checked.Clear();
            List<string> folders = new List<string>();
            long count = 0;
            string selected = '/' + e.Node.FullPath;

            if (src.DuplicatesGroupByPath.ContainsKey(selected))
            {
                foreach (long group in src.DuplicatesGroupByPath[selected])
                    foreach (DuplicateFileInfo s in src.DuplicatesByGroup[group])
                    {
                        if (s.FullPath.StartsWith(selected))
                        {
                            count++;
                            if (folders.Contains(s.FileName) == false && s.FileName != null)
                                folders.Add(s.FileName);
                        }
                    }

            }

            OnItemStatusUpdate?.Invoke(this, string.Format("{0} duplicate(s)", count));

            Files.Items.AddRange(folders.ToArray());
            Files.SelectedIndex = -1;
        }

        private void Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = string.Empty;
            Where.Nodes.Clear();
            _checked.Clear();
            if (Files.SelectedItem != null)
            {
                if (src.DuplicatesGroupByName.ContainsKey((string)Files.SelectedItem))

                    foreach (long index in src.DuplicatesGroupByName[(string)Files.SelectedItem])
                        foreach (DuplicateFileInfo s in src.DuplicatesByGroup[index])
                        {
                            if (s.FileName == Files.Text)
                            {
                                status = (string)Files.SelectedItem + " " + s.Length.ToFileSizeString();
                                Where.Add(s.FullPath);
                                foreach (DuplicateFileInfo o in src.DuplicatesByGroup[index])
                                {
                                    Where.Add(o.FullPath);
                                }
                            }
                        }
                Where.ExpandAll();
            }
            OnItemStatusUpdate?.Invoke(this, status);
        }

        private void control_Enter(object sender)
        {
            _context_file_control = sender;
        }

        private void treeView_MouseUp(TreeView sender, MouseEventArgs e)
        {
            bool location;
            bool file;
            bool isFile;

            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                context_node = sender.GetNodeAt(e.X, e.Y);

                if (context_node != null)
                {
                    if (context_node.FullPath.Contains("/"))
                    {
                        _context_file = GetUNCPath(context_node, out location, out file, out isFile);

                        setContextMenuStripItems(location, file, isFile);

                        contextMenuStrip1.Show(sender, e.Location);
                    }
                }
            }
        }

        private void setContextMenuStripItems(bool location, bool file, bool isFile)
        {
            openFileLocationToolStripMenuItem.Enabled = location;
            openFileToolStripMenuItem.Enabled = file;
            openFileToolStripMenuItem.Tag = isFile;
            compareExternallyToolStripMenuItem.Enabled = (_checked.Count > 1 && _checked.Count <= _maximum_comparable);
        }

        #region GetUNCPath
        private FileInfo GetUNCPath(TreeNode node, out bool location, out bool file, out bool isFile)
        {
            return GetUNCPath(node.FullPath, out location, out file, out isFile);
        }

        private FileInfo GetUNCPath(DataGridViewCellContextMenuStripNeededEventArgs e, out bool location, out bool file, out bool isFile)
        {
            var r = dataGridView1.Rows[e.RowIndex].DataBoundItem as DuplicateFileInfo;

            return GetUNCPath(r, out location, out file, out isFile);
        }

        private FileInfo GetUNCPath(DuplicateFileInfo duplicate, out bool location, out bool file, out bool isFile)
        {
            return GetUNCPath(duplicate.FullPath.Substring(1), out location, out file, out isFile);
        }

        private FileInfo GetUNCPath(string path, out bool location, out bool file, out bool isFile)
        {
            FileInfo result = null;
            try
            {
                path = RemoveVolumeFromPath(path);

                if (PathCanBeOpened(path, out location, out file, out isFile, out result) == false)
                {
                    string[] homes = path.Split('/');
                    if (homes.Length > 2)
                    {
                        if (homes[1] == "homes")
                        {
                            path = "/home" + path.Substring(2 + homes[1].Length + homes[2].Length);
                            PathCanBeOpened(path, out location, out file, out isFile, out result);
                        }
                    }
                }

                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", System.DateTime.UtcNow.Ticks, ex.Message));

                location = false;
                file = false;
                isFile = false;
                return null;
            }
        }

        private bool PathCanBeOpened(string path, out bool openFileLocation, out bool openFile, out bool isFile, out FileInfo result)
        {
            openFileLocation = false;
            openFile = false;
            isFile = false;
            if (string.IsNullOrWhiteSpace(HostName)) throw new ArgumentException("The HostName property must be set, it cannot be empty.");
            var uncpath = string.Format("{0}{1}", Path.DirectorySeparatorChar, Path.DirectorySeparatorChar) + HostName + path.Replace('/', Path.DirectorySeparatorChar);

            result = null;
            try
            {
                if (Directory.Exists(uncpath))
                {
                    openFileLocation = false;
                    openFile = true;
                    var test = Directory.GetFiles(uncpath);
                    isFile = false;
                }
                if (File.Exists(uncpath))
                {
                    openFileLocation = true;
                    openFile = true;
                    using (var test = File.OpenRead(uncpath))
                    { }
                    isFile = true;
                }
                result = new FileInfo(uncpath);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", System.DateTime.UtcNow.Ticks, ex.Message));
            }
            return openFile || openFileLocation;
        }

        private string RemoveVolumeFromPath(string path)
        {
            return path.Substring(path.IndexOf('/'));
        }
        #endregion

        private void Where_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            bool location;
            bool file;
            bool isFile;
            bool expected = false;


            var path = GetUNCPath(e.Node, out location, out file, out isFile);
            System.Diagnostics.Debug.WriteLine(string.Format("tested {0}", path == null ? "ACCESS DENIED" : path.FullName));
            System.Diagnostics.Debug.WriteLine(string.Format("type of selection {0}, {1} item(s) in selection", _checked_items, _checked.Count));

            if (path != null)
            {
                if (!e.Node.Checked)
                {
                    if (_checked.Count == 0)
                    {
                        _checked_items = isFile ? FsType.fsFile : FsType.fsFolder;
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("type of selection {0}, {1} item(s) in selection", _checked_items, _checked.Count));

                    if (_checked_items == (isFile ? FsType.fsFile : FsType.fsFolder))
                    {
                        if (!_checked.Contains(path.FullName))
                        {
                            _checked.Add(path.FullName);
                            expected = true;
                        }
                    }
                }
                else
                {
                    if (_checked_items == (isFile ? FsType.fsFile : FsType.fsFolder))
                    {
                        if (_checked.Contains(path.FullName))
                        {
                            _checked.Remove(path.FullName);
                            expected = true;
                        }
                    }
                }
            }

            e.Cancel = !expected;
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", System.DateTime.UtcNow.Ticks, expected ? "approved" : "canceled"));
            System.Diagnostics.Debug.WriteLine(string.Format("type of selection {0}, {1} item(s) in selection", _checked_items, _checked.Count));
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DuplicateFileInfo d = dataGridView1.Rows[e.RowIndex].DataBoundItem as DuplicateFileInfo;
                var nodes = Candidates.Nodes.Find(d.Path, true);
                if (nodes.Length == 1)
                {
                    Candidates.CollapseAll();
                    nodes[0].EnsureVisible();
                    Candidates.SelectedNode = nodes[0];
                    Files.SelectedItem = d.FileName;
                    nodes = Where.Nodes.Find(d.FullPath, true);
                    if (nodes.Length == 1)
                    {
                        tabControl1.SelectedTab = tabPage1;
                        Where.Select();
                        nodes[0].EnsureVisible();
                        Where.SelectedNode = nodes[0];
                    }
                }
            }
        }

        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            bool location;
            bool file;
            bool isFile;
            if (e.RowIndex != -1)
            {
                control_Enter(sender);
                _context_file = GetUNCPath(e, out location, out file, out isFile);
                setContextMenuStripItems(location, file, isFile);
                e.ContextMenuStrip = contextMenuStrip1;

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            _checked_items = FsType.fsFile;
            _checked.Clear();
            foreach (DataGridViewRow row in (sender as SynoReportDataGridView).SelectedRows)
            {
                var d = row.DataBoundItem as DuplicateFileInfo;
                _checked.Add(d.FullPath.Substring(1));
            }

        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_context_file != null)
            {
                if (e.ClickedItem != compareExternallyToolStripMenuItem)
                {
                    if (OnItemOpen != null)
                    {
                        if (e.ClickedItem == openFileToolStripMenuItem)
                        {
                            OnItemOpen(this, new ItemOpenedEventArgs(_context_file.FullName, false, (bool)openFileToolStripMenuItem.Tag));
                        }
                        if (e.ClickedItem == openFileLocationToolStripMenuItem)
                        {
                            OnItemOpen(this, new ItemOpenedEventArgs(_context_file.FullName, true, true));
                        }
                    }
                }
                else
                {
                    if (_context_file_control == dataGridView1)
                    {
                        List<string> test = new List<string>();
                        foreach (string path in _checked)
                        {
                            bool location;
                            bool file;
                            bool isFile;
                            FileInfo fi = GetUNCPath(path, out location, out file, out isFile);
                            if (fi != null)
                            {
                                test.Add(fi.FullName);
                            }
                        }
                        if (test.Count > 1)
                        {
                            _checked = test;
                        }
                        else
                        {
                            return;
                        }
                    }
                    OnItemCompare?.Invoke(this, new ItemsComparedEventArgs(_checked));
                }
            }

        }

        #region Treeview handlers
        private void Candidates_MouseUp(object sender, MouseEventArgs e)
        {
            treeView_MouseUp(sender as TreeView, e);
        }

        private void Where_MouseUp(object sender, MouseEventArgs e)
        {
            treeView_MouseUp(sender as TreeView, e);
        }

        private void Candidates_Enter(object sender, EventArgs e)
        {
            control_Enter(sender);
        }

        private void Where_Enter(object sender, EventArgs e)
        {
            control_Enter(sender);
        }
        #endregion

    }
}

