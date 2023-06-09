using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskStationManager.SecureShell
{
    public partial class PassPhrase : Form
    {
        internal PassPhrase(string user, string[] banner, string request)
        {
            int count = 0;
            InitializeComponent();
            this.Text = "Keyboard-Interactive Login for user "+user;
            label1.Text = request;
            foreach (string line in banner)
            {
                count += line.Trim().Length;
                listBox1.Items.Add(line);
            }
            if(count>0)listBox1.Visible = true;
            textBox1.Focus();
        }
        internal PassPhrase(string fileName)
        {
            InitializeComponent();
            label1.Text = string.Format("Please enter the pass-phrase for keyfile '{0}'", fileName);
            textBox1.Focus();
        }
        internal string Password { get { return textBox1.Text; } }

        private void Button1_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Button1_Click(null, null);
        }
    }
}
