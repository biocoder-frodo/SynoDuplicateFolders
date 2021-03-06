﻿namespace SynoDuplicateFolders
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
            this.txtKeep = new System.Windows.Forms.TextBox();
            this.optAnalyzerDbRemove = new System.Windows.Forms.RadioButton();
            this.optAnalyzerDbKeep = new System.Windows.Forms.RadioButton();
            this.chkKeep = new System.Windows.Forms.CheckBox();
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
            this.txtHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHost_KeyDown);
            this.txtHost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHost_KeyPress);
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(80, 82);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(89, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.Leave += new System.EventHandler(this.txtUser_Leave);
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.txtUser_Validating);
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(12, 9);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 13);
            this.lblHost.TabIndex = 0;
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
            this.chkSynoReportHome.Location = new System.Drawing.Point(12, 113);
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
            this.lblReports.Location = new System.Drawing.Point(80, 114);
            this.lblReports.Name = "lblReports";
            this.lblReports.Size = new System.Drawing.Size(73, 13);
            this.lblReports.TabIndex = 8;
            this.lblReports.Text = "Reports folder";
            // 
            // txtSynoReportHome
            // 
            this.txtSynoReportHome.Enabled = false;
            this.txtSynoReportHome.Location = new System.Drawing.Point(80, 130);
            this.txtSynoReportHome.Name = "txtSynoReportHome";
            this.txtSynoReportHome.Size = new System.Drawing.Size(238, 20);
            this.txtSynoReportHome.TabIndex = 5;
            this.txtSynoReportHome.Validating += new System.ComponentModel.CancelEventHandler(this.txtSynoReportHome_Validating);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(326, 542);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(74, 36);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(406, 542);
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
            this.txtPort.TabIndex = 1;
            this.txtPort.TabStop = false;
            this.txtPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPort_KeyDown);
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
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
            this.grpMethods.Location = new System.Drawing.Point(12, 172);
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtKeep
            // 
            this.txtKeep.Enabled = false;
            this.txtKeep.Location = new System.Drawing.Point(426, 501);
            this.txtKeep.Name = "txtKeep";
            this.txtKeep.Size = new System.Drawing.Size(41, 20);
            this.txtKeep.TabIndex = 16;
            this.txtKeep.Validating += new System.ComponentModel.CancelEventHandler(this.txtKeep_Validating);
            // 
            // optAnalyzerDbRemove
            // 
            this.optAnalyzerDbRemove.AutoSize = true;
            this.optAnalyzerDbRemove.Enabled = false;
            this.optAnalyzerDbRemove.Location = new System.Drawing.Point(15, 504);
            this.optAnalyzerDbRemove.Name = "optAnalyzerDbRemove";
            this.optAnalyzerDbRemove.Size = new System.Drawing.Size(405, 17);
            this.optAnalyzerDbRemove.TabIndex = 15;
            this.optAnalyzerDbRemove.Text = "Remove all but the latest analyzer.db files in your DSM installation, namely keep" +
    ": ";
            this.optAnalyzerDbRemove.UseVisualStyleBackColor = true;
            this.optAnalyzerDbRemove.CheckedChanged += new System.EventHandler(this.optAnalyzerDbRemove_CheckedChanged);
            // 
            // optAnalyzerDbKeep
            // 
            this.optAnalyzerDbKeep.AutoSize = true;
            this.optAnalyzerDbKeep.Checked = true;
            this.optAnalyzerDbKeep.Enabled = false;
            this.optAnalyzerDbKeep.Location = new System.Drawing.Point(15, 481);
            this.optAnalyzerDbKeep.Name = "optAnalyzerDbKeep";
            this.optAnalyzerDbKeep.Size = new System.Drawing.Size(257, 17);
            this.optAnalyzerDbKeep.TabIndex = 14;
            this.optAnalyzerDbKeep.TabStop = true;
            this.optAnalyzerDbKeep.Text = "Keep all analyzer.db files in your DSM installation.";
            this.optAnalyzerDbKeep.UseVisualStyleBackColor = true;
            // 
            // chkKeep
            // 
            this.chkKeep.AutoSize = true;
            this.chkKeep.Location = new System.Drawing.Point(15, 458);
            this.chkKeep.Name = "chkKeep";
            this.chkKeep.Size = new System.Drawing.Size(61, 17);
            this.chkKeep.TabIndex = 13;
            this.chkKeep.Text = "Custom";
            this.chkKeep.UseVisualStyleBackColor = true;
            this.chkKeep.CheckedChanged += new System.EventHandler(this.chkKeep_CheckedChanged);
            // 
            // HostConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 589);
            this.Controls.Add(this.chkKeep);
            this.Controls.Add(this.txtKeep);
            this.Controls.Add(this.optAnalyzerDbRemove);
            this.Controls.Add(this.optAnalyzerDbKeep);
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
        private System.Windows.Forms.TextBox txtKeep;
        private System.Windows.Forms.RadioButton optAnalyzerDbRemove;
        private System.Windows.Forms.RadioButton optAnalyzerDbKeep;
        private System.Windows.Forms.CheckBox chkKeep;
    }
}