using DiskStationManager.SecureShell;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Data.Core;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static SynoDuplicateFolders.Properties.Settings;

namespace SynoDuplicateFolders
{

    internal partial class HostConfiguration : Form
    {
        private static readonly Regex pathspec = new Regex(@"^((\/volume[0-9]+\/homes\/[a-z0-9]+\/?)(synoreport\/?){0,1})|^(\/volume[0-9]+\/[\/a-z0-9]+)");
        private static readonly Regex NAME_REGEX = new Regex(@"^[a-z][-a-z0-9]*");

        public readonly DSMHost Host;
        public bool Canceled = false;

        private bool initialization = true;

        private readonly DuplicateCandidatesExclusion<DSMHost> exclusion;

        private void InitializeComponent(bool existing)
        {
            radUserCustom.Checked = true;
            radFolderCustom.Checked = true;

            hostTextBox.Port = existing ? Host.Port : (int?)null;

            txtUser.Text = existing ? Host.UserName : DSMHost.DefaultUserName;

            if (existing)
            {
                hostTextBox.HostName = Host.Host;
                radUserDefault.Checked = IsDefaultUserNameSet;
                radFolderDefault.Checked = string.IsNullOrWhiteSpace(Host.SynoReportHome);
                txtSynoReportHome.Text = radFolderDefault.Checked ? DSMHost.SynoReportHomeDefault(Host.UserName) : Host.SynoReportHome;
            }
            else
            {
                radUserDefault.Checked = true;
                radFolderDefault.Checked = true;
                txtSynoReportHome.Text = DSMHost.SynoReportHomeDefault(DSMHost.DefaultUserName);
            }
            txtSynoReportHome.Enabled = IsDefaultFolderSet == false;
            txtUser.Enabled = IsDefaultUserNameSet == false;

            chkKeep.Checked = Host.Custom;

            txtKeep.Text = Host.KeepCount.ToString();
            optAnalyzerDbKeep.Checked = Host.KeepAll;
            optAnalyzerDbRemove.Checked = !Host.KeepAll;
            btnDupeRemoveAll.Enabled = false;

            if (exclusion != null)
            {
                btnDupeRemoveAll.Enabled = exclusion.Paths.Any();
                exclusion.Paths.ToList().ForEach(file => lstIgnoreDupes.Items.Add(file));
            }

            chkKeep_CheckedChanged(null, null);

            for (int i = 0; i < Host.AuthenticationSection.Count; i++)
            {
                var am = Host.AuthenticationSection[i];
                switch (am.Method)
                {
                    case DSMAuthenticationMethod.PrivateKeyFile:
                        {
                            chkKeyFiles.Checked = true;
                            for (int k = 0; k < am.AuthenticationKeys.Items.Count; k++)
                            {
                                var pkf = am.AuthenticationKeys.Items[k];
                                listView1.Items.Add(pkf.FileName);
                            }
                        }
                        break;

                    case DSMAuthenticationMethod.KeyboardInteractive:
                        chkKeyBoardInteractive.Checked = true;
                        break;

                    case DSMAuthenticationMethod.Password:
                        chkPassword.Checked = true;

                        break;
                    // if you press Ok with a saved password, you have just reset your password to 'blank', unless you typed a new one.
                    default:
                        break;
                }
            }

            SetDialogtTitle(hostTextBox.HostName);

            initialization = false;
        }
        
        private void hostTextBox_HostNameChange(object sender, EventArgs e) => SetDialogtTitle(hostTextBox.HostName);
        private void SetDialogtTitle(string host) => this.Text = string.IsNullOrWhiteSpace(host) ? "New host configuration" : $"Host configuration of {host}";

