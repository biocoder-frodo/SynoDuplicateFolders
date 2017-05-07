using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Extensions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders.Controls
{
    public partial class VolumeHistoricChart : UserControl
    {
        private ISynoReportCache src = null;
        private ISynoChartData data = null;
        private SynoReportType showing = SynoReportType.VolumeUsage;
        private IChartConfiguration _legends = null;
        private bool invalidated = false;
        private readonly List<string> unknown_traces = new List<string>();

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
                    if (ShowingType == SynoReportType.ShareList)
                    {
                        e.LocalizedValue = Convert.ToInt64(e.Value).ToFileSizeString(shares_range);
                    }
                }
            }
        }

        public IChartConfiguration Configuration
        {
            get { return _legends; }
            set { _legends = value; }
        }
        public SynoReportType ShowingType
        {
            get { return showing; }
            set
            {
                if (value == SynoReportType.VolumeUsage || value == SynoReportType.ShareList)
                {
                    if (showing != value)
                    {
                        showing = value;
                        DataSource = src;
                    }
                }
                else
                {
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
                    switch (showing)
                    {
                        case SynoReportType.ShareList:
                        case SynoReportType.VolumeUsage:
                            data = src.GetReport(showing) as ISynoChartData;
                            break;
                        default:
                            break;
                    }
                    if (data != null)
                    {
                        chart1.Series.Clear();
                        unknown_traces.Clear();

                        long peak = 0;

                        foreach (string s in data.Series)
                        {

                            var series1 = new Series
                            {
                                Name = s,
                                IsVisibleInLegend = true,
                                IsXValueIndexed = false,
                                ChartType = SeriesChartType.StepLine
                            };

                            Console.Write("trace " + s + ": ");
                            if (_legends.ContainsKey(s))
                            {
                                Console.WriteLine(" picking dictionary color");
                                series1.Color = _legends[s].Color;
                            }
                            else
                            {
                                unknown_traces.Add(s);
                                Console.WriteLine(" picking default color");
                            }

                            chart1.Series.Add(series1);


                            series1.Points.Clear();
                            if (showing == SynoReportType.ShareList)
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
                                    series1.Points.AddXY(dp.X, dp.Y);
                                }
                            }
                        }

                        shares_range = peak.GetFileSizeRange();

                        invalidated = true;

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
                    if (ShowingType == SynoReportType.ShareList)
                    {
                        ShowingType = SynoReportType.VolumeUsage;
                    }
                    else
                    {
                        ShowingType = SynoReportType.ShareList;
                    }
                    break;
                default:
                    break;
            }

        }

        private void chart1_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (invalidated && unknown_traces.Count > 0)
            {

                int index = 0;
                for (index = 0; index < data.Series.Count; index++)
                {
                    if (unknown_traces.Contains(data.Series[index]))
                    {
                        _legends.Add(data.Series[index], chart1.Series[index].Color, true);
                    }                    
                }
            }
            invalidated = false;
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            string text =string.Empty;
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                if (ShowingType == SynoReportType.ShareList)
                {
                    text = string.Format("{0}: {2} ({1})",
                        e.HitTestResult.Series.Name,
                        DateTime.FromOADate(e.HitTestResult.Series.Points[e.HitTestResult.PointIndex].XValue),
                        ((long)e.HitTestResult.Series.Points[e.HitTestResult.PointIndex].YValues[0]).ToFileSizeString());
                }
                else
                {
                    text = string.Format("{0}: {2:0.0}%, ({1})",
                        e.HitTestResult.Series.Name,
                        DateTime.FromOADate(e.HitTestResult.Series.Points[e.HitTestResult.PointIndex].XValue),
                        (float)e.HitTestResult.Series.Points[e.HitTestResult.PointIndex].YValues[0]);
                }
                if (!e.Text.Equals(text)) e.Text = text;
                //Console.WriteLine(e.Text);
            }
        }
    }
}
