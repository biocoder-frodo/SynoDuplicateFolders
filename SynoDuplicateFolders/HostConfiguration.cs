using System;
using SynoDuplicateFolders.Properties;
using SynoDuplicateFolders.Extensions;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders
{

    internal partial class HostConfiguration : Form
    {
        private static readonly Regex portspec = new Regex("(.*/{0}):([0-9]+)$");

        public readonly DSMHost Host;
        private readonly WrappedPassword<DSMHost> proxy;
        public bool Canceled = false;
        public HostConfiguration()
        {
            InitializeComponent();

            Host = new DSMHost();
            proxy = new WrappedPassword<DSMHost>("Password", Host);

            txtSynoReportHome.Text = "/homes/admin/synoreport";
            txtUser.Text = "admin";
            chkSynoReportHome.CheckState = CheckState.Unchecked;
            chkUser.CheckState = CheckState.Unchecked;
            txtPort.Text = Host.ElementInformation.Properties["port"].DefaultValue.ToString();
        }

        public HostConfiguration(DSMHost host)
        {
            InitializeComponent();

            Host = host;
            proxy = new WrappedPassword<DSMHost>("Password", Host);
            txtPassword.Text = "";
            txtUser.Text = Host.UserName.ToLower();
            txtHost.Text = Host.Host;
            txtSynoReportHome.Text = string.IsNullOrWhiteSpace(host.SynoReportHome) ? "/homes/" + host.UserName + "/synoreport" : host.SynoReportHome;
            txtPort.Text = host.Port.ToString();
        }
        private void HostConfiguration_Load(object sender, EventArgs e)
        {

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
            proxy.Password = txtPassword.Text;
            Host.SynoReportHome = chkSynoReportHome.Checked ?  txtSynoReportHome.Text : string.Empty ;
            if (Validate())
            {; }
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
                txtSynoReportHome.Text = "/homes/" + txtUser.Text + "/synoreport";                
            }

        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            chkSynoReportHome_CheckedChanged(sender, e);
        }

        private void txtSynoReportHome_Leave(object sender, EventArgs e)
        {

        }


        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            if (txtHost.Text.EndsWith(":", StringComparison.InvariantCulture)==false &&  portspec.IsMatch(txtHost.Text))
            {
                Match m = portspec.Match(txtHost.Text);
                txtPort.Text = m.Groups[2].Value;
                txtHost.Text = m.Groups[1].Value;                
                txtPort.SelectionStart = txtPort.Text.Length;
                txtPort.Focus();
            }

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtPort_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
          //  e.Cancel = true;

        }
    }
}
