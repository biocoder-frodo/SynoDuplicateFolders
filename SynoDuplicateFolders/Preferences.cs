using DiskStationManager.SecureShell;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using static SynoDuplicateFolders.Properties.CustomSettings;
using static SynoDuplicateFolders.Properties.Settings;
using static System.Environment;

namespace SynoDuplicateFolders
{
    public partial class Preferences : Form
    {
        private readonly DefaultProxy _proxy = new DefaultProxy();

        private bool dirty_custom = false;
        private string server = string.Empty;
        private string folder = string.Empty;
        private bool use_proxy = false;
        private bool store_passphrases = false;

        private string proxy_host = string.Empty;
        private int proxy_port = 8080;
        private string proxy_user = string.Empty;
        private string proxy_password = string.Empty;
        private string proxy_type = string.Empty;
        private string dpapi = string.Empty;

        private string diffexe = string.Empty;
        private string diffexeargs = string.Empty;
        private int max_compare = 3;

        ConsoleCommandMode mode = ConsoleCommandMode.InteractiveSudo;

        bool keep = true;

        int keepCount = 1;

        internal Preferences()
        {
            InitializeComponent();
            DataToControls(true);
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            ControlsToData();

            if (!Default.AutoRefreshServer.Equals(server)
                || !Default.DPAPIVector.Equals(dpapi)
                || !Default.DiffExe.Equals(diffexe)
                || !Default.DiffArgs.Equals(diffexeargs)
                || !Default.MaximumComparable.Equals(max_compare)
                || !Default.CacheFolder.Equals(folder)
                || !Default.KeepAnalyzerDb.Equals(keep)
                || !Default.KeepAnalyzerDbCount.Equals(keepCount)
                || !Default.RmExecutionMode.Equals(mode)
                || !Default.UseProxy.Equals(use_proxy)
                || !Default.StorePassPhrases.Equals(store_passphrases)
                || !_proxy.ProxyType.Equals(proxy_type)
                || !_proxy.Host.Equals(proxy_host)
                || !_proxy.UserName.Equals(proxy_user)
                || !_proxy.Password.Equals(proxy_password)
                || !_proxy.Port.Equals(proxy_port)
                )

            {
                Default.CacheFolder = folder;
                Default.DPAPIVector = dpapi;
                Default.DiffExe = diffexe;
                Default.DiffArgs = diffexeargs;
                Default.MaximumComparable = max_compare;
                Default.AutoRefreshServer = server;
                Default.KeepAnalyzerDb = keep;
                Default.KeepAnalyzerDbCount = keepCount;
                Default.RmExecutionMode = mode;
                Default.UseProxy = use_proxy;
                Default.StorePassPhrases = store_passphrases;
                _proxy.ProxyType = proxy_type;
                _proxy.Host = proxy_host;
                _proxy.UserName = proxy_user;
                _proxy.Password = proxy_password;
                _proxy.Port = proxy_port;
                Default.Save();
                Default.Reload();
                Profile.Reload();
            }
            if (dirty_custom)
            {
                Profile.Save();
                Profile.Reload();
                Default.Reload();
                dirty_custom = false;
            }
        }
        #region Data exchange on controls

        private void DataToControls(bool updateHostList)
        {
            if (updateHostList) //only done once
            {
                comboBox1.Items.Clear();
                foreach (DSMHost h in Profile.DSMHosts.Items)
                    comboBox1.Items.Add(h.Host);
                dataGridView1.DataSource = Profile.List;
            }

            if (!string.IsNullOrEmpty(Default.AutoRefreshServer))
            {
                if (Profile.DSMHosts.Items.ContainsKey(Default.AutoRefreshServer))
                {
                    checkBox1.CheckState = CheckState.Checked;
                    comboBox1.Enabled = true;
                    comboBox1.SelectedIndex = -1;
                    comboBox1.Text = Default.AutoRefreshServer;
                }
            }
            else
            {
                checkBox1.CheckState = CheckState.Unchecked;
                comboBox1.Enabled = false;
                comboBox1.SelectedIndex = -1;
            }

            if (!string.IsNullOrEmpty(Default.CacheFolder))
            {
                chkCacheFolder.CheckState = CheckState.Checked;
                txtFolder.Text = Default.CacheFolder;
            }
            else
            {
                chkCacheFolder.CheckState = CheckState.Unchecked;
                txtFolder.Text = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), Application.ProductName);

            }

            optUseStoredPassPhrases.Checked = Default.StorePassPhrases;
            optPassPhraseInteractive.Checked = !Default.StorePassPhrases;

