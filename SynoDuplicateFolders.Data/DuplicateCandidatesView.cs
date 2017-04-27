using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data
{
    class DuplicateCandidatesView
    {
        private readonly TreeView _candidates;
        private readonly ListBox _files;
        private readonly TreeView _where;

        private SynoReportDuplicateCandidates src = null;

        public DuplicateCandidatesView(TreeView candidates, ListBox files, TreeView where)
        {
            _candidates = candidates;
            _files = files;
            _where = where;
            _candidates.AfterSelect += Candidates_AfterSelect;
            _files.SelectedIndexChanged += Files_SelectedIndexChanged;
            _candidates.PathSeparator = "/";
            _where.PathSeparator = "/";

        }

        ~DuplicateCandidatesView()
        {
            _candidates.AfterSelect -= Candidates_AfterSelect;
            _files.SelectedIndexChanged -= Files_SelectedIndexChanged;
        }

        public SynoReportDuplicateCandidates DataSource
        {
            set
            {
                src = value;

                    _candidates.Nodes.Clear();
                    _files.Items.Clear();
                    _where.Nodes.Clear();

                if (src != null)
                {
                    foreach (string f in src.histogram.Keys)
                    {
                        if (src.histogram[f] > 10)
                        {
                            AddToTree(_candidates, f);
                        }
                    }
                }
            }
        }

        private void Candidates_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _files.Items.Clear();
            _where.Nodes.Clear();
            List<string> folders = new List<string>();
            try
            {
                string selected = '/' + _candidates.SelectedNode.FullPath;
                foreach (long index in src.bypath[selected])
                {
                    foreach (DuplicateFileInfo s in src.toc[index])
                    {
                        if (s.FullPath.StartsWith(selected))
                        {
                            if (folders.Contains(s.FileName) == false && s.FileName != null)
                                folders.Add(s.FileName);
                        }
                    }
                }
            }
            catch { }

            _files.Items.AddRange(folders.ToArray());
            _files.SelectedIndex = -1;
        }

        private void Files_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _where.Nodes.Clear();
            try
            {
                foreach (long index in src.bypath['/' + _candidates.SelectedNode.FullPath])
                {
                    foreach (DuplicateFileInfo s in src.toc[index])
                    {
                        if (s.FileName == _files.Text)
                        {
                            AddToTree(_where, s.FullPath);
                            foreach (DuplicateFileInfo o in src.toc[index])
                            {
                                AddToTree(_where, o.FullPath);
                            }
                        }
                    }
                }
            }
            catch { }
            _where.ExpandAll();
        }

        private void AddToTree(TreeView target, string path)
        {
            string[] folders = path.Split('/');
            string parent = '/' + folders[1];
            string key = string.Empty;

            for (int i = 1; i <= folders.GetUpperBound(0); i++)
            {
                key += '/' + folders[i];

                TreeNode[] nodes = target.Nodes.Find(parent, true);
                if (nodes.Count() == 0)
                {
                    target.Nodes.Add(key, folders[i]);
                    parent = key;
                }
                else
                {
                    if (target.Nodes.Find(key, true).Count() == 0)
                        nodes[0].Nodes.Add(key, folders[i]);
                    parent = key;
                }

            }
        }

    }

}

