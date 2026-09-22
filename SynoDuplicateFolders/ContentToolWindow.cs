using System;
using System.Windows.Forms;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Data.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SynoDuplicateFolders.Properties;

namespace SynoDuplicateFolders
{
    public partial class ContentToolWindow : Form
    {
        public ContentToolWindow(FormClosedEventHandler closedHandler)
        {
            InitializeComponent();
            chartGrid1.Configuration = CustomSettings.Profile;
            this.FormClosed += closedHandler;
        }

        public void Update(ISynoReportCache cache, DateTime time, ChartGridToolTipEventArgs e)
        {
            var report = cache.GetReport(time, SynoReportType.ShareList, SynoReportType.FileGroup) as SynoReportCategoryPieData;
            var dups = cache.GetReport(time, SynoReportType.DuplicateCandidates);

            if (report is null) return;
            this.Text = $"{e.Timestamp} {e.Text}";
            report.Share = e.DataPoint;
            report.PercentageFreeOnly = true;
            chartGrid1.DataSource = report;

            return;
            var list = (report as ISynoReportBindingSource<ISynoReportGroupDetail>).BindingSource.ToList();

            List<ISynoReportGroupDetail> scope;

            scope = TraceName.IsUsage(e.DataPoint) ? list : list.Where(r => r.Share == e.DataPoint).ToList();



            System.Diagnostics.Debug.WriteLine($"{e.Timestamp} {e.Series} {e.DataPoint} {e.Text}");


            var grouping = scope.GroupBy(r => r.Group).ToDictionary(g => g.Key, v => v.ToList());
            var totalSize = scope.Sum(r => (decimal)r.Size);
            var totalByGroup = grouping.Keys.ToDictionary(k => k, v => grouping[v].Sum(r => (decimal)r.Size));
            foreach (var pair in grouping)
            {
                var fileType = pair.Key;

                System.Diagnostics.Debug.WriteLine($"{fileType}  {totalByGroup[fileType]} / {totalSize}");

            }
        }
    }
}
