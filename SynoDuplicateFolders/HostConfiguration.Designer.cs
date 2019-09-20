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
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.chkSynoReportHome = new System.Windows.Forms.CheckBox();
            this.lblReports = new System.Windows.Forms.Label();
            this.txtSynoReportHome = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.grpMethods = new System.Windows.Forms.GroupBox();
            this.btnKeyFileRemove = new System.Windows.Forms.Button();
            this.btnKeyFileAdd = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chkKeyFiles = new System.Windows.Forms.CheckBox();
            this.chkKeyBoardInteractive = new System.Windows.Forms.CheckBox();
            this.chkPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkAuthNone = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grpMethods.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(12, 26);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(263, 20);
            this.txtHost.TabIndex = 0;
            this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(80, 82);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(238, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.Leave += new System.EventHandler(this.txtUser_Leave);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(12, 9);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 13);
            this.lblHost.TabIndex = 2;
            this.lblHost.Text = "Host";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Enabled = false;
            this.lblUser.Location = new System.Drawing.Point(80, 66);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(29, 13);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "User";
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.Location = new System.Drawing.Point(13, 65);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(61, 17);
            this.chkUser.TabIndex = 2;
            this.chkUser.Text = "Custom";
            this.chkUser.UseVisualStyleBackColor = true;
            this.chkUser.CheckedChanged += new System.EventHandler(this.chkUser_CheckedChanged);
            // 
            // chkSynoReportHome
            // 
            this.chkSynoReportHome.AutoSize = true;
            this.chkSynoReportHome.Location = new System.Drawing.Point(13, 114);
            this.chkSynoReportHome.Name = "chkSynoReportHome";
            this.chkSynoReportHome.Size = new System.Drawing.Size(61, 17);
            this.chkSynoReportHome.TabIndex = 4;
            this.chkSynoReportHome.Text = "Custom";
            this.chkSynoReportHome.UseVisualStyleBackColor = true;
            this.chkSynoReportHome.CheckedChanged += new System.EventHandler(this.chkSynoReportHome_CheckedChanged);
            // 
            // lblReports
            // 
            this.lblReports.AutoSize = true;
            this.lblReports.Enabled = false;
            this.lblReports.Location = new System.Drawing.Point(80, 115);
            this.lblReports.Name = "lblReports";
            this.lblReports.Size = new System.Drawing.Size(73, 13);
            this.lblReports.TabIndex = 8;
            this.lblReports.Text = "Reports folder";
            // 
            // txtSynoReportHome
            // 
            this.txtSynoReportHome.Enabled = false;
            this.txtSynoReportHome.Location = new System.Drawing.Point(80, 131);
            this.txtSynoReportHome.Name = "txtSynoReportHome";
            this.txtSynoReportHome.Size = new System.Drawing.Size(238, 20);
            this.txtSynoReportHome.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(323, 452);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 36);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(403, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(279, 9);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 13;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(279, 26);
            this.txtPort.MaxLength = 12;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(72, 20);
            this.txtPort.TabIndex = 12;
            this.txtPort.TabStop = false;
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
            this.grpMethods.Location = new System.Drawing.Point(12, 157);
            this.grpMethods.Name = "grpMethods";
            this.grpMethods.Size = new System.Drawing.Size(462, 270);
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
            this.btnKeyFileRemove.TabIndex = 22;
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
            this.btnKeyFileAdd.TabIndex = 21;
            this.btnKeyFileAdd.Text = "Add";
            this.btnKeyFileAdd.UseVisualStyleBackColor = true;
            this.btnKeyFileAdd.Click += new System.EventHandler(this.btnKeyFileAdd_Click);
            // 
            // listView1
            // 
            this.listView1.Enabled = false;
            this.listView1.Location = new System.Drawing.Point(18, 149);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(300, 100);
            this.listView1.TabIndex = 20;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // chkKeyFiles
            // 
            this.chkKeyFiles.AutoSize = true;
            this.chkKeyFiles.Enabled = false;
            this.chkKeyFiles.Location = new System.Drawing.Point(18, 126);
            this.chkKeyFiles.Name = "chkKeyFiles";
            this.chkKeyFiles.Size = new System.Drawing.Size(104, 17);
            this.chkKeyFiles.TabIndex = 19;
            this.chkKeyFiles.Text = "Private Key Files";
            this.chkKeyFiles.UseVisualStyleBackColor = true;
            this.chkKeyFiles.CheckedChanged += new System.EventHandler(this.chkKeyFiles_CheckedChanged);
            // 
            // chkKeyBoardInteractive
            // 
            this.chkKeyBoardInteractive.AutoSize = true;
            this.chkKeyBoardInteractive.Enabled = false;
            this.chkKeyBoardInteractive.Location = new System.Drawing.Point(18, 54);
            this.chkKeyBoardInteractive.Name = "chkKeyBoardInteractive";
            this.chkKeyBoardInteractive.Size = new System.Drawing.Size(123, 17);
            this.chkKeyBoardInteractive.TabIndex = 18;
            this.chkKeyBoardInteractive.Text = "Keyboard interactive";
            this.chkKeyBoardInteractive.UseVisualStyleBackColor = true;
            // 
            // chkPassword
            // 
            this.chkPassword.AutoSize = true;
            this.chkPassword.Location = new System.Drawing.Point(18, 77);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(72, 17);
            this.chkPassword.TabIndex = 17;
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
            this.txtPassword.TabIndex = 16;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // chkAuthNone
            // 
            this.chkAuthNone.AutoSize = true;
            this.chkAuthNone.Enabled = false;
            this.chkAuthNone.Location = new System.Drawing.Point(18, 31);
            this.chkAuthNone.Name = "chkAuthNone";
            this.chkAuthNone.Size = new System.Drawing.Size(52, 17);
            this.chkAuthNone.TabIndex = 15;
            this.chkAuthNone.Text = "None";
            this.chkAuthNone.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // HostConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 500);
            this.Controls.Add(this.grpMethods);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkSynoReportHome);
            this.Controls.Add(this.lblReports);
            this.Controls.Add(this.txtSynoReportHome);
            this.Controls.Add(this.chkUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HostConfiguration";
            this.ShowInTaskbar = false;
            this.Text = "SSH Configuration for Synology DSM4 and above";
            this.Load += new System.EventHandler(this.HostConfiguration_Load);
            this.grpMethods.ResumeLayout(false);
            this.grpMethods.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.CheckBox chkSynoReportHome;
        private System.Windows.Forms.Label lblReports;
        private System.Windows.Forms.TextBox txtSynoReportHome;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.GroupBox grpMethods;
        private System.Windows.Forms.CheckBox chkAuthNone;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.CheckBox chkKeyFiles;
        private System.Windows.Forms.CheckBox chkKeyBoardInteractive;
        private System.Windows.Forms.CheckBox chkPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnKeyFileRemove;
        private System.Windows.Forms.Button btnKeyFileAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}