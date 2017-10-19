using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.ComponentModel;
using SynoDuplicateFolders.Data.SecureShell;
using SynoDuplicateFolders.Properties;
using SynoDuplicateFolders.Extensions;

using static SynoDuplicateFolders.Configuration.UserSectionHandler;
using static System.Environment;
using System.IO;
using System.ComponentModel;

namespace SynoDuplicateFolders
{
    public partial class SynoReportClient : Form
    {
        private class CurrentSortOrder
        {
            public string Column;
            public ListSortDirection Direction;
        }
        private readonly Dictionary<SynoReportType, CurrentSortOrder> sortOrderGrid = new Dictionary<SynoReportType, CurrentSortOrder>();
        private SynoReportType detailsGridType;
        private event Action<string> CacheUpdateCompleted;
        private event Action DuplicatesAnalysisCompleted;

        private ISynoReportCache cache = null;
        private CustomSettings config = null;
        private SynoReportDuplicateCandidates dupes = null;
        
        private int fileSizeColumn = -1;

        public SynoReportClient()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            cmbFileDetails.SelectedIndex = 0;

            CacheUpdateCompleted += SynoReportClient_CacheUpdateCompleted;
            DuplicatesAnalysisCompleted += SynoReportClient_DuplicatesAnalysisCompleted;
        }

