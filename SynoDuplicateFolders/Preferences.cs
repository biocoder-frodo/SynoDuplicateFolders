using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using SynoDuplicateFolders.Properties;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Extensions;
using static System.Environment;
using static SynoDuplicateFolders.Properties.Settings;
namespace SynoDuplicateFolders
{
    public partial class Preferences : Form
    {
        private CustomSettings _config;

        private bool dirty_custom = false;
        string server = string.Empty;
        string folder = string.Empty;
        bool keep = true;
        int keepCount = 1;

        internal Preferences(CustomSettings config)
        {
            InitializeComponent();

            _config = config;

            DataToControls(_config);
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            ControlsToData();

            if (!Default.AutoRefreshServer.Equals(server)
                || !Default.CacheFolder.Equals(folder)
                || !Default.KeepAnalyzerDb.Equals(keep)
                || !Default.KeepAnalyzerDbCount.Equals(keepCount))

            {
                Default.CacheFolder = folder;
                Default.AutoRefreshServer = server;
                Default.KeepAnalyzerDb = keep;
                Default.KeepAnalyzerDbCount = keepCount;
                Default.Save();
            }
            if (dirty_custom)
            {
                _config.CurrentConfiguration.Save();
                dirty_custom = false;
            }
        }
        #region Data exchange on controls

        private void DataToControls(CustomSettings config = null)
        {
            if (config != null) //only done once
            {
                foreach (DSMHost h in config.DSMHosts.Items)
                    comboBox1.Items.Add(h.Host);
                dataGridView1.DataSource = config.List;
            }

            if (!string.IsNullOrEmpty(Default.AutoRefreshServer))
            {
                if (_config.DSMHosts.Items.ContainsKey(Default.AutoRefreshServer))
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
            optAnalyzerDbKeep.Checked = Default.KeepAnalyzerDb;
            optAnalyzerDbRemove.Checked = !Default.KeepAnalyzerDb;
            txtKeep.Text = Default.KeepAnalyzerDbCount.ToString();

            txtDPAPI.Text = Default.DPAPIVector;

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

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                var r = ((DataGridView)sender).Rows[e.RowIndex];
                var d = r.DataBoundItem as ITaggedColor;
                colorDialog1.AnyColor = false;
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    d.ColorName = colorDialog1.Color.ToClosestKnownColor().ToString();
                    dirty_custom = true;
                }
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var r = ((DataGridView)sender).Rows[e.RowIndex];
            var d = r.DataBoundItem as ITaggedColor;
            var c = r.Cells["colorDataGridViewTextBoxColumn"];
            var k = Color.FromName(d.ColorName);

            c.Style.BackColor = k;
            c.Style.SelectionBackColor = k;
            c.Style.SelectionForeColor = k;
        }
        #endregion
    }
}
