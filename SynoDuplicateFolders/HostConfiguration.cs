using System;
using SynoDuplicateFolders.Data.SecureShell;
using SynoDuplicateFolders.Extensions;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static SynoDuplicateFolders.Properties.Settings;
using SynoDuplicateFolders.Data.Core;
using System.Linq;
using System.Collections.Generic;
using SynoDuplicateFolders.Controls;

namespace SynoDuplicateFolders
{

    internal partial class HostConfiguration : Form
    {
        private static readonly Regex portspec = new Regex("(.*/{0}):([0-9]+)$");
        private static readonly Regex pathspec = new Regex(@"^((\/volume[0-9]+\/homes\/[a-z0-9]+\/?)(synoreport\/?){0,1})|^(\/volume[0-9]+\/[\/a-z0-9]+)");
        private static readonly Regex NAME_REGEX = new Regex(@"^[a-z][-a-z0-9]*");

        public readonly DSMHost Host;
        private bool passwordDirty = false;
        private bool initialization = true;
        public bool Canceled = false;

        private DuplicateCandidatesExclusion<DSMHost> exclusion;

        private void InitializeComponent(bool existing)
        {

            txtPort.Text = (existing ? Host.Port : Host.ElementInformation.Properties["port"].DefaultValue).ToString();

            txtUser.Text = existing ? Host.UserName : DSMHost.DefaultUserName;

            txtSynoReportHome.Text = existing ? string.IsNullOrWhiteSpace(Host.SynoReportHome)
                                                ? DSMHost.SynoReportHomeDefault(Host.UserName)
                                                : Host.SynoReportHome
                                              : DSMHost.SynoReportHomeDefault(DSMHost.DefaultUserName);

            if (existing)
            {
                txtHost.Text = Host.Host;
                chkUser.Checked = !txtUser.Text.Equals(DSMHost.DefaultUserName);
                chkSynoReportHome.Checked = !txtSynoReportHome.Text.Equals(DSMHost.SynoReportHomeDefault(Host.UserName));               
            }
            else
            {
                chkUser.CheckState = CheckState.Unchecked;
                chkSynoReportHome.CheckState = CheckState.Unchecked;
            }

            chkKeep.Checked = Host.KeepDsmFilesCustom;

            txtKeep.Text = Host.KeepDsmCount.ToString();
            optAnalyzerDbKeep.Checked = Host.KeepAllDsmFiles;
            optAnalyzerDbRemove.Checked = !Host.KeepAllDsmFiles;
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
                        txtPassword.Text = "thisisnotyourpassword";
                        break;
                    // if you press Ok with a saved password, you have just reset your password to 'blank', unless you typed a new one.
                    default:
                        break;
                }
            }
            initialization = false;
        }

        public HostConfiguration(DSMHost host, DuplicateCandidatesExclusion<DSMHost> candidatesExclusion)
        {
            InitializeComponent();
            Host = host;
            exclusion = candidatesExclusion;
            InitializeComponent(true);
        }
        public HostConfiguration()
        {
            InitializeComponent();
            Host = new DSMHost();
            exclusion = null;
            InitializeComponent(false);
        }
        private void HostConfiguration_Load(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Canceled = true;
            //if (exclusion != null) exclusion.PropertyChanged -= Exclusion_PropertyChanged;
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

            if (chkSynoReportHome.Checked)
            {
                if (ValidateHomePath())
                {
                    if (!txtSynoReportHome.Text.EndsWith("/")) txtSynoReportHome.Text += "/";

                    if (Host.SynoReportHome.Equals(txtSynoReportHome.Text) == false && txtSynoReportHome.Text.EndsWith("/synoreport/"))
                    {

                        if (MessageBox.Show(string.Format("Are you sure your Storage Analyzer reports can be found in '{0}'?", txtSynoReportHome.Text + "synoreport/"),
                            "Please confirm path",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
            }


            Host.KeepDsmFilesCustom = chkKeep.Checked;
            Host.KeepDsmCount = int.Parse(txtKeep.Text);
            Host.KeepAllDsmFiles = optAnalyzerDbKeep.Checked;

            Host.Host = txtHost.Text;
            Host.Port = int.Parse(txtPort.Text);
            Host.UserName = txtUser.Text;
            DSMAuthentication method = null;

            Host.SynoReportHome = chkSynoReportHome.Checked ? txtSynoReportHome.Text : string.Empty;
            if (Validate())
            {
                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.None, chkAuthNone.Checked);

                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.KeyboardInteractive, chkKeyBoardInteractive.Checked);

                if (passwordDirty)
                {
                    method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.Password, chkPassword.Checked);
                    if (method != null)
                    {
                        method.Password = txtPassword.Text;
                    }
                }
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
            }
            else
            {
                ;
            }

            //if (exclusion != null) exclusion.PropertyChanged -= Exclusion_PropertyChanged;

            Hide();

        }

        private void chkKeyBoardInteractive_CheckedChanged(object sender, EventArgs e)
        {
            ;
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
        private void chkUser_CheckedChanged(object sender, EventArgs e)
        {
            bool custom = chkUser.CheckState != CheckState.Unchecked;//&& txtUser.Text.Equals(DSMHost.DefaultUserName)==false;

            lblUser.Enabled = custom;
            txtUser.Enabled = custom;

            if (!custom)
            {
                txtUser.Text = DSMHost.DefaultUserName;
            }
            else
            {
                txtUser.Focus();
            }
            chkSynoReportHome_CheckedChanged(sender, e);
        }
        private void txtUser_Leave(object sender, EventArgs e)
        {
            chkSynoReportHome_CheckedChanged(sender, e);
        }
        private void txtUser_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = !NAME_REGEX.IsMatch((sender as TextBox).Text);

            if (string.IsNullOrWhiteSpace((sender as TextBox).Text) == true)
            {
                e.Cancel = false;
                chkUser.Checked = false;
            }
            if (!e.Cancel && (sender as TextBox).Text.Equals(DSMHost.DefaultUserName))
            {
                chkUser.Checked = false;
            }
            Console.WriteLine((sender as TextBox).Name + " CancelEventArgs e.Cancel = " + e.Cancel);
        }

        private void chkSynoReportHome_CheckedChanged(object sender, EventArgs e)
        {
            bool custom = chkSynoReportHome.CheckState != CheckState.Unchecked;
            lblReports.Enabled = custom;
            txtSynoReportHome.Enabled = custom;

            if (!custom)
            {
                txtSynoReportHome.Text = DSMHost.SynoReportHomeDefault(txtUser.Text);
            }
            else
            {
                txtSynoReportHome.Focus();
            }

        }
        private void txtSynoReportHome_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !ValidateHomePath();
            if (!e.Cancel && (sender as TextBox).Text.Equals(DSMHost.SynoReportHomeDefault(txtUser.Text)))
            {
                if (chkSynoReportHome.Checked)
                    chkSynoReportHome.Checked = false;
            }
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            if (txtHost.Text.EndsWith(":", StringComparison.InvariantCulture) == false && portspec.IsMatch(txtHost.Text))
            {
                Match m = portspec.Match(txtHost.Text);
                txtPort.Text = m.Groups[2].Value;
                txtHost.Text = m.Groups[1].Value;
                txtPort.SelectionStart = txtPort.Text.Length;
                txtPort.Focus();
            }
            this.Text = string.IsNullOrWhiteSpace(txtHost.Text) ? "New host configuration" : $"Host configuration of {txtHost.Text}";
        }

        private void chkPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = ((CheckBox)sender).Checked;
            passwordDirty = true;
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
            string filename;
            if (OpenDialog("Open a key file", out filename))
            {
                listView1.Items.Add(filename);
            }
        }

        private void btnKeyFileRemove_Click(object sender, EventArgs e)
        {
            foreach (int i in listView1.SelectedIndices)
            {
                listView1.Items.Remove(listView1.Items[i]);
            }
        }
        private bool OpenDialog(string title, out string filename)
        {
            DialogResult r;
            filename = string.Empty;

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
                filename = openFileDialog1.FileName;
            }
            return r == DialogResult.OK;
        }


        private bool _allowedKeyPress;

        private void txtHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            _allowedKeyPress = true;

            if (e.KeyChar == '.' || e.KeyChar == ':')
            {
                _allowedKeyPress = false;
                if (txtHost.Text.EndsWith(e.KeyChar.ToString()) == false)
                    _allowedKeyPress = true;
            }
            if (!_allowedKeyPress)
                e.Handled = true;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') _allowedKeyPress = true;
            if (!_allowedKeyPress)
                e.Handled = true;
        }

        private void txtHost_KeyDown(object sender, KeyEventArgs e)
        {
            _allowedKeyPress = e.KeyCode == Keys.Back;
        }
        private void txtPort_KeyDown(object sender, KeyEventArgs e)
        {
            _allowedKeyPress = e.KeyCode == Keys.Back;
        }

        private void txtKeep_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int value = 0;
            int.TryParse(txtKeep.Text, out value);
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

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

            if (initialization == false)
            {
                passwordDirty = true;
            }
        }
    }
}
