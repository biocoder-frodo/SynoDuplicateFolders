using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static SynoDuplicateFolders.Data.Core.Extensions;

namespace SynoDuplicateFolders.Controls
{
    public partial class HostTextBox : UserControl
    {
        public event EventHandler HostNameChange;

        private static readonly Regex portspec = new Regex("(.*/{0}):([0-9]+)$", RegexOptions.Compiled);

        private bool _allowedKeyPress;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? Port
        {
            get => string.IsNullOrWhiteSpace(txtPort.Text) ? (int?)null : int.Parse(txtPort.Text);
            set => txtPort.Text = value.HasValue ? value.Value.ToString() : string.Empty;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HostName { get => txtHost.Text; set => txtHost.Text = value; }
        public void OnHostNameChange(object sender, EventArgs e)
        {
            HostNameChange?.Invoke(sender, e);
        }
        public HostTextBox()
        {
            InitializeComponent();

            HostNameLabelText = this.GetDefaultValueAttribute<string>(nameof(HostNameLabelText));
            PortLabelText = this.GetDefaultValueAttribute<string>(nameof(PortLabelText));
            PortTextWidth = this.GetDefaultValueAttribute<int>(nameof(PortTextWidth));

            UserControl1_SizeChanged(this, null);
        }
        [DefaultValue("Host")]
        public string HostNameLabelText { get => lblHostname.Text; set => lblHostname.Text = value; }

        [DefaultValue("Port")]
        public string PortLabelText { get => lblPort.Text; set => lblPort.Text = value; }

        [DefaultValue(64)]
        public int PortTextWidth { get => txtPort.Width; set => txtPort.Width = value; }

        private void UserControl1_SizeChanged(object sender, EventArgs e)
        {
            txtHost.Width = Width - txtPort.Width;
            lblPort.Left = lblHostname.Left + txtHost.Width;
            txtPort.Left = txtHost.Left + txtHost.Width;
            lblHostname.Width = Width - lblPort.Width;
        }
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
        private void AllowBackspace_KeyDown(object sender, KeyEventArgs e)
        {
            _allowedKeyPress = e.KeyCode == Keys.Back;
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
            OnHostNameChange(this, new EventArgs());
        }
    }

}
