
namespace SynoDuplicateFolders.Controls.DesignerWorkAround
{
    partial class Form1
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
            this.timeStampTrackBar1 = new SynoDuplicateFolders.Controls.TimeStampTrackBar();
            this.duplicateCandidatesView1 = new SynoDuplicateFolders.Controls.DuplicateCandidatesView();
            this.SuspendLayout();
            // 
            // timeStampTrackBar1
            // 
            this.timeStampTrackBar1.Location = new System.Drawing.Point(41, 21);
            this.timeStampTrackBar1.Name = "timeStampTrackBar1";
            this.timeStampTrackBar1.Size = new System.Drawing.Size(458, 79);
            this.timeStampTrackBar1.TabIndex = 0;
            // 
            // duplicateCandidatesView1
            // 
            this.duplicateCandidatesView1.ExclusionSource = null;
            this.duplicateCandidatesView1.HostName = null;
            this.duplicateCandidatesView1.Location = new System.Drawing.Point(41, 106);
            this.duplicateCandidatesView1.MaximumComparable = 3;
            this.duplicateCandidatesView1.Name = "duplicateCandidatesView1";
            this.duplicateCandidatesView1.Size = new System.Drawing.Size(550, 423);
            this.duplicateCandidatesView1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.duplicateCandidatesView1);
            this.Controls.Add(this.timeStampTrackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TimeStampTrackBar timeStampTrackBar1;
        private DuplicateCandidatesView duplicateCandidatesView1;
    }
}