        public HostConfiguration(DSMHost host, DuplicateCandidatesExclusion<DSMHost> candidatesExclusion)
        {
            InitializeComponent();
            Host = host;
            exclusion = candidatesExclusion;
            InitializeComponent(true);

            if (exclusion.Paths.Count > 0)
            {
                tabControl1.SelectedIndex = 1;
            }
        }
        public HostConfiguration()
        {
            InitializeComponent();
            Host = new DSMHost();
            exclusion = null;
            InitializeComponent(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Canceled = true;
            Hide();
        }

        private bool ValidateHomePath()
        {
            string p1 = txtSynoReportHome.Text;
            var m1 = pathspec.Match(p1); ;
            if (m1.Success && (m1.Groups[1].Value.Equals(p1) || m1.Groups[4].Value.Equals(p1)))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please enter a pathname with only letters and numbers.\r\nThe path must begin with \\volume? ...", "Absolute pathname");
                return false;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {

            if (radUserCustom.Checked)
            {
                if (ValidateHomePath())
                {
                    if (!txtSynoReportHome.Text.EndsWith("/")) txtSynoReportHome.Text += "/";

                    if (Host.SynoReportHome.Equals(txtSynoReportHome.Text) == false && txtSynoReportHome.Text.EndsWith("/synoreport/"))
                    {

                        if (MessageBox.Show($"Are you sure your Storage Analyzer reports can be found in '{txtSynoReportHome.Text + "synoreport/"}'?",
                            "Please confirm path",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
            }

            if (Validate())
            {
                DSMAuthentication method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.Password, chkPassword.Checked);
                if (method != null)
                {
                    using (var pw = new PassPhrase(txtUser.Text))
                    {
                        pw.ShowDialog();
                        if (pw.Canceled) return;

                        method.Password = pw.Password;
                    }

                }
                _ = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.None, chkAuthNone.Checked);

                _ = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.KeyboardInteractive, chkKeyBoardInteractive.Checked);

                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.PrivateKeyFile, chkKeyFiles.Checked);
                if (method != null)
                {
                    method.AuthenticationKeys.Items.Clear();

                    foreach (ListViewItem k in listView1.Items)
                    {
                        var kf = new DSMAuthenticationKeyFile()
                        {
                            FileName = k.Text
                        };
                        method.AuthenticationKeys.Items.Add(kf);
                    }
                }

                Host.Custom = chkKeep.Checked;
                Host.KeepCount = int.Parse(txtKeep.Text);
                Host.KeepAll = optAnalyzerDbKeep.Checked;

                Host.Host = hostTextBox.HostName;
                Host.Port = hostTextBox.Port.HasValue ? hostTextBox.Port.Value : Host.GetDefaultValueAttribute<int>(nameof(Host.Port));

                Host.UserName = txtUser.Text;
                Host.SynoReportHome = radFolderCustom.Checked ? txtSynoReportHome.Text : string.Empty;

                Hide();
            }
            else
            {
                ;
            }
        }
        private void chkKeyBoardInteractive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKeyBoardInteractive.Checked) chkPassword.Checked = false;
        }
        private void chkKeep_CheckedChanged(object sender, EventArgs e)
        {
            bool custom = chkKeep.CheckState != CheckState.Unchecked;

            optAnalyzerDbKeep.Enabled = custom;
            optAnalyzerDbRemove.Enabled = custom;
            txtKeep.Enabled = custom;

            if (!custom)
            {
                txtKeep.Text = Default.KeepAnalyzerDbCount.ToString();
                optAnalyzerDbKeep.Checked = Default.KeepAnalyzerDb;
                optAnalyzerDbRemove.Checked = !Default.KeepAnalyzerDb;
            }
        }

        private void optAnalyzerDbRemove_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = optAnalyzerDbRemove.Checked;
            txtKeep.Enabled = enable;
        }

        private void txtUser_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !NAME_REGEX.IsMatch((sender as TextBox).Text);

