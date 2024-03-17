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

                        _displaying_traces = data.ActiveSeries;
                        if (_viewtype == SynoReportType.VolumeUsage)
                        {
                            _displaying_traces = _displaying_traces.Where(s => s.Contains("Total") == (_view == vhcViewMode.VolumeTotals)).ToList();
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


                            series1.Points.Clear();
                            if (_viewtype == SynoReportType.ShareList)
                            {

                                foreach (IXYDataPoint dp in data[s])
                                {
                                    if (peak < (long)dp.Y) peak = (long)dp.Y;
                                    series1.Points.AddXY(dp.X, dp.Y);
                                }
                            }
                            else
                            {
                                foreach (IXYDataPoint dp in data[s])
                                {
                                    if (_viewtype == SynoReportType.VolumeUsage && s.Contains("Total"))
                                    {
                                        if (peak < (long)dp.Y) peak = (long)dp.Y;
                                    }
                                    series1.Points.AddXY(dp.X, dp.Y);
                                }
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

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult h = chart1.HitTest(e.X, e.Y);
            switch (h.ChartElementType)
            {
                case ChartElementType.LegendItem:

                    LegendItem li = h.Object as LegendItem;
                    Console.WriteLine(li.SeriesName);
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
                default:
                    break;
            }

        }

        private void chart1_PostPaint(object sender, ChartPaintEventArgs e)
        {
            _legendConfiguration.AddNewTraces(_displaying_traces.Count, (idx) => data.Series[idx], (idx) => chart1.Series[idx].Color);
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
                    text = string.Format("{0}: {2} ({1})",
                        h.Series.Name,
                        DateTime.FromOADate(dp.XValue),
                        ((long)dp.YValues[0]).ToFileSizeString());
                }
                else
                {
                    if (h.Series.Name.Contains("Total"))
                    {
                        text = string.Format("{0}: {2} ({1})",
                            h.Series.Name,
                            DateTime.FromOADate(dp.XValue),
                            ((long)dp.YValues[0]).ToFileSizeString());

                    }
                    else
                    {
                        text = string.Format("{0}: {2:0.0}%, ({1})",
                            h.Series.Name,
                            DateTime.FromOADate(dp.XValue),
                            (float)dp.YValues[0]);
                    }
                }
                if (!e.Text.Equals(text)) e.Text = text;

            }
        }
    }
    public enum vhcViewMode
    {
        Shares,
        Volume,
        VolumeTotals
    }
}
