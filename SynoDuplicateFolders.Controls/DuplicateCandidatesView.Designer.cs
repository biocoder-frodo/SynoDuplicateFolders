namespace SynoDuplicateFolders.Controls
{
    partial class DuplicateCandidatesView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Candidates = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.Files = new System.Windows.Forms.ListBox();
            this.Where = new System.Windows.Forms.NoDblClickTreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblContext = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.compareExternallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 133);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Candidates);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(544, 368);
            this.splitContainer1.SplitterDistance = 181;
            this.splitContainer1.TabIndex = 0;
            // 
            // Candidates
            // 
            this.Candidates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Candidates.Location = new System.Drawing.Point(0, 0);
            this.Candidates.Name = "Candidates";
            this.Candidates.PathSeparator = "/";
            this.Candidates.Size = new System.Drawing.Size(181, 368);
            this.Candidates.TabIndex = 1;
            this.Candidates.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Candidates_AfterSelect);
            this.Candidates.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Candidates_MouseUp);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.Files);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.Where);
            this.splitContainer2.Size = new System.Drawing.Size(359, 368);
            this.splitContainer2.SplitterDistance = 105;
            this.splitContainer2.TabIndex = 0;
            // 
            // Files
            // 
            this.Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Files.FormattingEnabled = true;
            this.Files.Location = new System.Drawing.Point(0, 0);
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(359, 105);
            this.Files.TabIndex = 1;
            this.Files.SelectedIndexChanged += new System.EventHandler(this.Files_SelectedIndexChanged);
            // 
            // Where
            // 
            this.Where.CheckBoxes = true;
            this.Where.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Where.Location = new System.Drawing.Point(0, 0);
            this.Where.Name = "Where";
            this.Where.PathSeparator = "/";
            this.Where.Size = new System.Drawing.Size(359, 259);
            this.Where.TabIndex = 1;
            this.Where.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.Where_BeforeCheck);
            this.Where.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Where_MouseUp);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblContext, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(550, 423);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblContext
            // 
            this.lblContext.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContext.AutoSize = true;
            this.lblContext.Location = new System.Drawing.Point(3, 58);
            this.lblContext.Name = "lblContext";
            this.lblContext.Size = new System.Drawing.Size(0, 13);
            this.lblContext.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.openFileLocationToolStripMenuItem,
            this.toolStripSeparator2,
            this.compareExternallyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 104);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.openFileLocationToolStripMenuItem.Text = "Open file location ...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(268, 6);
            // 
            // compareExternallyToolStripMenuItem
            // 
            this.compareExternallyToolStripMenuItem.Name = "compareExternallyToolStripMenuItem";
            this.compareExternallyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.compareExternallyToolStripMenuItem.Text = "Compare ...";
            // 
            // DuplicateCandidatesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DuplicateCandidatesView";
            this.Size = new System.Drawing.Size(550, 423);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView Candidates;
        private System.Windows.Forms.ListBox Files;
        private System.Windows.Forms.NoDblClickTreeView Where;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblContext;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem compareExternallyToolStripMenuItem;
    }
}