            if (string.IsNullOrWhiteSpace((sender as TextBox).Text) == true)
            {
                e.Cancel = false;
                radUserDefault.Checked = true;
            }
            if (!e.Cancel && (sender as TextBox).Text.Equals(DSMHost.DefaultUserName))
            {
                radUserDefault.Checked = true;
            }
            System.Diagnostics.Debug.WriteLine((sender as TextBox).Name + " CancelEventArgs e.Cancel = " + e.Cancel);
        }

        private void txtSynoReportHome_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !ValidateHomePath();
            if (!e.Cancel && (sender as TextBox).Text.Equals(DSMHost.SynoReportHomeDefault(txtUser.Text)))
            {
                if (radFolderCustom.Checked)
                    radFolderDefault.Checked = true;
            }
        }
        private void chkKeyFiles_CheckedChanged(object sender, EventArgs e)
        {
            bool check = ((CheckBox)sender).Checked;
            listView1.Enabled = check;
            btnKeyFileAdd.Enabled = check;
            btnKeyFileRemove.Enabled = check;
        }

        private void btnKeyFileAdd_Click(object sender, EventArgs e)
        {
            if (OpenDialog("Open a key file", out FileInfo filename))
            {
                listView1.Items.Add(filename.FullName);
            }
        }

        private void btnKeyFileRemove_Click(object sender, EventArgs e)
        {
            foreach (int i in listView1.SelectedIndices)
            {
                listView1.Items.Remove(listView1.Items[i]);
            }
        }
        private bool OpenDialog(string title, out FileInfo filename)
        {
            DialogResult r;
            filename = null;

            openFileDialog1.AddExtension = true;
            openFileDialog1.FileName = "";
            openFileDialog1.DefaultExt = "key";
            openFileDialog1.Filter = "All files (*.*)|*.*|Private Key Files (*.key)|*.key";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.SupportMultiDottedExtensions = true;
            openFileDialog1.Title = title;
            r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                filename = new FileInfo(openFileDialog1.FileName);
            }
            return r == DialogResult.OK;
        }

        private void txtKeep_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int.TryParse(txtKeep.Text, out int value);
            e.Cancel = value < 1;
        }

        private void btnDupeRemove_Click(object sender, EventArgs e)
        {
            exclusion.RemoveExclusion(lstIgnoreDupes.SelectedItem as string);

            lstIgnoreDupes.Items.RemoveAt(lstIgnoreDupes.SelectedIndex);
            btnDupeRemove.Enabled = false;
            if (lstIgnoreDupes.Items.Count == 0) btnDupeRemoveAll.Enabled = false;
        }

        private void btnDupeRemoveAll_Click(object sender, EventArgs e)
        {
            exclusion.RemoveAllExclusions();

            lstIgnoreDupes.Items.Clear();
            btnDupeRemoveAll.Enabled = false;
        }

        private void lstIgnoreDupes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDupeRemove.Enabled = true;
        }

        private void radioButtonDefault_MouseHover(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;

            if (rb == radFolderDefault && radFolderCustom.Checked && IsDefaultFolderSet == false)
                lblReportFolderHint.Text = DSMHost.SynoReportHomeDefault(txtUser.Text);
            if (rb == radUserDefault && radUserCustom.Checked && IsDefaultUserNameSet == false)
                lblUserHint.Text = DSMHost.DefaultUserName;

        }

        private void radioButtonDefault_MouseLeave(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == radFolderDefault)
                lblReportFolderHint.Text = string.Empty;
            if (rb == radUserDefault)
                lblUserHint.Text = string.Empty;
        }
        private bool IsDefaultUserNameSet => txtUser.Text.Equals(DSMHost.DefaultUserName);
        private bool IsDefaultFolderSet => DSMHost.SynoReportHomeDefault(txtUser.Text) == txtSynoReportHome.Text || string.IsNullOrWhiteSpace(txtSynoReportHome.Text);

        private void radUserDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (initialization == false)
            {
                var text = txtUser;
                var self = radUserCustom as RadioButton;
                bool custom = self.Checked;

                if (custom)
                {
                    text.Focus();
                }
                else
                {
                    text.Text = DSMHost.DefaultUserName;
                }

                text.Enabled = custom;
            }
        }

        private void radFolderDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (initialization == false)
            {
                var text = txtSynoReportHome;
                var self = radFolderCustom as RadioButton;
                bool custom = self.Checked;

                if (custom)
                {
                    text.Focus();
                }
                else
                {
                    text.Text = DSMHost.SynoReportHomeDefault(txtUser.Text);
                }

                text.Enabled = custom;
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (radFolderDefault.Checked) txtSynoReportHome.Text = DSMHost.SynoReportHomeDefault(txtUser.Text);
        }
        private void chkPassword_CheckedChanged(object sender, EventArgs e)
        {
            var control = sender as CheckBox;
            txtPassword.Text = control.Checked ? "thisisnotyourpassword" : string.Empty;
        }
    }
}