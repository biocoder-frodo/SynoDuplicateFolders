namespace SynoDuplicateFolders
{
    partial class Preferences
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.tabpageServers = new System.Windows.Forms.TabPage();
            this.txtKeep = new System.Windows.Forms.TextBox();
            this.optAnalyzerDbRemove = new System.Windows.Forms.RadioButton();
            this.optAnalyzerDbKeep = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.chkCacheFolder = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabCharting = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taggedColorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabpageSecurity = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDPAPI = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.tabpageServers.SuspendLayout();
            this.tabCharting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taggedColorBindingSource)).BeginInit();
            this.tabpageSecurity.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tabConfig, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnApply, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(618, 411);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.tabpageServers);
            this.tabConfig.Controls.Add(this.tabCharting);
            this.tabConfig.Controls.Add(this.tabpageSecurity);
            this.tabConfig.Location = new System.Drawing.Point(3, 3);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(607, 356);
            this.tabConfig.TabIndex = 1;
            // 
            // tabpageServers
            // 
            this.tabpageServers.Controls.Add(this.txtKeep);
            this.tabpageServers.Controls.Add(this.optAnalyzerDbRemove);
            this.tabpageServers.Controls.Add(this.optAnalyzerDbKeep);
            this.tabpageServers.Controls.Add(this.button1);
            this.tabpageServers.Controls.Add(this.txtFolder);
            this.tabpageServers.Controls.Add(this.chkCacheFolder);
            this.tabpageServers.Controls.Add(this.checkBox1);
            this.tabpageServers.Controls.Add(this.comboBox1);
            this.tabpageServers.Location = new System.Drawing.Point(4, 22);
            this.tabpageServers.Name = "tabpageServers";
            this.tabpageServers.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageServers.Size = new System.Drawing.Size(599, 330);
            this.tabpageServers.TabIndex = 0;
            this.tabpageServers.Text = "General";
            this.tabpageServers.UseVisualStyleBackColor = true;
            // 
            // txtKeep
            // 
            this.txtKeep.Enabled = false;
            this.txtKeep.Location = new System.Drawing.Point(419, 175);
            this.txtKeep.Name = "txtKeep";
            this.txtKeep.Size = new System.Drawing.Size(41, 20);
            this.txtKeep.TabIndex = 11;
            // 
            // optAnalyzerDbRemove
            // 
            this.optAnalyzerDbRemove.AutoSize = true;
            this.optAnalyzerDbRemove.Location = new System.Drawing.Point(8, 178);
            this.optAnalyzerDbRemove.Name = "optAnalyzerDbRemove";
            this.optAnalyzerDbRemove.Size = new System.Drawing.Size(405, 17);
            this.optAnalyzerDbRemove.TabIndex = 10;
            this.optAnalyzerDbRemove.Text = "Remove all but the latest analyzer.db files in your DSM installation, namely keep" +
    ": ";
            this.optAnalyzerDbRemove.UseVisualStyleBackColor = true;
            this.optAnalyzerDbRemove.CheckedChanged += new System.EventHandler(this.optAnalyzerDbRemove_CheckedChanged);
            // 
            // optAnalyzerDbKeep
            // 
            this.optAnalyzerDbKeep.AutoSize = true;
            this.optAnalyzerDbKeep.Checked = true;
            this.optAnalyzerDbKeep.Location = new System.Drawing.Point(8, 155);
            this.optAnalyzerDbKeep.Name = "optAnalyzerDbKeep";
            this.optAnalyzerDbKeep.Size = new System.Drawing.Size(257, 17);
            this.optAnalyzerDbKeep.TabIndex = 9;
            this.optAnalyzerDbKeep.TabStop = true;
            this.optAnalyzerDbKeep.Text = "Keep all analyzer.db files in your DSM installation.";
            this.optAnalyzerDbKeep.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(375, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = "Browse ....";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Enabled = false;
            this.txtFolder.Location = new System.Drawing.Point(8, 100);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(349, 20);
            this.txtFolder.TabIndex = 6;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // chkCacheFolder
            // 
            this.chkCacheFolder.AutoSize = true;
            this.chkCacheFolder.Location = new System.Drawing.Point(8, 77);
            this.chkCacheFolder.Name = "chkCacheFolder";
            this.chkCacheFolder.Size = new System.Drawing.Size(207, 17);
            this.chkCacheFolder.TabIndex = 5;
            this.chkCacheFolder.Text = "Alternate location for the report cache:";
            this.chkCacheFolder.UseVisualStyleBackColor = true;
            this.chkCacheFolder.CheckedChanged += new System.EventHandler(this.chkCacheFolder_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(250, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Refresh data for the specified server on startup:";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(8, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(214, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // tabCharting
            // 
            this.tabCharting.Controls.Add(this.dataGridView1);
            this.tabCharting.Location = new System.Drawing.Point(4, 22);
            this.tabCharting.Name = "tabCharting";
            this.tabCharting.Padding = new System.Windows.Forms.Padding(3);
            this.tabCharting.Size = new System.Drawing.Size(599, 330);
            this.tabCharting.TabIndex = 2;
            this.tabCharting.Text = "Charting";
            this.tabCharting.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.colorDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.taggedColorBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(593, 327);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            this.keyDataGridViewTextBoxColumn.HeaderText = "Key";
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // colorDataGridViewTextBoxColumn
            // 
            this.colorDataGridViewTextBoxColumn.HeaderText = "Color";
            this.colorDataGridViewTextBoxColumn.Name = "colorDataGridViewTextBoxColumn";
            this.colorDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // taggedColorBindingSource
            // 
            this.taggedColorBindingSource.AllowNew = true;
            this.taggedColorBindingSource.DataSource = typeof(SynoDuplicateFolders.Controls.ITaggedColor);
            // 
            // tabpageSecurity
            // 
            this.tabpageSecurity.Controls.Add(this.label2);
            this.tabpageSecurity.Controls.Add(this.txtDPAPI);
            this.tabpageSecurity.Controls.Add(this.label1);
            this.tabpageSecurity.Location = new System.Drawing.Point(4, 22);
            this.tabpageSecurity.Name = "tabpageSecurity";
            this.tabpageSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageSecurity.Size = new System.Drawing.Size(599, 330);
            this.tabpageSecurity.TabIndex = 1;
            this.tabpageSecurity.Text = "Security";
            this.tabpageSecurity.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "DPAPI Entropy (file) for storing your passwords";
            // 
            // txtDPAPI
            // 
            this.txtDPAPI.Location = new System.Drawing.Point(22, 281);
            this.txtDPAPI.Name = "txtDPAPI";
            this.txtDPAPI.Size = new System.Drawing.Size(462, 20);
            this.txtDPAPI.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 94);
            this.label1.TabIndex = 0;
            this.label1.Text = "The SSH login by the tool currently only supports logging in using a password and" +
    " (on DSM6 and above) doing an interactive sudo to remove analyzer.db files.";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(532, 380);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(83, 28);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 411);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.tabpageServers.ResumeLayout(false);
            this.tabpageServers.PerformLayout();
            this.tabCharting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.taggedColorBindingSource)).EndInit();
            this.tabpageSecurity.ResumeLayout(false);
            this.tabpageSecurity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource taggedColorBindingSource;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabConfig;
        private System.Windows.Forms.TabPage tabpageServers;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabPage tabCharting;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabpageSecurity;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtKeep;
        private System.Windows.Forms.RadioButton optAnalyzerDbRemove;
        private System.Windows.Forms.RadioButton optAnalyzerDbKeep;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.CheckBox chkCacheFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDPAPI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}