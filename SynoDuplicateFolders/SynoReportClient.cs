using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Data.SecureShell;
using SynoDuplicateFolders.Extensions;
using SynoDuplicateFolders.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SynoDuplicateFolders.Configuration.UserSectionHandler;
using static SynoDuplicateFolders.Properties.CustomSettings;
using static SynoDuplicateFolders.Properties.Settings;
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
        private bool _PassPhraseUpdate = false;

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
        }

        private void OnDispose(bool disposing)
        {
            if (dupes != null) dupes.Dispose();
        }
        private void DuplicateCandidatesView1_OnItemStatusUpdate(object sender, ItemStatusUpdateEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Status;
        }

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

                volumeHistoricChart1.Configuration = Profile;
                chartGrid1.Configuration = Profile;

                duplicateCandidatesView1.MaximumComparable = Default.MaximumComparable;

                if (string.IsNullOrEmpty(Default.AutoRefreshServer) == false)
                {
                    string tag = Default.AutoRefreshServer;
                    if (Profile.DSMHosts.Items.ContainsKey(tag))
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
                    connection = new SynoReportViaSSH(selected, GetPassPhrase, GetInteractiveMethod, new DefaultProxy());
                }
                else
                {
                    if (selected.Proxy != null)
                    {
                        connection = new SynoReportViaSSH(selected, GetPassPhrase, GetInteractiveMethod, selected.Proxy);
                    }
                    else
                    {
                        connection = new SynoReportViaSSH(selected, GetPassPhrase, GetInteractiveMethod);
                    }
                }
                connection.HostKeyChange += Connection_HostKeyChange;
                connection.RmExecutionMode = Default.RmExecutionMode;

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
                    CacheUpdateCompleted.Invoke(string.Format("{0} ({1})", connection.Host, connection.Version));
                    duplicateCandidatesView1.HostName = connection.Host;
                }

                if (_PassPhraseUpdate)
                {
                    Profile.Save();
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
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                Invoke(new Action(ProgressUpdateFailed));
            }
        }

        private void Exclusion_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Exclusion PropertyChanged {e.PropertyName}");

            Profile.Save();
            Profile.Reload();

            PopulateDuplicatesTab();

        }
        private void DuplicateCandidatesView1_OnItemHide(object sender, ItemHiddenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"OnItemHide {e.Path}");
            exclusion.AddExclusion(e.Path);
        }

        private void Connection_HostKeyChange(object sender, EventArgs e)
        {
            Profile.Save();
        }

        private string GetPassPhrase(string fileName)
        {
            string result;
            using (PassPhrase dialog = new PassPhrase(fileName))
            {
                dialog.ShowDialog();
                _PassPhraseUpdate = Default.StorePassPhrases;
                result = dialog.Password;
            }
            return result;
        }
        private string GetInteractiveMethod(DSMKeyboardInteractiveEventArgs e)
        {
            List<string> banner = new List<string>();
            banner.AddRange(e.Banner.Split('\n'));
            banner.AddRange(e.Instruction.Split('\n'));

            string result;
            using (PassPhrase dialog = new PassPhrase(e.Username, banner.ToArray(), e.Id + ": " + e.Request))
            {
                dialog.ShowDialog();
                result = dialog.Password;
            }
            return result;
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

        private void timeStampTrackBar_Scroll(object sender, EventArgs e)
        {
            chartGrid1.DataSource = cache.GetReport(timeStampTrackBar.Value, SynoReportType.VolumeUsage, SynoReportType.ShareList) as IVolumePieChart;
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
        private void setDataSource<T>(SynoReportDataGridView grid, DateTime ts, SynoReportType type) where T : class, ISynoReportDetail
        {
            if (cache != null)
            {
                grid.setDataSource<T>(cache.GetReport(ts, type));
            }
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string tag = (string)KnownHosts.SelectedNode.Tag;
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
                        MessageBox.Show(string.Format("There is already a configuration present for the host named '{0}'", srv.Host.Host),
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
            KnownHosts.Nodes[0].ContextMenuStrip = contextMenuStrip1;

            foreach (DSMHost h in Profile.DSMHosts.Items)
            {
                var node = KnownHosts.Nodes[0].Nodes.Add(h.Host, h.Host);
                node.ContextMenuStrip = contextMenuStrip2;
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
                    toolStripStatusLabel1.Text = string.Format("Downloading files.. [{0} of {1}]", e.FilesFetched, e.TotalFiles);
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

            volumeHistoricChart1.View = vhcViewMode.VolumeTotals;
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
                duplicateCandidatesView1.ExclusionSource = exclusion;
                duplicateCandidatesView1.DataSource = dupes;
            }
            ProgressUpdate(new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        private void KnownHosts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
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

    }
}
