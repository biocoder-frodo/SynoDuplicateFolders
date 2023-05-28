namespace SynoDuplicateFolders
{
    partial class HostConfiguration
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grpReportsFolder = new System.Windows.Forms.GroupBox();
            this.lblReportFolderHint = new System.Windows.Forms.Label();
            this.radFolderDefault = new System.Windows.Forms.RadioButton();
            this.radFolderCustom = new System.Windows.Forms.RadioButton();
            this.txtSynoReportHome = new System.Windows.Forms.TextBox();
            this.grpUser = new System.Windows.Forms.GroupBox();
            this.lblUserHint = new System.Windows.Forms.Label();
            this.radUserCustom = new System.Windows.Forms.RadioButton();
            this.radUserDefault = new System.Windows.Forms.RadioButton();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.grpMethods = new System.Windows.Forms.GroupBox();
            this.btnKeyFileRemove = new System.Windows.Forms.Button();
            this.btnKeyFileAdd = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chkKeyFiles = new System.Windows.Forms.CheckBox();
            this.chkKeyBoardInteractive = new System.Windows.Forms.CheckBox();
            this.chkPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkAuthNone = new System.Windows.Forms.CheckBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDupeRemoveAll = new System.Windows.Forms.Button();
            this.lblIgnoreDupesFromPaths = new System.Windows.Forms.Label();
            this.btnDupeRemove = new System.Windows.Forms.Button();
            this.lstIgnoreDupes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optAnalyzerDbKeep = new System.Windows.Forms.RadioButton();
            this.chkKeep = new System.Windows.Forms.CheckBox();
            this.txtKeep = new System.Windows.Forms.TextBox();
            this.optAnalyzerDbRemove = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grpReportsFolder.SuspendLayout();
            this.grpUser.SuspendLayout();
            this.grpMethods.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(513, 538);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(507, 490);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grpReportsFolder);
            this.tabPage1.Controls.Add(this.grpUser);
            this.tabPage1.Controls.Add(this.lblHost);
            this.tabPage1.Controls.Add(this.txtHost);
            this.tabPage1.Controls.Add(this.grpMethods);
            this.tabPage1.Controls.Add(this.lblPort);
            this.tabPage1.Controls.Add(this.txtPort);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(499, 464);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SSH Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grpReportsFolder
            // 
            this.grpReportsFolder.Controls.Add(this.lblReportFolderHint);
            this.grpReportsFolder.Controls.Add(this.radFolderDefault);
            this.grpReportsFolder.Controls.Add(this.radFolderCustom);
            this.grpReportsFolder.Controls.Add(this.txtSynoReportHome);
            this.grpReportsFolder.Location = new System.Drawing.Point(9, 120);
            this.grpReportsFolder.Name = "grpReportsFolder";
            this.grpReportsFolder.Size = new System.Drawing.Size(462, 62);
            this.grpReportsFolder.TabIndex = 18;
            this.grpReportsFolder.TabStop = false;
            this.grpReportsFolder.Text = "Reports folder";
            // 
            // lblReportFolderHint
            // 
            this.lblReportFolderHint.AutoSize = true;
            this.lblReportFolderHint.Location = new System.Drawing.Point(155, 42);
            this.lblReportFolderHint.Name = "lblReportFolderHint";
            this.lblReportFolderHint.Size = new System.Drawing.Size(0, 13);
            this.lblReportFolderHint.TabIndex = 20;
            // 
            // radFolderDefault
            // 
            this.radFolderDefault.AutoSize = true;
            this.radFolderDefault.Location = new System.Drawing.Point(7, 22);
            this.radFolderDefault.Name = "radFolderDefault";
            this.radFolderDefault.Size = new System.Drawing.Size(59, 17);
            this.radFolderDefault.TabIndex = 19;
            this.radFolderDefault.TabStop = true;
            this.radFolderDefault.Text = "Default";
            this.radFolderDefault.UseVisualStyleBackColor = true;
            this.radFolderDefault.CheckedChanged += new System.EventHandler(this.radFolderDefault_CheckedChanged);
            this.radFolderDefault.MouseLeave += new System.EventHandler(this.radioButtonDefault_MouseLeave);
            this.radFolderDefault.MouseHover += new System.EventHandler(this.radioButtonDefault_MouseHover);
            // 
            // radFolderCustom
            // 
            this.radFolderCustom.AutoSize = true;
            this.radFolderCustom.Location = new System.Drawing.Point(72, 22);
            this.radFolderCustom.Name = "radFolderCustom";
            this.radFolderCustom.Size = new System.Drawing.Size(77, 17);
            this.radFolderCustom.TabIndex = 18;
            this.radFolderCustom.TabStop = true;
            this.radFolderCustom.Text = "This folder:";
            this.radFolderCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radFolderCustom.UseVisualStyleBackColor = true;
            // 
            // txtSynoReportHome
            // 
            this.txtSynoReportHome.Enabled = false;
            this.txtSynoReportHome.Location = new System.Drawing.Point(155, 19);
            this.txtSynoReportHome.Name = "txtSynoReportHome";
            this.txtSynoReportHome.Size = new System.Drawing.Size(246, 20);
            this.txtSynoReportHome.TabIndex = 6;
            this.txtSynoReportHome.Validating += new System.ComponentModel.CancelEventHandler(this.txtSynoReportHome_Validating);
            // 
            // grpUser
            // 
            this.grpUser.Controls.Add(this.lblUserHint);
            this.grpUser.Controls.Add(this.radUserCustom);
            this.grpUser.Controls.Add(this.radUserDefault);
            this.grpUser.Controls.Add(this.txtUser);
            this.grpUser.Location = new System.Drawing.Point(9, 55);
            this.grpUser.Name = "grpUser";
            this.grpUser.Size = new System.Drawing.Size(462, 59);
            this.grpUser.TabIndex = 17;
            this.grpUser.TabStop = false;
            this.grpUser.Text = "User";
            // 
            // lblUserHint
            // 
            this.lblUserHint.AutoSize = true;
            this.lblUserHint.Location = new System.Drawing.Point(155, 42);
            this.lblUserHint.Name = "lblUserHint";
            this.lblUserHint.Size = new System.Drawing.Size(0, 13);
            this.lblUserHint.TabIndex = 21;
            // 
            // radUserCustom
            // 
            this.radUserCustom.AutoSize = true;
            this.radUserCustom.Location = new System.Drawing.Point(78, 19);
            this.radUserCustom.Name = "radUserCustom";
            this.radUserCustom.Size = new System.Drawing.Size(71, 17);
            this.radUserCustom.TabIndex = 17;
            this.radUserCustom.TabStop = true;
            this.radUserCustom.Text = "This user:";
            this.radUserCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radUserCustom.UseVisualStyleBackColor = true;
            // 
            // radUserDefault
            // 
            this.radUserDefault.AutoSize = true;
            this.radUserDefault.Location = new System.Drawing.Point(7, 19);
            this.radUserDefault.Name = "radUserDefault";
            this.radUserDefault.Size = new System.Drawing.Size(59, 17);
            this.radUserDefault.TabIndex = 16;
            this.radUserDefault.TabStop = true;
            this.radUserDefault.Text = "Default";
            this.radUserDefault.UseVisualStyleBackColor = true;
            this.radUserDefault.CheckedChanged += new System.EventHandler(this.radUserDefault_CheckedChanged);
            this.radUserDefault.MouseLeave += new System.EventHandler(this.radioButtonDefault_MouseLeave);
            this.radUserDefault.MouseHover += new System.EventHandler(this.radioButtonDefault_MouseHover);
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(155, 19);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(164, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.txtUser_Validating);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(6, 12);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 13);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(9, 28);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(263, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            this.txtHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHost_KeyDown);
            this.txtHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHost_KeyPress);
            // 
            // grpMethods
            // 
            this.grpMethods.Controls.Add(this.btnKeyFileRemove);
            this.grpMethods.Controls.Add(this.btnKeyFileAdd);
            this.grpMethods.Controls.Add(this.listView1);
            this.grpMethods.Controls.Add(this.chkKeyFiles);
            this.grpMethods.Controls.Add(this.chkKeyBoardInteractive);
            this.grpMethods.Controls.Add(this.chkPassword);
            this.grpMethods.Controls.Add(this.txtPassword);
            this.grpMethods.Controls.Add(this.chkAuthNone);
            this.grpMethods.Location = new System.Drawing.Point(9, 188);
            this.grpMethods.Name = "grpMethods";
            this.grpMethods.Size = new System.Drawing.Size(462, 261);
            this.grpMethods.TabIndex = 15;
            this.grpMethods.TabStop = false;
            this.grpMethods.Text = "Authentication Methods";
            // 
            // btnKeyFileRemove
            // 
            this.btnKeyFileRemove.Enabled = false;
            this.btnKeyFileRemove.Location = new System.Drawing.Point(324, 178);
            this.btnKeyFileRemove.Name = "btnKeyFileRemove";
            this.btnKeyFileRemove.Size = new System.Drawing.Size(75, 23);
            this.btnKeyFileRemove.TabIndex = 12;
            this.btnKeyFileRemove.Text = "Remove";
            this.btnKeyFileRemove.UseVisualStyleBackColor = true;
            this.btnKeyFileRemove.Click += new System.EventHandler(this.btnKeyFileRemove_Click);
            // 
            // btnKeyFileAdd
            // 
            this.btnKeyFileAdd.Enabled = false;
            this.btnKeyFileAdd.Location = new System.Drawing.Point(324, 149);
            this.btnKeyFileAdd.Name = "btnKeyFileAdd";
            this.btnKeyFileAdd.Size = new System.Drawing.Size(75, 23);
            this.btnKeyFileAdd.TabIndex = 11;
            this.btnKeyFileAdd.Text = "Add";
            this.btnKeyFileAdd.UseVisualStyleBackColor = true;
            this.btnKeyFileAdd.Click += new System.EventHandler(this.btnKeyFileAdd_Click);
            // 
            // listView1
            // 
            this.listView1.Enabled = false;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(18, 149);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(300, 100);
            this.listView1.TabIndex = 20;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // chkKeyFiles
            // 
            this.chkKeyFiles.AutoSize = true;
            this.chkKeyFiles.Location = new System.Drawing.Point(18, 126);
            this.chkKeyFiles.Name = "chkKeyFiles";
            this.chkKeyFiles.Size = new System.Drawing.Size(104, 17);
            this.chkKeyFiles.TabIndex = 10;
            this.chkKeyFiles.Text = "Private Key Files";
            this.chkKeyFiles.UseVisualStyleBackColor = true;
            this.chkKeyFiles.CheckedChanged += new System.EventHandler(this.chkKeyFiles_CheckedChanged);
            // 
            // chkKeyBoardInteractive
            // 
            this.chkKeyBoardInteractive.AutoSize = true;
            this.chkKeyBoardInteractive.Location = new System.Drawing.Point(18, 54);
            this.chkKeyBoardInteractive.Name = "chkKeyBoardInteractive";
            this.chkKeyBoardInteractive.Size = new System.Drawing.Size(123, 17);
            this.chkKeyBoardInteractive.TabIndex = 7;
            this.chkKeyBoardInteractive.Text = "Keyboard interactive";
            this.chkKeyBoardInteractive.UseVisualStyleBackColor = true;
            this.chkKeyBoardInteractive.CheckedChanged += new System.EventHandler(this.chkKeyBoardInteractive_CheckedChanged);
            // 
            // chkPassword
            // 
            this.chkPassword.AutoSize = true;
            this.chkPassword.Location = new System.Drawing.Point(18, 77);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(72, 17);
            this.chkPassword.TabIndex = 8;
            this.chkPassword.Text = "Password";
            this.chkPassword.UseVisualStyleBackColor = true;
            this.chkPassword.CheckedChanged += new System.EventHandler(this.chkPassword_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(43, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(263, 20);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // chkAuthNone
            // 
            this.chkAuthNone.AutoSize = true;
            this.chkAuthNone.Enabled = false;
            this.chkAuthNone.Location = new System.Drawing.Point(18, 31);
            this.chkAuthNone.Name = "chkAuthNone";
            this.chkAuthNone.Size = new System.Drawing.Size(52, 17);
            this.chkAuthNone.TabIndex = 6;
            this.chkAuthNone.Text = "None";
            this.chkAuthNone.UseVisualStyleBackColor = true;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(278, 12);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 13;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(278, 28);
            this.txtPort.MaxLength = 12;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(72, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.TabStop = false;
            this.txtPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPort_KeyDown);
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(499, 464);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Storage Analyzer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDupeRemoveAll);
            this.groupBox2.Controls.Add(this.lblIgnoreDupesFromPaths);
            this.groupBox2.Controls.Add(this.btnDupeRemove);
            this.groupBox2.Controls.Add(this.lstIgnoreDupes);
            this.groupBox2.Location = new System.Drawing.Point(6, 126);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(484, 319);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Duplicate candidates";
            // 
            // btnDupeRemoveAll
            // 
            this.btnDupeRemoveAll.Location = new System.Drawing.Point(395, 73);
            this.btnDupeRemoveAll.Name = "btnDupeRemoveAll";
            this.btnDupeRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.btnDupeRemoveAll.TabIndex = 3;
            this.btnDupeRemoveAll.Text = "Remove all";
            this.btnDupeRemoveAll.UseVisualStyleBackColor = true;
            this.btnDupeRemoveAll.Click += new System.EventHandler(this.btnDupeRemoveAll_Click);
            // 
            // lblIgnoreDupesFromPaths
            // 
            this.lblIgnoreDupesFromPaths.AutoSize = true;
            this.lblIgnoreDupesFromPaths.Location = new System.Drawing.Point(6, 28);
            this.lblIgnoreDupesFromPaths.Name = "lblIgnoreDupesFromPaths";
            this.lblIgnoreDupesFromPaths.Size = new System.Drawing.Size(255, 13);
            this.lblIgnoreDupesFromPaths.TabIndex = 2;
            this.lblIgnoreDupesFromPaths.Text = "Ignore duplicate candidates from the following paths:";
            // 
            // btnDupeRemove
            // 
            this.btnDupeRemove.Enabled = false;
            this.btnDupeRemove.Location = new System.Drawing.Point(395, 44);
            this.btnDupeRemove.Name = "btnDupeRemove";
            this.btnDupeRemove.Size = new System.Drawing.Size(75, 23);
            this.btnDupeRemove.TabIndex = 1;
            this.btnDupeRemove.Text = "Remove";
            this.btnDupeRemove.UseVisualStyleBackColor = true;
            this.btnDupeRemove.Click += new System.EventHandler(this.btnDupeRemove_Click);
            // 
            // lstIgnoreDupes
            // 
            this.lstIgnoreDupes.FormattingEnabled = true;
            this.lstIgnoreDupes.Location = new System.Drawing.Point(18, 44);
            this.lstIgnoreDupes.Name = "lstIgnoreDupes";
            this.lstIgnoreDupes.Size = new System.Drawing.Size(366, 251);
            this.lstIgnoreDupes.TabIndex = 0;
            this.lstIgnoreDupes.SelectedIndexChanged += new System.EventHandler(this.lstIgnoreDupes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optAnalyzerDbKeep);
            this.groupBox1.Controls.Add(this.chkKeep);
            this.groupBox1.Controls.Add(this.txtKeep);
            this.groupBox1.Controls.Add(this.optAnalyzerDbRemove);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 114);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disk space";
            // 
            // optAnalyzerDbKeep
            // 
            this.optAnalyzerDbKeep.AutoSize = true;
            this.optAnalyzerDbKeep.Checked = true;
            this.optAnalyzerDbKeep.Enabled = false;
            this.optAnalyzerDbKeep.Location = new System.Drawing.Point(18, 53);
            this.optAnalyzerDbKeep.Name = "optAnalyzerDbKeep";
            this.optAnalyzerDbKeep.Size = new System.Drawing.Size(257, 17);
            this.optAnalyzerDbKeep.TabIndex = 18;
            this.optAnalyzerDbKeep.TabStop = true;
            this.optAnalyzerDbKeep.Text = "Keep all analyzer.db files in your DSM installation.";
            this.optAnalyzerDbKeep.UseVisualStyleBackColor = true;
            this.optAnalyzerDbKeep.CheckedChanged += new System.EventHandler(this.optAnalyzerDbRemove_CheckedChanged);
            // 
            // chkKeep
            // 
            this.chkKeep.AutoSize = true;
            this.chkKeep.Location = new System.Drawing.Point(18, 30);
            this.chkKeep.Name = "chkKeep";
            this.chkKeep.Size = new System.Drawing.Size(423, 17);
            this.chkKeep.TabIndex = 17;
            this.chkKeep.Text = "Use the setting below instead of the one the applications General Preferences pag" +
    "e.";
            this.chkKeep.UseVisualStyleBackColor = true;
            this.chkKeep.CheckedChanged += new System.EventHandler(this.chkKeep_CheckedChanged);
            // 
            // txtKeep
            // 
            this.txtKeep.Enabled = false;
            this.txtKeep.Location = new System.Drawing.Point(429, 73);
            this.txtKeep.Name = "txtKeep";
            this.txtKeep.Size = new System.Drawing.Size(41, 20);
            this.txtKeep.TabIndex = 20;
            this.txtKeep.Validating += new System.ComponentModel.CancelEventHandler(this.txtKeep_Validating);
            // 
            // optAnalyzerDbRemove
            // 
            this.optAnalyzerDbRemove.AutoSize = true;
            this.optAnalyzerDbRemove.Enabled = false;
            this.optAnalyzerDbRemove.Location = new System.Drawing.Point(18, 76);
            this.optAnalyzerDbRemove.Name = "optAnalyzerDbRemove";
            this.optAnalyzerDbRemove.Size = new System.Drawing.Size(405, 17);
            this.optAnalyzerDbRemove.TabIndex = 19;
            this.optAnalyzerDbRemove.Text = "Remove all but the latest analyzer.db files in your DSM installation, namely keep" +
    ": ";
            this.optAnalyzerDbRemove.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(356, 499);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 36);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(436, 499);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 36);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // HostConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 538);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HostConfiguration";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.HostConfiguration_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.grpReportsFolder.ResumeLayout(false);
            this.grpReportsFolder.PerformLayout();
            this.grpUser.ResumeLayout(false);
            this.grpUser.PerformLayout();
            this.grpMethods.ResumeLayout(false);
            this.grpMethods.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.GroupBox grpMethods;
        private System.Windows.Forms.Button btnKeyFileRemove;
        private System.Windows.Forms.Button btnKeyFileAdd;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox chkKeyFiles;
        private System.Windows.Forms.CheckBox chkKeyBoardInteractive;
        private System.Windows.Forms.CheckBox chkPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkAuthNone;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optAnalyzerDbKeep;
        private System.Windows.Forms.CheckBox chkKeep;
        private System.Windows.Forms.TextBox txtKeep;
        private System.Windows.Forms.RadioButton optAnalyzerDbRemove;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDupeRemoveAll;
        private System.Windows.Forms.Label lblIgnoreDupesFromPaths;
        private System.Windows.Forms.Button btnDupeRemove;
        private System.Windows.Forms.ListBox lstIgnoreDupes;
        private System.Windows.Forms.GroupBox grpReportsFolder;
        private System.Windows.Forms.RadioButton radFolderDefault;
        private System.Windows.Forms.RadioButton radFolderCustom;
        private System.Windows.Forms.TextBox txtSynoReportHome;
        private System.Windows.Forms.GroupBox grpUser;
        private System.Windows.Forms.RadioButton radUserCustom;
        private System.Windows.Forms.RadioButton radUserDefault;
        private System.Windows.Forms.Label lblReportFolderHint;
        private System.Windows.Forms.Label lblUserHint;
    }
}