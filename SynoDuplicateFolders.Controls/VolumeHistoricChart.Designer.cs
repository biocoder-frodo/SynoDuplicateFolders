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
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noTimeLimitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastWeekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastMonthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastYearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1030, 576);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart1_GetToolTipText);
            this.chart1.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.chart1_PostPaint);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noTimeLimitsToolStripMenuItem,
            this.lastWeekToolStripMenuItem,
            this.lastMonthToolStripMenuItem,
            this.lastYearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 92);
            // 
            // noTimeLimitsToolStripMenuItem
            // 
            this.noTimeLimitsToolStripMenuItem.Name = "noTimeLimitsToolStripMenuItem";
            this.noTimeLimitsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.noTimeLimitsToolStripMenuItem.Text = "No time limits";
            this.noTimeLimitsToolStripMenuItem.Click += new System.EventHandler(this.noTimeLimitsToolStripMenuItem_Click);
            // 
            // lastWeekToolStripMenuItem
            // 
            this.lastWeekToolStripMenuItem.Name = "lastWeekToolStripMenuItem";
            this.lastWeekToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastWeekToolStripMenuItem.Text = "Last week";
            this.lastWeekToolStripMenuItem.Click += new System.EventHandler(this.lastWeekToolStripMenuItem_Click);
            // 
            // lastMonthToolStripMenuItem
            // 
            this.lastMonthToolStripMenuItem.Name = "lastMonthToolStripMenuItem";
            this.lastMonthToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastMonthToolStripMenuItem.Text = "Last month";
            this.lastMonthToolStripMenuItem.Click += new System.EventHandler(this.lastMonthToolStripMenuItem_Click);
            // 
            // lastYearToolStripMenuItem
            // 
            this.lastYearToolStripMenuItem.Name = "lastYearToolStripMenuItem";
            this.lastYearToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.lastYearToolStripMenuItem.Text = "Last year";
            this.lastYearToolStripMenuItem.Click += new System.EventHandler(this.lastYearToolStripMenuItem_Click);
            // 
            // VolumeHistoricChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "VolumeHistoricChart";
            this.Size = new System.Drawing.Size(1030, 576);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem noTimeLimitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastWeekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastMonthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastYearToolStripMenuItem;
    }
}
