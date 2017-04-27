using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Extensions;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SynoDuplicateFolders.Controls
{
    public partial class DuplicateCandidatesView : UserControl
    {
        private SynoReportDuplicateCandidates src = null;

        public DuplicateCandidatesView()
        {
            InitializeComponent();
        }

        public SynoReportDuplicateCandidates DataSource
        {
            set
            {
                src = value;

                Candidates.Nodes.Clear();
                Files.Items.Clear();
                Where.Nodes.Clear();

                if (src != null)
                {
                    //foreach (string f in src.DuplicatesGroupByPath.Keys)
                    //{
                    //    if (src.DuplicatesGroupByPath[f].Count > 10)
                    //    {
                    //        Candidates.Add(f);
                    //    }
                    //}
                    foreach (var f in src.Folders.Values)
                    {
                        Candidates.Add(f);
                    }
                }
            }
        }

        private void Candidates_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Files.Items.Clear();
            Where.Nodes.Clear();
            List<string> folders = new List<string>();

            string selected = '/' + Candidates.SelectedNode.FullPath;

            if (src.DuplicatesGroupByPath.ContainsKey(selected))
                foreach (long group in src.DuplicatesGroupByPath[selected])
                    foreach (DuplicateFileInfo s in src.DuplicatesByGroup[group])
                    {
                        if (s.FullPath.StartsWith(selected))
                        {
                            if (folders.Contains(s.FileName) == false && s.FileName != null)
                                folders.Add(s.FileName);
                        }
                    }


            Files.Items.AddRange(folders.ToArray());
            Files.SelectedIndex = -1;
        }

        private void Files_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Where.Nodes.Clear();

            if (src.DuplicatesGroupByName.ContainsKey((string)Files.SelectedItem))
                foreach (long index in src.DuplicatesGroupByName[(string)Files.SelectedItem])
                    foreach (DuplicateFileInfo s in src.DuplicatesByGroup[index])
                    {
                        if (s.FileName == Files.Text)
                        {
                            Where.Add(s.FullPath);
                            foreach (DuplicateFileInfo o in src.DuplicatesByGroup[index])
                            {
                                Where.Add(o.FullPath);
                            }
                        }
                    }
            Where.ExpandAll();
        }
    }
}
