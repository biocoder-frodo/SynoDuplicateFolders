using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SynoDuplicateFolders.Controls
{
    public partial class DuplicateCandidatesView : UserControl
    {
        public delegate void DuplicateCandidatesItemOpenHandler(object sender, ItemOpenedEventArgs e);
        public delegate void DuplicateCandidatesItemCompareHandler(object sender, ItemsComparedEventArgs e);
        public delegate void DuplicateCandidatesItemStatusUpdate(object sender, ItemStatusUpdateEventArgs e);
        public delegate void DuplicateCandidatesItemHideHandler(object sender, ItemHiddenEventArgs e);
        private enum FsType
        {
            fsFile,
            fsFolder
        }
        private SynoReportDuplicateCandidates src = null;
        private TreeNode _context_node = null;

        private FileInfo _context_file = null;
        private object _context_file_control = null;

        private FsType _checked_items = FsType.fsFile;
        private List<string> _checked = new List<string>();
        private int _maximum_comparable = 3;

        public event DuplicateCandidatesItemOpenHandler OnItemOpen;
        public event DuplicateCandidatesItemCompareHandler OnItemCompare;
        public event DuplicateCandidatesItemStatusUpdate OnItemStatusUpdate;
        public event DuplicateCandidatesItemHideHandler OnItemHide;

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
        private IDuplicateExclusionSource exclusionSource;
        public IDuplicateExclusionSource ExclusionSource
        {
            get
            {
                return exclusionSource;
            }
            set
            {
                if (value != exclusionSource)
                {
                    if (exclusionSource != null) exclusionSource.PropertyChanged -= ExclusionSource_PropertyChanged;
                    exclusionSource = value;
                    if (exclusionSource != null) exclusionSource.PropertyChanged += ExclusionSource_PropertyChanged;
                    exclusionSource.AttachDetach();
                }
            }
        }

        private void ExclusionSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (exclusionSource.Paths.Any())
            {
                System.Diagnostics.Debug.WriteLine("Duplicate exclusions:");
                foreach (var path in exclusionSource.Paths)
                    System.Diagnostics.Debug.WriteLine(path);
            }
            else
                System.Diagnostics.Debug.WriteLine("No duplicate exclusions.");
        }

        public SynoReportDuplicateCandidates DataSource
        {
            set
            {
                System.Diagnostics.Debug.WriteLine($" filtering ...");
                src = value;
                src.Filter = dfi => dfi.Any(f => exclusionSource.Paths.Any(p => p.StartsWith(f.FullPath.Substring(1)))) == false;
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
                dataGridView1.setDataSource<IDuplicateFileInfo>(src);

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

            OnItemStatusUpdate?.Invoke(this, new ItemStatusUpdateEventArgs(string.Format("{0} duplicate(s)", count)));

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
            OnItemStatusUpdate?.Invoke(this, new ItemStatusUpdateEventArgs(status));
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
                _context_node = sender.GetNodeAt(e.X, e.Y);

                if (_context_node != null)
                {
                    if (_context_node.FullPath.Contains("/"))
                    {
                        _context_file = GetUNCPath(_context_node, out location, out file, out isFile);

                        setContextMenuStripItems(location, file, isFile);
                        hideToolStripMenuItem.Enabled = sender == Where;
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

        private FileInfo GetUNCPath(TreeNode node, out bool location, out bool file, out bool isFile)
        {
            return SynoReportDuplicateCandidates.GetUNCPath(HostName, node.FullPath, out location, out file, out isFile);
        }

        private FileInfo GetUNCPath(DataGridViewCellContextMenuStripNeededEventArgs e, out bool location, out bool file, out bool isFile)
        {
            return GetUNCPath(dataGridView1.Rows[e.RowIndex].DataBoundItem as DuplicateFileInfo, out location, out file, out isFile);
        }

        private FileInfo GetUNCPath(DuplicateFileInfo duplicate, out bool location, out bool file, out bool isFile)
        {
            return SynoReportDuplicateCandidates.GetUNCPath(HostName, duplicate.FullPath.Substring(1), out location, out file, out isFile);
        }

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
                _checked.Add((row.DataBoundItem as DuplicateFileInfo).FullPath.Substring(1));
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_context_file != null)
            {
                if (e.ClickedItem == compareExternallyToolStripMenuItem)
                {
                    if (_context_file_control == dataGridView1)
                    {
                        List<string> test = new List<string>();
                        foreach (string path in _checked)
                        {
                            bool location;
                            bool file;
                            bool isFile;
                            FileInfo fi = SynoReportDuplicateCandidates.GetUNCPath(HostName, path, out location, out file, out isFile);
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

                if (e.ClickedItem == openFileToolStripMenuItem)
                {
                    OnItemOpen?.Invoke(this, new ItemOpenedEventArgs(_context_file.FullName, false, (bool)openFileToolStripMenuItem.Tag));
                }

                if (e.ClickedItem == openFileLocationToolStripMenuItem)
                {
                    OnItemOpen?.Invoke(this, new ItemOpenedEventArgs(_context_file.FullName, true, true));
                }
                if (e.ClickedItem == hideToolStripMenuItem)
                {
                    string hide;
                    if (_context_file_control == dataGridView1)
                    {
                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            hide = (row.DataBoundItem as DuplicateFileInfo).FullPath.Substring(1);
                            exclusionSource?.RemoveExclusion(hide);
                            OnItemHide?.Invoke(this, new ItemHiddenEventArgs(HostName, hide, true));
                        }
                    }
                    else
                    {
                        exclusionSource?.RemoveExclusion(_context_node.FullPath);
                        OnItemHide?.Invoke(this, new ItemHiddenEventArgs(HostName, _context_node.FullPath, (bool)openFileToolStripMenuItem.Tag));
                    }
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

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}

