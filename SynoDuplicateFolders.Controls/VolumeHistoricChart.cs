using Extensions;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders.Controls
{
    public partial class VolumeHistoricChart : UserControl
    {
        private ISynoReportCache src = null;
        private ISynoChartData data = null;

        private vhcViewMode _view = vhcViewMode.VolumeTotals;
        private SynoReportType _viewtype = SynoReportType.VolumeUsage;
        private List<string> _displaying_traces = null;

        private IChartConfiguration _legends = null;
        private LegendConfiguration _legendConfiguration = null;

        private FileSizeFormatSize shares_range = FileSizeFormatSize.PB;

        public DateTime? EarliestTime { get; set; }
        public TimeSpan? TimeRange { get; set; }
        public bool ShowIndividualStoragePoolUsage { get; set; }
        public VolumeHistoricChart()
        {
            InitializeComponent();
            chart1.Visible = false;
            chart1.FormatNumber += Chart1_FormatNumber;
        }

        private void Chart1_FormatNumber(object sender, FormatNumberEventArgs e)
        {
            if (sender is Axis)
            {
                Axis a = sender as Axis;
                if (a.AxisName == AxisName.Y)
                {
                    if (_view == vhcViewMode.Shares || _view == vhcViewMode.VolumeTotals)
                    {
                        e.LocalizedValue = Convert.ToInt64(e.Value).ToFileSizeString(shares_range);
                    }
                }
            }
        }

        public IChartConfiguration Configuration
        {
            get => _legends;
            set
            {
                _legends = value;
                _legendConfiguration = new LegendConfiguration(_legends);
            }
        }

        public vhcViewMode View
        {
            get { return _view; }
            set
            {
                switch (value)
                {
                    case vhcViewMode.Shares:
                    case vhcViewMode.Volume:
                    case vhcViewMode.VolumeTotals:
                        if (_view != value)
                        {
                            _view = value;
                            _viewtype = _view == vhcViewMode.Shares ? SynoReportType.ShareList : SynoReportType.VolumeUsage;
                            DataSource = src;

                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }
        public ISynoReportCache DataSource
        {
            set
            {
                src = value;
                if (src != null)
                {
                    switch (_viewtype)
                    {
                        case SynoReportType.ShareList:
                        case SynoReportType.VolumeUsage:
                            data = src.GetReport(_viewtype) as ISynoChartData;
                            break;
                        default:
                            break;
                    }
                    if (data != null)
                    {
                        chart1.Series.Clear();

                        _legendConfiguration.ResetUnknownTraces();

                        long peak = 0;

                        if (_viewtype == SynoReportType.VolumeUsage)
                        {
                            switch (_view)
                            {
                                case vhcViewMode.VolumeTotals:
                                    _displaying_traces = data.ActiveSeries.Where(s => TraceName.IsTotal(s)).ToList();
                                    break;
                                case vhcViewMode.Volume:
                                    _displaying_traces = (ShowIndividualStoragePoolUsage
                                        ? data.ActiveSeries.Where(s => s != "/volumes" && TraceName.IsTotal(s) == false)
                                        : data.ActiveSeries.Where(s => s == "/volumes")
                                        ).ToList();
                                    break;
                                default:
                                    _displaying_traces = data.ActiveSeries;
                                    break;
                            }
                        }
                        else
                        {
                            _displaying_traces = data.ActiveSeries;
                        }

                        foreach (string s in _displaying_traces)
                        {

                            Series series1 = new Series()
                            {
                                Name = s,
                                IsVisibleInLegend = true,
                                IsXValueIndexed = false,
                                ChartType = SeriesChartType.StepLine
                            };

                            _ = _legendConfiguration.TryPickColor(s, series1);

                            chart1.Series.Add(series1);

                            DateTime? timeLimit = null;
                            if (EarliestTime.HasValue || TimeRange.HasValue)
                            {
                                timeLimit = TimeRange.HasValue ? DateTime.Now.Subtract(TimeRange.Value) : EarliestTime.Value;
                            }
                            series1.Points.Clear();
                            switch (_viewtype)
                            {
                                case SynoReportType.ShareList:
                                    AddPoints(series1, s, ref peak, timeLimit);
                                    break;

                                case SynoReportType.VolumeUsage:
                                    if (TraceName.IsTotal(s))
                                    {
                                        AddPoints(series1, s, ref peak, timeLimit);
                                    }
                                    else
                                        AddPoints(series1, s, timeLimit);
                                    break;
                                default:
                                    AddPoints(series1, s, timeLimit);
                                    break;
                            }

                        }
                        shares_range = peak.GetFileSizeRange();

                        _legendConfiguration.Invalidate();

                        chart1.Invalidate();
                        chart1.Visible = true;
                    }
                }
            }
        }
        private void AddPoints(Series series, string trace, DateTime? minTs)
        {
            foreach (IXYDataPoint dp in data[trace])
            {
                if (minTs.HasValue == false || ((DateTime)dp.X) >= minTs.Value)
                    series.Points.AddXY(dp.X, dp.Y);
            }
        }
        private void AddPoints(Series series, string trace, ref long peak, DateTime? minTs)
        {
            foreach (IXYDataPoint dp in data[trace])
            {
                if (peak < (long)dp.Y) peak = (long)dp.Y;
                if (minTs.HasValue == false || ((DateTime)dp.X) >= minTs.Value)
                    series.Points.AddXY(dp.X, dp.Y);
            }
        }
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult h = chart1.HitTest(e.X, e.Y);

            System.Diagnostics.Debug.WriteLine($"HitTestResult: {h.ChartElementType}");

            switch (h.ChartElementType)
            {
                case ChartElementType.LegendItem:

                    LegendItem li = h.Object as LegendItem;
                    System.Diagnostics.Debug.WriteLine(li.SeriesName);
                    if (_viewtype == SynoReportType.VolumeUsage && _view == vhcViewMode.Volume)
                    {
                        ShowIndividualStoragePoolUsage = !ShowIndividualStoragePoolUsage;
                        DataSource = src;
                    }
                    break;

                case ChartElementType.Axis:
                    Axis a = h.Object as Axis;
                    switch (a.AxisName)
                    {
                        case AxisName.Y:
                            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                            break;
                        case AxisName.Y2:
                            break;
                        default:
                            break;
                    }
                    break;
                case ChartElementType.PlottingArea:
                    int v = Enum.GetValues(typeof(vhcViewMode)).Length;
                    View = (vhcViewMode)(((int)_view + 1) % v);
                    break;

                case ChartElementType.Nothing:
                    if (e.Button == MouseButtons.Right)
                    {
                        contextMenuStrip1.Show(MousePosition);
                    }
                    break;

                default:
                    break;
            }

        }

        private void chart1_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (_legendConfiguration.LegendUpdateNeeded)
            {
                if (_view == vhcViewMode.VolumeTotals)
                {
                    _legendConfiguration.AddNewTraces(data, chart1.Series);
                }
                else
                {
                    _legendConfiguration.AddNewTraces((idx) => data.Series[idx], (idx) => chart1.Series[idx].Color, _displaying_traces.Count);
                }
            }
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            string text = string.Empty;
            HitTestResult h = e.HitTestResult;

            if (h.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint dp = h.Series.Points[h.PointIndex];
                if (_viewtype == SynoReportType.ShareList)
                {
                    text = $"{h.Series.Name}: {((long)dp.YValues[0]).ToFileSizeString()} ({DateTime.FromOADate(dp.XValue)})";
                }
                else
                {
                    if (TraceName.IsTotal(h.Series.Name))
                    {
                        text = $"{h.Series.Name}: {((long)dp.YValues[0]).ToFileSizeString()} ({DateTime.FromOADate(dp.XValue)})";

                    }
                    else
                    {
                        text = $"{h.Series.Name}: {(float)dp.YValues[0]:0.0}%, ({DateTime.FromOADate(dp.XValue)})";
                    }
                }
                if (!e.Text.Equals(text)) e.Text = text;

            }
        }

        private void noTimeLimitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EarliestTime = null;
            TimeRange = null;
            DataSource = src;
        }

        private void lastWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EarliestTime = null;
            TimeRange = new TimeSpan(7, 0, 0, 0);
            DataSource = src;
        }

        private void lastMonthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EarliestTime = null;
            TimeRange = new TimeSpan(31, 0, 0, 0);
            DataSource = src;
        }

        private void lastYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EarliestTime = null;
            TimeRange = new TimeSpan(366, 0, 0, 0);
            DataSource = src;
        }
    }
    public enum vhcViewMode
    {
        Shares,
        Volume,
        VolumeTotals
    }
}
