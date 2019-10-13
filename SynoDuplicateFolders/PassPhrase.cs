using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynoDuplicateFolders
{
    internal partial class PassPhrase : Form
    {
        public PassPhrase(string fileName)
        {
            InitializeComponent();
            label1.Text = string.Format("Please enter the pass-phrase for keyfile '{0}'", fileName);
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

    }
}
