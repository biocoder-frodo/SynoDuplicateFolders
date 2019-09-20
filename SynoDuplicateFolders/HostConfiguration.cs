using System;
using SynoDuplicateFolders.Data.SecureShell;
using SynoDuplicateFolders.Extensions;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders
{

    internal partial class HostConfiguration : Form
    {
        private static readonly Regex portspec = new Regex("(.*/{0}):([0-9]+)$");

        public readonly DSMHost Host;

        public bool Canceled = false;
        public HostConfiguration()
        {
            InitializeComponent();

            Host = new DSMHost();

            txtSynoReportHome.Text = DSMHost.SynoReportHomeDefault(DSMHost.DefaultUserName);
            txtUser.Text = DSMHost.DefaultUserName;
            chkSynoReportHome.CheckState = CheckState.Unchecked;
            chkUser.CheckState = CheckState.Unchecked;
            txtPort.Text = Host.ElementInformation.Properties["port"].DefaultValue.ToString();
        }

        public HostConfiguration(DSMHost host)
        {
            InitializeComponent();

            Host = host;

            txtPassword.Text = "";
            txtUser.Text = Host.UserName.ToLower();
            txtHost.Text = Host.Host;
            txtSynoReportHome.Text = string.IsNullOrWhiteSpace(host.SynoReportHome) ? DSMHost.SynoReportHomeDefault(host.UserName) : host.SynoReportHome;
            chkSynoReportHome.CheckState = CheckState.Checked;
            if (txtSynoReportHome.Text == DSMHost.SynoReportHomeDefault(host.UserName)) chkSynoReportHome.CheckState = CheckState.Unchecked;
            txtPort.Text = host.Port.ToString();
        }
        private void HostConfiguration_Load(object sender, EventArgs e)
        {
            listView1.View = View.List;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Canceled = true;
            Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Host.Host = txtHost.Text;
            Host.Port = int.Parse(txtPort.Text);
            Host.UserName = txtUser.Text;
            DSMAuthentication method = null;

            Host.SynoReportHome = chkSynoReportHome.Checked ? txtSynoReportHome.Text : string.Empty;
            if (Validate())
            {
                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.None, chkAuthNone.Checked);

                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.KeyboardInteractive, chkKeyBoardInteractive.Checked);
                if (method != null)
                {
                    method.UserName = Host.UserName;
                }

                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.Password, chkPassword.Checked);
                if (method != null)
                {
                    method.UserName = Host.UserName;
                    method.Password = txtPassword.Text;
                }

                method = Host.UpdateAuthenticationMethod(DSMAuthenticationMethod.PrivateKeyFile, chkKeyFiles.Checked);
                if (method != null)
                {
                    method.UserName = Host.UserName;
                }
            }
            else
            {; }

            Hide();

        }

        private void chkUser_CheckedChanged(object sender, EventArgs e)
        {
            bool custom = chkUser.CheckState != CheckState.Unchecked;

            lblUser.Enabled = custom;
            txtUser.Enabled = custom;

            if (!custom)
            {
                txtUser.Text = "admin";
                chkSynoReportHome_CheckedChanged(sender, e);
            }
        }

        private void chkSynoReportHome_CheckedChanged(object sender, EventArgs e)
        {
            bool custom = chkSynoReportHome.CheckState != CheckState.Unchecked;
            lblReports.Enabled = custom;
            txtSynoReportHome.Enabled = custom;

            if (!custom)
            {
                txtSynoReportHome.Text = "/volume1/homes/" + txtUser.Text + "/synoreport/";
            }

        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            chkSynoReportHome_CheckedChanged(sender, e);
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
        }

        private void chkPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.Enabled = ((CheckBox)sender).Checked;
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
            foreach(int i in listView1.SelectedIndices)
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
    }
}
