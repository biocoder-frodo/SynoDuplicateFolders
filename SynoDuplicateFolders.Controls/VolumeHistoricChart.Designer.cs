namespace SynoDuplicateFolders.Controls
{
    partial class VolumeHistoricChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.volumeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TimeRangeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noTimeLimitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastWeekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastMonthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastYearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.volumeChart)).BeginInit();
            this.TimeRangeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.volumeChart.ChartAreas.Add(chartArea3);
            this.volumeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.volumeChart.Legends.Add(legend3);
            this.volumeChart.Location = new System.Drawing.Point(0, 0);
            this.volumeChart.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.volumeChart.Series.Add(series3);
            this.volumeChart.Size = new System.Drawing.Size(1030, 576);
            this.volumeChart.TabIndex = 0;
            this.volumeChart.Text = "volumeChart";
            this.volumeChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.VolumeChart_GetToolTipText);
            this.volumeChart.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.VolumeChart_PostPaint);
            this.volumeChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.VolumeChart_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.TimeRangeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noTimeLimitsToolStripMenuItem,
            this.lastWeekToolStripMenuItem,
            this.lastMonthToolStripMenuItem,
            this.lastYearToolStripMenuItem});
            this.TimeRangeContextMenuStrip.Name = "TimeRangeContextMenuStrip";
            this.TimeRangeContextMenuStrip.Size = new System.Drawing.Size(150, 92);
            // 
            // noTimeLimitsToolStripMenuItem
            // 
            this.noTimeLimitsToolStripMenuItem.Name = "noTimeLimitsToolStripMenuItem";
            this.noTimeLimitsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.noTimeLimitsToolStripMenuItem.Text = "No time limits";
            this.noTimeLimitsToolStripMenuItem.Click += new System.EventHandler(this.TimeRangeContextMenuStripItem_Click);
            // 
            // lastWeekToolStripMenuItem
            // 
            this.lastWeekToolStripMenuItem.Name = "lastWeekToolStripMenuItem";
            this.lastWeekToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastWeekToolStripMenuItem.Text = "Last week";
            this.lastWeekToolStripMenuItem.Click += new System.EventHandler(this.TimeRangeContextMenuStripItem_Click);
            // 
            // lastMonthToolStripMenuItem
            // 
            this.lastMonthToolStripMenuItem.Name = "lastMonthToolStripMenuItem";
            this.lastMonthToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastMonthToolStripMenuItem.Text = "Last month";
            this.lastMonthToolStripMenuItem.Click += new System.EventHandler(this.TimeRangeContextMenuStripItem_Click);
            // 
            // lastYearToolStripMenuItem
            // 
            this.lastYearToolStripMenuItem.Name = "lastYearToolStripMenuItem";
            this.lastYearToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastYearToolStripMenuItem.Text = "Last year";
            this.lastYearToolStripMenuItem.Click += new System.EventHandler(this.TimeRangeContextMenuStripItem_Click);
            // 
            // VolumeHistoricChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.volumeChart);
            this.Name = "VolumeHistoricChart";
            this.Size = new System.Drawing.Size(1030, 576);
            ((System.ComponentModel.ISupportInitialize)(this.volumeChart)).EndInit();
            this.TimeRangeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart volumeChart;
        private System.Windows.Forms.ContextMenuStrip TimeRangeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem noTimeLimitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastWeekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastMonthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastYearToolStripMenuItem;
    }
}