        private void SynoReportClient_Load(object sender, EventArgs e)
        {
            try
            {
                FileInfo source = null;
                string entropy = "Is a gift a gift without wrapping?";
                if (!string.IsNullOrEmpty(Settings.Default.DPAPIVector) && !string.IsNullOrWhiteSpace(Settings.Default.DPAPIVector))
                {
                    try
                    {
                        source = new FileInfo(Settings.Default.DPAPIVector);
                    }
                    catch (ArgumentException)
                    {
                        //don't care if it's not a filename
                    }

                    if (source != null && source.Exists)
                    {
                        entropy = new StreamReader(source.FullName).ReadToEnd();
                    }
                    else
                    {
                        entropy = Settings.Default.DPAPIVector;
                    }
                }

                WrappedPassword<DSMAuthentication>.SetEntropy(entropy);
                WrappedPassword<DSMAuthenticationKeyFile>.SetEntropy(entropy);
                WrappedPassword<DefaultProxy>.SetEntropy(entropy);

                config = GetSection<CustomSettings>();

                PopulateServerTree();

                volumeHistoricChart1.Configuration = config;
                chartGrid1.Configuration = config;

                if (string.IsNullOrEmpty(Settings.Default.AutoRefreshServer) == false)
                {
                    string tag = Settings.Default.AutoRefreshServer;
                    if (config.DSMHosts.Items.ContainsKey(tag))
                    {
                        Task.Factory.StartNew(() => SynoReportClient_CacheUpdate(tag));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SynoReportClient_DuplicatesAnalysisCompleted()
        {
            Invoke(new Action(PopulateDuplicatesTab));
        }

        private void ProgressUpdateProcessing()
        {
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
        }

        private void SynoReportClient_CacheUpdate(string host)
        {
            try
            {
                Invoke(new Action(ProgressUpdateProcessing));

                DSMHost h = config.DSMHosts.Items[host];

                SynoReportViaSSH connection = null;

                if (Settings.Default.UseProxy && h.Proxy == null)
                {
                    connection = new SynoReportViaSSH(h, new DefaultProxy());
                }
                else
                {
                    if (h.Proxy != null)
                    {
                        connection = new SynoReportViaSSH(h, h.Proxy);
                    }
                    else
                    {
                        connection = new SynoReportViaSSH(h);
                    }
                }

                connection.RmExecutionMode = Settings.Default.RmExecutionMode;

                cache = connection;

                if (!string.IsNullOrEmpty(Settings.Default.CacheFolder))
                {
                    cache.Path = Path.Combine(Settings.Default.CacheFolder, h.Host);
                }
                else
                {
                    cache.Path = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), Application.ProductName, h.Host);
                }

                if (Settings.Default.KeepAnalyzerDb == true)
                {
                    cache.KeepAnalyzerDbCount = -1;
                }
                else
                {
                    cache.KeepAnalyzerDbCount = Settings.Default.KeepAnalyzerDbCount;
                }

                cache.DownloadUpdate += Cache_StatusUpdate;
                connection.DownloadCSVFiles();

                if (CacheUpdateCompleted != null)
                {
                    CacheUpdateCompleted.Invoke(string.Format("{0} ({1})", connection.Host, connection.Version));
                }

                if (cache.GetReports(SynoReportType.DuplicateCandidates).Count > 0)
                {
                    dupes = cache.GetReport(SynoReportType.DuplicateCandidates) as SynoReportDuplicateCandidates;
                }

                if (DuplicatesAnalysisCompleted != null)
                {
                    DuplicatesAnalysisCompleted.Invoke();
                }
            }
            catch (SynoReportViaSSHLoginFailure ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cache_StatusUpdate(object sender, SynoReportCacheDownloadEventArgs e)
        {
            Invoke(new Action<SynoReportCacheDownloadEventArgs>(ProgressUpdate), e);
        }

        private void SynoReportClient_CacheUpdateCompleted(string version)
        {
            Invoke(new Action<string>(PopulateFromCache), version);
        }

        private void preferences_Click(object sender, EventArgs e)
        {
            new Preferences(config).ShowDialog();
        }

        private void timeStampTrackBar_Scroll(object sender, EventArgs e)
        {
            chartGrid1.DataSource = cache.GetReport(timeStampTrackBar.Value, SynoReportType.VolumeUsage, SynoReportType.ShareList) as IVolumePieChart;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == fileSizeColumn)
            {
                e.Value = ((long)e.Value).ToFileSizeString();
                e.FormattingApplied = true;
            }
            else
            {
                e.FormattingApplied = false;
            }

        }
        private void timeStampTrackBar1_Scroll(object sender, EventArgs e)
        {
            string value = cmbFileDetails.Text.ToLowerInvariant();
            switch (value)
            {
                case "owners":
                    setDataSource<ISynoReportOwnerDetail>(dataGridView1, timeStampTrackBar1.Value, SynoReportType.FileOwner);
                    break;
                case "most modified":
                    setDataSource<ISynoReportFileDetail>(dataGridView1, timeStampTrackBar1.Value, SynoReportType.MostModified);
                    break;
                case "least modified":
                    setDataSource<ISynoReportFileDetail>(dataGridView1, timeStampTrackBar1.Value, SynoReportType.LeastModified);
                    break;
                default:
                    setDataSource<ISynoReportGroupDetail>(dataGridView1, timeStampTrackBar1.Value, SynoReportType.FileGroup);
                    break;
            }
        }
        private void cmbFileDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            timeStampTrackBar1_Scroll(sender, e);
        }

        private void setDataSource<T>(DataGridView grid, DateTime ts, SynoReportType type) where T : class, ISynoReportDetail
        {
            detailsGridType = type;
            fileSizeColumn = -1;
            if (cache != null)
            {
                var rows = cache.GetReport(ts, type);
                if (rows != null)
                {
                    CurrentSortOrder sort;
                    if (!sortOrderGrid.ContainsKey(type))
                    {
                        sort = new CurrentSortOrder() { Column = string.Empty, Direction = ListSortDirection.Ascending };
                        if (grid.SortOrder != SortOrder.None)
                        {
                            sort.Column = grid.SortedColumn.Name;
                            sort.Direction = grid.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                        }
                        else
                        {
                            sort.Column = string.Empty;
                            sort.Direction = ListSortDirection.Ascending;
                        }
                        sortOrderGrid.Add(type, sort);
                    }
                    else
                    {
                        sort = sortOrderGrid[type];
                    }


                    grid.Visible = true;
                   
                    grid.DataSource = (rows as ISynoReportBindingSource<T>).BindingSource;
                    
                    if (grid.Columns.Contains("Size"))
                    {
                        fileSizeColumn = grid.Columns["Size"].Index;
                    }
                    if (!string.IsNullOrEmpty(sort.Column))
                    {
                        grid.Sort(grid.Columns[sort.Column], sort.Direction);
                    }
                }
                else
                {
                    grid.DataSource = null;
                }
            }
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string tag = (string)treeView1.SelectedNode.Tag;
            if (e.ClickedItem == refreshToolStripMenuItem)
            {
                Task.Factory.StartNew(() => SynoReportClient_CacheUpdate(tag));
            }
            else if (e.ClickedItem == removeServerToolStripMenuItem)
            {
                if (MessageBox.Show(string.Format("Are you sure you want to remove server '{0}' from the list of servers?", tag),
                    "Remove server",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2)

                    == DialogResult.Yes)
                {
                    config.DSMHosts.Items.Remove(tag);
                    config.CurrentConfiguration.Save();
                    PopulateServerTree();
                }

            }
            else if (e.ClickedItem == propertiesToolStripMenuItem)
            {
                var srv = new HostConfiguration(config.DSMHosts.Items[tag]);
                srv.ShowDialog();
                if (srv.Canceled == false)
                {
                    config.CurrentConfiguration.Save();
                }
            }
        }

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var srv = new HostConfiguration();
            srv.ShowDialog();
            if (srv.Canceled == false)
            {
                config.DSMHosts.Items.Add(srv.Host);
                config.CurrentConfiguration.Save();
                PopulateServerTree();
            }
        }

        private void PopulateServerTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("NAS");
            treeView1.Nodes[0].ContextMenuStrip = contextMenuStrip1;

            foreach (DSMHost h in config.DSMHosts.Items)
            {
                var node = treeView1.Nodes[0].Nodes.Add(h.Host, h.Host);
                node.ContextMenuStrip = contextMenuStrip2;
                node.Tag = h.Host;
            }
        }

