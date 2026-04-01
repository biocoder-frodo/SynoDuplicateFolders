
namespace SynoDuplicateFolders
{
    partial class ContentToolWindow
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
            this.chartGrid1 = new SynoDuplicateFolders.Controls.ChartGrid(false);
            this.SuspendLayout();
            // 
            // chartGrid1
            // 
            this.chartGrid1.Configuration = null;
            this.chartGrid1.DataSource = null;
            this.chartGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartGrid1.Location = new System.Drawing.Point(0, 0);
            this.chartGrid1.Name = "chartGrid1";
            this.chartGrid1.Size = new System.Drawing.Size(800, 450);
            this.chartGrid1.TabIndex = 0;
            // 
            // ContentToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ContentToolWindow";
            this.ShowInTaskbar = false;
            this.Text = "ContentToolWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ChartGrid chartGrid1;
    }
}