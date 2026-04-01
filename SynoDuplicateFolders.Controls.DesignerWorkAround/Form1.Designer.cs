
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
            this.duplicateCandidatesView1 = new SynoDuplicateFolders.Controls.DuplicateCandidatesView();
            this.timeStampTrackBar1 = new SynoDuplicateFolders.Controls.TimeStampTrackBar();
            this.chartGrid1 = new SynoDuplicateFolders.Controls.ChartGrid();
            this.hostTextBox1 = new SynoDuplicateFolders.Controls.HostTextBox();
            this.volumeHistoricChart1 = new SynoDuplicateFolders.Controls.VolumeHistoricChart();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // duplicateCandidatesView1
            // 
            this.duplicateCandidatesView1.ExclusionSource = null;
            this.duplicateCandidatesView1.HostName = null;
            this.duplicateCandidatesView1.Location = new System.Drawing.Point(522, 30);
            this.duplicateCandidatesView1.MaximumComparable = 3;
            this.duplicateCandidatesView1.Name = "duplicateCandidatesView1";
            this.duplicateCandidatesView1.Size = new System.Drawing.Size(754, 423);
            this.duplicateCandidatesView1.TabIndex = 1;
            // 
            // timeStampTrackBar1
            // 
            this.timeStampTrackBar1.Location = new System.Drawing.Point(25, 12);
            this.timeStampTrackBar1.Name = "timeStampTrackBar1";
            this.timeStampTrackBar1.Size = new System.Drawing.Size(458, 79);
            this.timeStampTrackBar1.TabIndex = 0;
            this.timeStampTrackBar1.Value = new System.DateTime(((long)(0)));
            // 
            // chartGrid1
            // 
            this.chartGrid1.Configuration = null;
            this.chartGrid1.DataSource = null;
            this.chartGrid1.Location = new System.Drawing.Point(41, 459);
            this.chartGrid1.Name = "chartGrid1";
            this.chartGrid1.Size = new System.Drawing.Size(1235, 324);
            this.chartGrid1.TabIndex = 2;
            // 
            // hostTextBox1
            // 
            this.hostTextBox1.Location = new System.Drawing.Point(41, 122);
            this.hostTextBox1.Name = "hostTextBox1";
            this.hostTextBox1.Size = new System.Drawing.Size(323, 39);
            this.hostTextBox1.TabIndex = 3;
            // 
            // volumeHistoricChart1
            // 
            this.volumeHistoricChart1.Configuration = null;
            this.volumeHistoricChart1.EarliestTime = null;
            this.volumeHistoricChart1.Location = new System.Drawing.Point(41, 184);
            this.volumeHistoricChart1.Name = "volumeHistoricChart1";
            this.volumeHistoricChart1.ShowIndividualStoragePoolUsage = false;
            this.volumeHistoricChart1.Size = new System.Drawing.Size(458, 269);
            this.volumeHistoricChart1.TabIndex = 4;
            this.volumeHistoricChart1.TimeRange = null;
            this.volumeHistoricChart1.View = SynoDuplicateFolders.Controls.vhcViewMode.VolumeTotals;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(143, 280);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 838);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.volumeHistoricChart1);
            this.Controls.Add(this.hostTextBox1);
            this.Controls.Add(this.chartGrid1);
            this.Controls.Add(this.duplicateCandidatesView1);
            this.Controls.Add(this.timeStampTrackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TimeStampTrackBar timeStampTrackBar1;
        private DuplicateCandidatesView duplicateCandidatesView1;
        private ChartGrid chartGrid1;
        private HostTextBox hostTextBox1;
        private VolumeHistoricChart volumeHistoricChart1;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

