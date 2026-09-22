using DiskStationManager.SecureShell;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.ComponentModel;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Data.SecureShell;
using SynoDuplicateFolders.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SynoDuplicateFolders.Properties.CustomSettings;
using static SynoDuplicateFolders.Properties.Settings;
using static System.Configuration.UserSectionHandler;
using static System.Environment;

namespace SynoDuplicateFolders
{
    public partial class SynoReportClient : Form
    {
        private event Action<string> CacheUpdateCompleted;
        private event Action DuplicatesAnalysisCompleted;

        private ISynoReportCache cache = null;
        private SynoReportDuplicateCandidates dupes = null;

        private DSMHost selected;
        private DuplicateCandidatesExclusion<DSMHost> exclusion;
        private DateTime? commonDateSelection;
        private ContentToolWindow contentTool;
        public SynoReportClient()
        {

            InitializeComponent();
            this.components.Add(new Disposer(this.OnDispose));

            dataGridView1.AutoGenerateColumns = true;
            cmbFileDetails.SelectedIndex = 0;

            CacheUpdateCompleted += SynoReportClient_CacheUpdateCompleted;
            DuplicatesAnalysisCompleted += SynoReportClient_DuplicatesAnalysisCompleted;

            duplicateCandidatesView1.OnItemOpen += DuplicateCandidatesView1_OnItemOpen;
            duplicateCandidatesView1.OnItemCompare += DuplicateCandidatesView1_OnItemCompare;
            duplicateCandidatesView1.OnItemStatusUpdate += DuplicateCandidatesView1_OnItemStatusUpdate;
            duplicateCandidatesView1.OnItemHide += DuplicateCandidatesView1_OnItemHide;
            duplicateCandidatesView1.OnDeduplicationRequest += DuplicateCandidatesView1_OnDeduplicationRequest;

            volumePies.ToolTipChanged += VolumePies_ToolTipChanged;
        }

        private void VolumePies_ToolTipChanged(object sender, ChartGridToolTipEventArgs e) => contentTool?.Update(cache, commonDateSelection.Value, e);


        private void DuplicateCandidatesView1_OnDeduplicationRequest(object sender, ItemsComparedEventArgs e)
        {
            var folders = new List<string>(e.Items).Select(f => new DirectoryInfo(f)).ToList();
            using (var form = new DeduplicationConfirmation(folders))
            {
                form.ShowDialog();
            }
            duplicateCandidatesView1.ClearDedupSelection();
        }

        private void Profile_LegendChanged(object sender, EventArgs e)
        {
            volumeHistoricChart1.Refresh();
            volumePies.Refresh();
        }

        private void OnDispose(bool disposing)
        {
            dupes?.Dispose();
        }
        private void DuplicateCandidatesView1_OnItemStatusUpdate(object sender, ItemStatusUpdateEventArgs e) => toolStripStatusLabel1.Text = e.Status;