            optAnalyzerDbKeep.Checked = Default.KeepAnalyzerDb;
            optAnalyzerDbRemove.Checked = !Default.KeepAnalyzerDb;
            txtKeep.Text = Default.KeepAnalyzerDbCount.ToString();

            txtDPAPI.Text = Default.DPAPIVector;

            txtDiffExeArgs.Text = Default.DiffArgs;
            txtDiffTool.Text = Default.DiffExe;
            cmbMaxCompare.Text = Default.MaximumComparable.ToString();

            optLoginAsRoot.Checked = true;
            optSudo.Checked = Default.RmExecutionMode == ConsoleCommandMode.Sudo;
            optInteractiveSudo.Checked = Default.RmExecutionMode == ConsoleCommandMode.InteractiveSudo;

            txtProxyHost.Text = _proxy.Host;
            txtProxyPort.Text = _proxy.Port.ToString();
            txtProxyUser.Text = _proxy.UserName;
            cmbProxy.Text = _proxy.ProxyType;

            ProxyControls(Default.UseProxy);

        }

        private void ControlsToData()
        {
            if (checkBox1.CheckState == CheckState.Checked && comboBox1.SelectedIndex != -1)
            {
                server = comboBox1.Text;
            }

            if (chkCacheFolder.CheckState == CheckState.Checked)
            {
                folder = txtFolder.Text;
                DirectoryInfo default_profile = new DirectoryInfo(Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), Application.ProductName));
                DirectoryInfo custom_profile = new DirectoryInfo(folder);
                folder = default_profile.FullName;
                if (custom_profile.Exists && custom_profile.FullName.Equals(folder) == false)
                {
                    folder = custom_profile.FullName;
                }
                else
                {
                    folder = string.Empty;
                }
            }

            keep = optAnalyzerDbKeep.Checked;
            keepCount = int.Parse(txtKeep.Text);

            mode = ConsoleCommandMode.Directly;
            if (optSudo.Checked) mode = ConsoleCommandMode.Sudo;
            if (optInteractiveSudo.Checked) mode = ConsoleCommandMode.InteractiveSudo;

            proxy_host = txtProxyHost.Text;
            proxy_port = int.Parse(txtProxyPort.Text);
            proxy_user = txtProxyUser.Text;
            proxy_password = txtProxyPassword.Text;
            proxy_type = cmbProxy.Text;
            use_proxy = optProxy.Checked;
            store_passphrases = optUseStoredPassPhrases.Checked;

            dpapi = txtDPAPI.Text;

            diffexe = txtDiffTool.Text;
            diffexeargs = txtDiffExeArgs.Text;
            max_compare = int.Parse(cmbMaxCompare.Text);

        }
        #endregion

        #region Controls interaction
        private void chkCacheFolder_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = ((CheckBox)sender).Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = ((CheckBox)sender).Checked;
        }

        private void optAnalyzerDbRemove_CheckedChanged(object sender, EventArgs e)
        {
            txtKeep.Enabled = ((RadioButton)sender).Checked;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                var r = ((DataGridView)sender).Rows[e.RowIndex];
                var d = r.DataBoundItem as ITaggedColor;
                using (var c = new ColorSelection(ChartLegend.PaletteMap, CustomSettings.Profile.ChartLegends, d.Color))
                {
                    c.ShowDialog();
                    if (c.Canceled == false)
                    {
                        d.Color = c.Selection;
                        dirty_custom = true;
                    }
                }
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var r = ((DataGridView)sender).Rows[e.RowIndex];
            var d = r.DataBoundItem as ITaggedColor;
            var c = r.Cells["colorDataGridViewTextBoxColumn"];

            c.Style.BackColor = d.Color;
            c.Style.SelectionBackColor = d.Color;
            c.Style.SelectionForeColor = d.Color;
        }

        private void optProxy_CheckedChanged(object sender, EventArgs e)
        {
            ProxyControls(((RadioButton)sender).Checked);
        }

        private void ProxyControls(bool enabled)
        {
            optProxy.Checked = enabled;
            txtProxyHost.Enabled = enabled;
            txtProxyPort.Enabled = enabled;
            txtProxyUser.Enabled = enabled;
            txtProxyPassword.Enabled = enabled;
            txtProxyPassword.Enabled = enabled;
            lblProxyHost.Enabled = enabled;
            lblProxyPassword.Enabled = enabled;
            lblProxyPort.Enabled = enabled;
            lblProxyUser.Enabled = enabled;
            cmbProxy.Enabled = enabled;
        }

        #endregion

    }
}
