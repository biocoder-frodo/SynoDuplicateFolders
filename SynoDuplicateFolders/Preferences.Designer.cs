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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optInteractiveSudo = new System.Windows.Forms.RadioButton();
            this.optSudo = new System.Windows.Forms.RadioButton();
            this.optLoginAsRoot = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDPAPI = new System.Windows.Forms.TextBox();
            this.tabPageProxy = new System.Windows.Forms.TabPage();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblProxyUser = new System.Windows.Forms.Label();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblProxyHost = new System.Windows.Forms.Label();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.optProxy = new System.Windows.Forms.RadioButton();
            this.optNoProxy = new System.Windows.Forms.RadioButton();
            this.cmbProxy = new System.Windows.Forms.ComboBox();
            this.tabPageDiffTool = new System.Windows.Forms.TabPage();
            this.cmbMaxCompare = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDiffExeArgs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDiffTool = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.optPassPhraseInteractive = new System.Windows.Forms.RadioButton();
            this.optUseStoredPassPhrases = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.tabpageServers.SuspendLayout();
            this.tabCharting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taggedColorBindingSource)).BeginInit();
            this.tabpageSecurity.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageProxy.SuspendLayout();
            this.tabPageDiffTool.SuspendLayout();
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
            this.tabConfig.Controls.Add(this.tabPageProxy);
            this.tabConfig.Controls.Add(this.tabPageDiffTool);
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
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.tabpageSecurity.Controls.Add(this.groupBox2);
            this.tabpageSecurity.Controls.Add(this.groupBox1);
            this.tabpageSecurity.Controls.Add(this.label2);
            this.tabpageSecurity.Controls.Add(this.txtDPAPI);
            this.tabpageSecurity.Location = new System.Drawing.Point(4, 22);
            this.tabpageSecurity.Name = "tabpageSecurity";
            this.tabpageSecurity.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageSecurity.Size = new System.Drawing.Size(599, 330);
            this.tabpageSecurity.TabIndex = 1;
            this.tabpageSecurity.Text = "Security";
            this.tabpageSecurity.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optUseStoredPassPhrases);
            this.groupBox2.Controls.Add(this.optPassPhraseInteractive);
            this.groupBox2.Location = new System.Drawing.Point(6, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(578, 72);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keyfile pass phrases";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optInteractiveSudo);
            this.groupBox1.Controls.Add(this.optSudo);
            this.groupBox1.Controls.Add(this.optLoginAsRoot);
            this.groupBox1.Location = new System.Drawing.Point(6, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(578, 131);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analyzer.db remove method";
            // 
            // optInteractiveSudo
            // 
            this.optInteractiveSudo.AutoSize = true;
            this.optInteractiveSudo.Checked = true;
            this.optInteractiveSudo.Location = new System.Drawing.Point(19, 80);
            this.optInteractiveSudo.Name = "optInteractiveSudo";
            this.optInteractiveSudo.Size = new System.Drawing.Size(219, 17);
            this.optInteractiveSudo.TabIndex = 6;
            this.optInteractiveSudo.TabStop = true;
            this.optInteractiveSudo.Text = "Remove the file using interactive sudo rm";
            this.optInteractiveSudo.UseVisualStyleBackColor = true;
            // 
            // optSudo
            // 
            this.optSudo.AutoSize = true;
            this.optSudo.Location = new System.Drawing.Point(19, 57);
            this.optSudo.Name = "optSudo";
            this.optSudo.Size = new System.Drawing.Size(167, 17);
            this.optSudo.TabIndex = 5;
            this.optSudo.Text = "Remove the file using sudo rm";
            this.optSudo.UseVisualStyleBackColor = true;
            // 
            // optLoginAsRoot
            // 
            this.optLoginAsRoot.AutoSize = true;
            this.optLoginAsRoot.Enabled = false;
            this.optLoginAsRoot.Location = new System.Drawing.Point(19, 34);
            this.optLoginAsRoot.Name = "optLoginAsRoot";
            this.optLoginAsRoot.Size = new System.Drawing.Size(422, 17);
            this.optLoginAsRoot.TabIndex = 4;
            this.optLoginAsRoot.Text = "Login as root using the same password as the admin account (DSM4.x and DSM5.x)";
            this.optLoginAsRoot.UseVisualStyleBackColor = true;
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
            // tabPageProxy
            // 
            this.tabPageProxy.Controls.Add(this.lblProxyPassword);
            this.tabPageProxy.Controls.Add(this.txtProxyPassword);
            this.tabPageProxy.Controls.Add(this.lblProxyUser);
            this.tabPageProxy.Controls.Add(this.txtProxyUser);
            this.tabPageProxy.Controls.Add(this.lblProxyPort);
            this.tabPageProxy.Controls.Add(this.txtProxyPort);
            this.tabPageProxy.Controls.Add(this.label3);
            this.tabPageProxy.Controls.Add(this.lblProxyHost);
            this.tabPageProxy.Controls.Add(this.txtProxyHost);
            this.tabPageProxy.Controls.Add(this.optProxy);
            this.tabPageProxy.Controls.Add(this.optNoProxy);
            this.tabPageProxy.Controls.Add(this.cmbProxy);
            this.tabPageProxy.Location = new System.Drawing.Point(4, 22);
            this.tabPageProxy.Name = "tabPageProxy";
            this.tabPageProxy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProxy.Size = new System.Drawing.Size(599, 330);
            this.tabPageProxy.TabIndex = 3;
            this.tabPageProxy.Text = "Proxy";
            this.tabPageProxy.UseVisualStyleBackColor = true;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(41, 158);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(53, 13);
            this.lblProxyPassword.TabIndex = 11;
            this.lblProxyPassword.Text = "Password";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(100, 158);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '*';
            this.txtProxyPassword.Size = new System.Drawing.Size(200, 20);
            this.txtProxyPassword.TabIndex = 10;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // lblProxyUser
            // 
            this.lblProxyUser.AutoSize = true;
            this.lblProxyUser.Location = new System.Drawing.Point(41, 132);
            this.lblProxyUser.Name = "lblProxyUser";
            this.lblProxyUser.Size = new System.Drawing.Size(29, 13);
            this.lblProxyUser.TabIndex = 9;
            this.lblProxyUser.Text = "User";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Location = new System.Drawing.Point(100, 132);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(200, 20);
            this.txtProxyUser.TabIndex = 8;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(277, 97);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(26, 13);
            this.lblProxyPort.TabIndex = 7;
            this.lblProxyPort.Text = "Port";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(309, 94);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(63, 20);
            this.txtProxyPort.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "proxy";
            // 
            // lblProxyHost
            // 
            this.lblProxyHost.AutoSize = true;
            this.lblProxyHost.Location = new System.Drawing.Point(41, 94);
            this.lblProxyHost.Name = "lblProxyHost";
            this.lblProxyHost.Size = new System.Drawing.Size(29, 13);
            this.lblProxyHost.TabIndex = 4;
            this.lblProxyHost.Text = "Host";
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(71, 94);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(200, 20);
            this.txtProxyHost.TabIndex = 3;
            // 
            // optProxy
            // 
            this.optProxy.AutoSize = true;
            this.optProxy.Location = new System.Drawing.Point(20, 61);
            this.optProxy.Name = "optProxy";
            this.optProxy.Size = new System.Drawing.Size(106, 17);
            this.optProxy.TabIndex = 2;
            this.optProxy.Text = "Use the following";
            this.optProxy.UseVisualStyleBackColor = true;
            this.optProxy.CheckedChanged += new System.EventHandler(this.optProxy_CheckedChanged);
            // 
            // optNoProxy
            // 
            this.optNoProxy.AutoSize = true;
            this.optNoProxy.Checked = true;
            this.optNoProxy.Location = new System.Drawing.Point(20, 29);
            this.optNoProxy.Name = "optNoProxy";
            this.optNoProxy.Size = new System.Drawing.Size(114, 17);
            this.optNoProxy.TabIndex = 1;
            this.optNoProxy.TabStop = true;
            this.optNoProxy.Text = "Do not use a proxy";
            this.optNoProxy.UseVisualStyleBackColor = true;
            // 
            // cmbProxy
            // 
            this.cmbProxy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProxy.FormattingEnabled = true;
            this.cmbProxy.Items.AddRange(new object[] {
            "Http",
            "Socks4",
            "Socks5"});
            this.cmbProxy.Location = new System.Drawing.Point(132, 60);
            this.cmbProxy.Name = "cmbProxy";
            this.cmbProxy.Size = new System.Drawing.Size(90, 21);
            this.cmbProxy.TabIndex = 0;
            // 
            // tabPageDiffTool
            // 
            this.tabPageDiffTool.Controls.Add(this.cmbMaxCompare);
            this.tabPageDiffTool.Controls.Add(this.label5);
            this.tabPageDiffTool.Controls.Add(this.txtDiffExeArgs);
            this.tabPageDiffTool.Controls.Add(this.label4);
            this.tabPageDiffTool.Controls.Add(this.label1);
            this.tabPageDiffTool.Controls.Add(this.txtDiffTool);
            this.tabPageDiffTool.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiffTool.Name = "tabPageDiffTool";
            this.tabPageDiffTool.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiffTool.Size = new System.Drawing.Size(599, 330);
            this.tabPageDiffTool.TabIndex = 4;
            this.tabPageDiffTool.Text = "Differences";
            this.tabPageDiffTool.UseVisualStyleBackColor = true;
            // 
            // cmbMaxCompare
            // 
            this.cmbMaxCompare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaxCompare.FormattingEnabled = true;
            this.cmbMaxCompare.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cmbMaxCompare.Location = new System.Drawing.Point(21, 142);
            this.cmbMaxCompare.Name = "cmbMaxCompare";
            this.cmbMaxCompare.Size = new System.Drawing.Size(70, 21);
            this.cmbMaxCompare.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "External diff tool arguments";
            // 
            // txtDiffExeArgs
            // 
            this.txtDiffExeArgs.Location = new System.Drawing.Point(21, 85);
            this.txtDiffExeArgs.Name = "txtDiffExeArgs";
            this.txtDiffExeArgs.Size = new System.Drawing.Size(264, 20);
            this.txtDiffExeArgs.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Maximum files/folders to compare";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "External diff tool";
            // 
            // txtDiffTool
            // 
            this.txtDiffTool.Location = new System.Drawing.Point(21, 43);
            this.txtDiffTool.Name = "txtDiffTool";
            this.txtDiffTool.Size = new System.Drawing.Size(264, 20);
            this.txtDiffTool.TabIndex = 0;
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
            // optPassPhraseInteractive
            // 
            this.optPassPhraseInteractive.AutoSize = true;
            this.optPassPhraseInteractive.Checked = true;
            this.optPassPhraseInteractive.Enabled = false;
            this.optPassPhraseInteractive.Location = new System.Drawing.Point(16, 19);
            this.optPassPhraseInteractive.Name = "optPassPhraseInteractive";
            this.optPassPhraseInteractive.Size = new System.Drawing.Size(162, 17);
            this.optPassPhraseInteractive.TabIndex = 0;
            this.optPassPhraseInteractive.TabStop = true;
            this.optPassPhraseInteractive.Text = "Ask pass phrase interactively";
            this.optPassPhraseInteractive.UseVisualStyleBackColor = true;
            // 
            // optUseStoredPassPhrases
            // 
            this.optUseStoredPassPhrases.AutoSize = true;
            this.optUseStoredPassPhrases.Enabled = false;
            this.optUseStoredPassPhrases.Location = new System.Drawing.Point(16, 42);
            this.optUseStoredPassPhrases.Name = "optUseStoredPassPhrases";
            this.optUseStoredPassPhrases.Size = new System.Drawing.Size(136, 17);
            this.optUseStoredPassPhrases.TabIndex = 1;
            this.optUseStoredPassPhrases.Text = "Use stored pass phrase";
            this.optUseStoredPassPhrases.UseVisualStyleBackColor = true;
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageProxy.ResumeLayout(false);
            this.tabPageProxy.PerformLayout();
            this.tabPageDiffTool.ResumeLayout(false);
            this.tabPageDiffTool.PerformLayout();
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
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optInteractiveSudo;
        private System.Windows.Forms.RadioButton optSudo;
        private System.Windows.Forms.RadioButton optLoginAsRoot;
        private System.Windows.Forms.ComboBox cmbProxy;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label lblProxyUser;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblProxyHost;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.RadioButton optProxy;
        private System.Windows.Forms.RadioButton optNoProxy;
        private System.Windows.Forms.TabPage tabPageDiffTool;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDiffTool;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDiffExeArgs;
        private System.Windows.Forms.ComboBox cmbMaxCompare;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton optUseStoredPassPhrases;
        private System.Windows.Forms.RadioButton optPassPhraseInteractive;
    }
}