        private void ProgressUpdate(SynoReportCacheDownloadEventArgs e)
        {
            treeView1.Enabled = false;
            toolsStripMenuItem.Enabled = false;
            toolStripMenuItem2.Enabled = false;

            switch (e.Status)
            {
                case CacheStatus.FetchingDirectoryInfo:
                    toolStripStatusLabel1.Text = "Fetching folder contents.";
                    break;
                case CacheStatus.FetchingVersionInfo:
                    toolStripStatusLabel1.Text = "Fetching DSM version.";
                    break;
                case CacheStatus.FetchingVersionInfoCompleted:
                    toolStripStatusLabel1.Text = "DSM version " + e.Message;
                    break;
                case CacheStatus.Downloading:
                    toolStripStatusLabel1.Text = string.Format("Downloading files.. [{0} of {1}]", e.FilesFetched, e.TotalFiles);
                    toolStripProgressBar1.Minimum = 0;
                    toolStripProgressBar1.Maximum = e.TotalFiles;
                    toolStripProgressBar1.Value = e.FilesFetched;
                    break;
                default:

                    treeView1.Enabled = true;
                    toolsStripMenuItem.Enabled = true;
                    toolStripMenuItem2.Enabled = true;
                    exportSharesReportToolStripMenuItem.Enabled = cache != null;
                    exportVolumeReportToolStripMenuItem.Enabled = cache != null;

                    toolStripStatusLabel1.Text = "Idle.";
                    toolStripProgressBar1.Minimum = 0;
                    toolStripProgressBar1.Maximum = e.TotalFiles;
                    toolStripProgressBar1.Value = e.FilesFetched;
                    break;

                case CacheStatus.Processing:
                    toolStripStatusLabel1.Text = "Processing...";
                    break;

            }
        }
        private void PopulateFromCache(string version)
        {
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            this.Text = "SynoReport Client - " + version;
            treeView1.SelectedNode.ToolTipText = version;

            volumeHistoricChart1.ShowingType = SynoReportType.ShareList;
            volumeHistoricChart1.DataSource = cache;

            timeStampTrackBar.DateRange = cache.DateRange;
            timeStampTrackBar1.DateRange = cache.DateRange;

            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        private void PopulateDuplicatesTab()
        {
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            if (dupes != null)
            {
                duplicateCandidatesView1.DataSource = dupes;
            }
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportVolumeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file;
            if (SaveDialog("Save Volume report...", out file))
            {
                (cache.GetReport(SynoReportType.VolumeUsage) as SynoReportVolumeUsage).WriteTimeLineData(file);
            }
        }

        private void exportSharesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file;
            if (SaveDialog("Save Shares report...", out file))
            {
                (cache.GetReport(SynoReportType.ShareList) as SynoReportShares).WriteTimeLineData(file);
            }

        }
        private bool SaveDialog(string title, out string filename)
        {
            DialogResult r;
            filename = string.Empty;

            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "tab";
            saveFileDialog1.Filter = "All files (*.*)|*.*|Tab Delimited Files (*.tab)|*.tab";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.SupportMultiDottedExtensions = true;
            saveFileDialog1.Title = title;
            r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            return r == DialogResult.OK;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);

            switch (dgv.SortOrder)
            {
                case SortOrder.Descending:
                    sortOrderGrid[detailsGridType].Direction = ListSortDirection.Descending;
                    sortOrderGrid[detailsGridType].Column = dgv.SortedColumn.Name;
                    break;
                case SortOrder.Ascending:
                    sortOrderGrid[detailsGridType].Direction = ListSortDirection.Ascending;
                    sortOrderGrid[detailsGridType].Column = dgv.SortedColumn.Name;
                    break;
                default:
                    sortOrderGrid[detailsGridType].Direction = ListSortDirection.Ascending;
                    sortOrderGrid[detailsGridType].Column = string.Empty;
                    break;
            }
        }
        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);
            dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.None;            
            dataGridView1_ColumnHeaderMouseClick(dgv, e);
        }
    }
}