        private void DuplicateCandidatesView1_OnItemCompare(object sender, ItemsComparedEventArgs e)
        {
            try
            {
                string args = Default.DiffArgs;
                if (!args.Contains("{0}"))
                {
                    args += "{0}";
                }
                string list = string.Empty;
                foreach (string p in e.Items)
                {
                    list += "\"" + p + "\"" + " ";
                }
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Default.DiffExe,
                    Arguments = string.Format(args, list)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DuplicateCandidatesView1_OnItemOpen(object sender, ItemOpenedEventArgs e)
        {
            try
            {
                if (e.OpenLocation)
                {
                    if (File.Exists(e.Path))
                    {
                        Process.Start(Path.Combine(GetFolderPath(SpecialFolder.Windows), "explorer.exe"), "/select, \"" + e.Path + "\"");
                    }
                }
                else
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = e.Path,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SynoReportClient_Load(object sender, EventArgs e)
        {
            try
            {
                FileInfo source = null;
                string entropy = "Is a gift a gift without wrapping?";
                if (!string.IsNullOrEmpty(Default.DPAPIVector) && !string.IsNullOrWhiteSpace(Default.DPAPIVector))
                {
                    try
                    {
                        source = new FileInfo(Default.DPAPIVector);
                    }
                    catch (ArgumentException)
                    {
                        //don't care if it's not a filename
                    }

                    if (source != null && source.Exists)
                    {
                        using (var sr = new StreamReader(source.FullName))
                        {
                            entropy = sr.ReadToEnd();
                        }
                    }
                    else
                    {
                        entropy = Default.DPAPIVector;
                    }
                }

                WrappedPassword<DSMAuthentication>.SetEntropy(entropy);
                WrappedPassword<DSMAuthenticationKeyFile>.SetEntropy(entropy);
                WrappedPassword<DefaultProxy>.SetEntropy(entropy);

                CustomSettings.Initialize(GetSection<CustomSettings>);

                PopulateServerTree();

                Profile.LegendChanged += Profile_LegendChanged;
                volumeHistoricChart1.Configuration = Profile;
                volumePies.Configuration = Profile;


                duplicateCandidatesView1.MaximumComparable = Default.MaximumComparable;

                if (string.IsNullOrEmpty(Default.AutoRefreshServer) == false)
                {
                    string tag = Default.AutoRefreshServer;
                    if (Profile.DSMHosts.Items.ContainsKey(tag))
                    {
                        RefreshDataForHost(tag);
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
        private void ProgressUpdateFailed()
        {
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }
        private bool SelectHost(string hostName)
        {
            var host = Profile.DSMHosts.Items.TryGet(hostName);
            if (host != null)
            {
                selected = host;
                selected.StorePassPhrases = Default.StorePassPhrases;

                if (exclusion != null) exclusion.PropertyChanged -= Exclusion_PropertyChanged;
                exclusion = new DuplicateCandidatesExclusion<DSMHost>(selected, cfg => cfg.FilterDuplicates, (cfg, v) => cfg.FilterDuplicates = v);
                exclusion.PropertyChanged += Exclusion_PropertyChanged;
                return true;
            }
            return false;
        }
        private void SynoReportClient_CacheUpdate(string hostName)
        {

            try
            {
                Invoke(new Action(ProgressUpdateProcessing));

                if (SelectHost(hostName) == false)
                    throw new ArgumentException($"The requested name '{hostName}' cannot be found in the configuration.", nameof(hostName));

                SynoReportViaSSH connection = null;

                if (Default.UseProxy && selected.Proxy == null)
                {
                    connection = new SynoReportViaSSH(selected, new DefaultProxy());
                }
                else
                {
                    if (selected.Proxy != null)
                    {
                        connection = new SynoReportViaSSH(selected, selected.Proxy);
                    }
                    else
                    {
                        connection = new SynoReportViaSSH(selected);
                    }
                }

                connection.HostKeyChange += Connection_HostKeyChange;
                //connection.RmExecutionMode = Default.RmExecutionMode;


                cache = connection;

                if (!string.IsNullOrEmpty(Default.CacheFolder))
                {
                    cache.Path = Path.Combine(Default.CacheFolder, selected.Host);
                }
                else
                {
                    cache.Path = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), Application.ProductName, selected.Host);
                }
                var k = selected as IKeepDSMFiles;
                if (k.Custom)
                {
                    if (k.KeepAll == true)
                    {
                        cache.KeepAnalyzerDbCount = -1;
                    }
                    else
                    {
                        cache.KeepAnalyzerDbCount = k.KeepCount;
                    }
                }
                else
                {
                    if (Default.KeepAnalyzerDb == true)
                    {
                        cache.KeepAnalyzerDbCount = -1;
                    }
                    else
                    {
                        cache.KeepAnalyzerDbCount = Default.KeepAnalyzerDbCount;
                    }
                }

                cache.DownloadUpdate += Cache_StatusUpdate;
                connection.DownloadCSVFiles();

                connection.ScanCachedReports();

                if (CacheUpdateCompleted != null)
                {
                    var session = connection.Session;
                    var connectedHost = session.ConnectionInfo.Host;

                    CacheUpdateCompleted.Invoke($"{connectedHost} ({session.Version})");
                    duplicateCandidatesView1.HostName = connectedHost;
                }

                if (selected.StorePassPhrases)
                {
                    Profile.Save();
                }

                if (cache.GetReports(SynoReportType.DuplicateCandidates).Count > 0)
                {
                    dupes = cache.GetReport(SynoReportType.DuplicateCandidates) as SynoReportDuplicateCandidates;
                }

                DuplicatesAnalysisCompleted?.Invoke();

            }
            catch (SynoReportViaSSHLoginFailure ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                Invoke(new Action(ProgressUpdateFailed));
            }
        }

        private void Exclusion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var modified = sender as DSMHost;
            Debug.WriteLine($"Exclusion PropertyChanged {e.PropertyName}");

            var host = Profile.DSMHosts.Items.TryGet(modified.Host);

            Debug.WriteLine($"The TryGet method did {(host != modified ? "not " : string.Empty)}return the same DSMHost object.");

            host.FilterDuplicates = modified.FilterDuplicates;
            Profile.Save();
            Profile.Reload();

            PopulateDuplicatesTab();

        }
        private void DuplicateCandidatesView1_OnItemHide(object sender, ItemHiddenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"OnItemHide {e.Path}");
            exclusion.AddExclusion(e.Path);
            duplicateCandidatesView1.DataSource = dupes;

        }

        private void Connection_HostKeyChange(object sender, EventArgs e)
        {
            Profile.Save();
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
            using (var p = new Preferences())
            {
                p.ShowDialog();
            }
        }
        private void cmbFileDetails_SelectedIndexChanged(object sender, EventArgs e) => TimeStampTrackBar_ValueChanged(timestampTrackBarFileDetails, e);
        private void TimeStampTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var control = sender as TimeStampTrackBar;
            if (control is null) return;

            commonDateSelection = control.Value;
            PopulateFromTimeline(commonDateSelection.Value);
        }
        private void PopulateFromTimeline(DateTime ts)
        {
            if (cache is null) return;

            volumePies.DataSource = cache.GetReport(ts, SynoReportType.VolumeUsage, SynoReportType.ShareList) as IVolumePieChart;

            string value = cmbFileDetails.Text.ToLowerInvariant();
            object current;
            switch (value)
            {
                case "owners":
                    current = dataGridView1.GetSelection<ISynoReportOwnerDetail>();
                    setDataSource<ISynoReportOwnerDetail>(dataGridView1, ts, SynoReportType.FileOwner);
                    dataGridView1.TryReselection(current as IReadOnlyList<ISynoReportOwnerDetail>);
                    break;
                case "most modified":
                    current = dataGridView1.GetSelection<ISynoReportFileDetail>();
                    setDataSource<ISynoReportFileDetail>(dataGridView1, ts, SynoReportType.MostModified);
                    dataGridView1.TryReselection(current as IReadOnlyList<ISynoReportFileDetail>);
                    break;
                case "least modified":
                    current = dataGridView1.GetSelection<ISynoReportFileDetail>();
                    setDataSource<ISynoReportFileDetail>(dataGridView1, ts, SynoReportType.LeastModified);
                    dataGridView1.TryReselection(current as IReadOnlyList<ISynoReportFileDetail>);
                    break;
                default:
                    current = dataGridView1.GetSelection<ISynoReportGroupDetail>();
                    setDataSource<ISynoReportGroupDetail>(dataGridView1, ts, SynoReportType.FileGroup);
                    dataGridView1.TryReselection(current as IReadOnlyList<ISynoReportGroupDetail>);
                    break;
            }
        }
        private void setDataSource<T>(SynoReportDataGridView grid, DateTime ts, SynoReportType type) where T : class, ISynoReportDetail
        {
            if (cache != null)
            {
                grid.setDataSource<T>(cache.GetReport(ts, type));
            }
        }

        private void RefreshDataForHost(string tag)
        {
            NoDataNotification.Visible = false;
            HostInformationTabs.Visible = true;
            Task.Factory.StartNew(() => SynoReportClient_CacheUpdate(tag));
        }

        private void contextMenuHost_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string tag = (string)KnownHosts.SelectedNode.Tag;
            if (e.ClickedItem == refreshToolStripMenuItem)
            {
                RefreshDataForHost(tag);
            }
            else if (e.ClickedItem == removeServerToolStripMenuItem)
            {
                if (MessageBox.Show($"Are you sure you want to remove server '{tag}' from the list of servers?",
                    "Remove server",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2)

                    == DialogResult.Yes)
                {
                    Profile.DSMHosts.Items.Remove(tag);
                    Profile.Save();
                    PopulateServerTree();
                }

            }
            else if (e.ClickedItem == propertiesToolStripMenuItem)
            {
                if (SelectHost(tag))
                    using (var srv = new HostConfiguration(selected, exclusion))
                    {

                        srv.ShowDialog();
                        if (srv.Canceled == false)
                        {
                            Profile.Save();
                            Profile.Reload();
                        }
                    }
            }
        }

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var srv = new HostConfiguration())
            {
                srv.ShowDialog();
                if (srv.Canceled == false)
                {
                    if (Profile.DSMHosts.Items.ContainsKey(srv.Host.Host) == false)
                    {
                        Profile.DSMHosts.Items.Add(srv.Host);
                        Profile.Save();
                        PopulateServerTree();
                    }
                    else
                    {
                        MessageBox.Show($"There is already a configuration present for the host named '{srv.Host.Host}'",
                            "Duplicate host", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void PopulateServerTree()
        {
            KnownHosts.Nodes.Clear();
            KnownHosts.Nodes.Add("NAS");
            KnownHosts.Nodes[0].ContextMenuStrip = contextMenuAddServer;

            foreach (DSMHost h in Profile.DSMHosts.Items)
            {
                var node = KnownHosts.Nodes[0].Nodes.Add(h.Host, h.Host);
                node.ContextMenuStrip = contextMenuHost;
                node.Tag = h.Host;
            }
        }

        private void ProgressUpdate(SynoReportCacheDownloadEventArgs e)
        {
            KnownHosts.Enabled = false;
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

                case CacheStatus.Cleanup:
                    toolStripStatusLabel1.Text = "Removing Storage Analyzer database files...";
                    break;

                case CacheStatus.Downloading:
                    toolStripStatusLabel1.Text = $"Downloading files.. [{e.FilesFetched} of {e.TotalFiles}]";
                    toolStripProgressBar1.Minimum = 0;
                    toolStripProgressBar1.Maximum = e.TotalFiles;
                    toolStripProgressBar1.Value = e.FilesFetched;
                    break;
                default:

                    KnownHosts.Enabled = true;
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
            KnownHosts.SelectedNode.ToolTipText = version;

            volumeHistoricChart1.View = vhcViewMode.Shares;
            volumeHistoricChart1.DataSource = cache;

            timestampTrackBarPieCharts.DateRange = cache.DateRange;
            timestampTrackBarFileDetails.DateRange = cache.DateRange;

            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        private void PopulateDuplicatesTab()
        {
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            if (dupes != null)
            {
                duplicateCandidatesView1.ExclusionSource = exclusion;
                duplicateCandidatesView1.DataSource = dupes;
            }
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        private void KnownHosts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
        }
        private void KnownHosts_KeyPress(object sender, KeyPressEventArgs e)
        {
            var node = (sender as TreeView).SelectedNode;

            if (node is null) return;

            if (e.KeyChar == '\r')
            {
                e.Handled = true;

                if (node.Tag is null)
                {
                    addServerToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    RefreshDataForHost((string)node.Tag);
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportVolumeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialog("Save Volume report...", out string file))
            {
                (cache.GetReport(SynoReportType.VolumeUsage) as SynoReportVolumeUsage).WriteTimeLineData(file);
            }
        }

        private void exportSharesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialog("Save Shares report...", out string file))
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

        private void HostInformationTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 2:
                    if (commonDateSelection.HasValue)
                    {
                        timestampTrackBarPieCharts.Value = commonDateSelection.Value;
                    }
                    ToggleContentToolWindow();
                    break;

                case 3:
                    {
                        timestampTrackBarFileDetails.Value = commonDateSelection.Value;
                        contentTool?.Hide();
                    }
                    break;

                default:
                    contentTool?.Hide();
                    break;

            }
        }

        private void ContentWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            contentTool = null;
        }

        private void volumePies_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuPiecharts.Show(MousePosition);
        }

        private void showFreeVsUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showFreeVsUsedToolStripMenuItem.Checked = !showFreeVsUsedToolStripMenuItem.Checked;
            volumePies.PercentageFreeOnly = showFreeVsUsedToolStripMenuItem.Checked;
        }

        private void showContentTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showContentTypesToolStripMenuItem.Checked = !showContentTypesToolStripMenuItem.Checked;
            ToggleContentToolWindow();
        }
        private void ToggleContentToolWindow()
        {
            if (showContentTypesToolStripMenuItem.Checked)
            {
                if (contentTool is null)
                {
                    contentTool = new ContentToolWindow(ContentWindow_FormClosed);
                }
                contentTool?.Show();
            }
            else
            {
                contentTool?.Hide();
            }
        }
    }
}
