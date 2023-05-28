using SynoDuplicateFolders.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SynoDuplicateFolders.Extensions;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Controls
{
    public partial class ChartGrid : UserControl
    {
        private IVolumePieChart _src = null;
        private bool _first = true;
        private readonly ChartControls _charts;
        private IChartConfiguration _legends = null;
        private LegendConfiguration _legendConfiguration = null;
        private bool _percentage_free_only = false;
        public ChartGrid()
        {
            InitializeComponent();
            _charts = new ChartControls(chart_MouseClick, chart_GetToolTipText, chart_PostPaint);

        }
        public IChartConfiguration Configuration
        {
            get { return _legends; }
            set
            {
                if (value != null)
                {
                    _legends = value;

                    if (value.ContainsKey("Free") == false)
                    {
                        value.Add("Free", KnownColor.AntiqueWhite);
                    }
                    _legendConfiguration = new LegendConfiguration(value);
                }
            }
        }

        public IVolumePieChart DataSource
        {
            get { return _src; }
            set
            {
                _src = value;
#if DEBUG
                System.Diagnostics.Debug.WriteLineIf(_src == null, "IVolumePieChart DataSource reset");
#endif
                if (_src != null)
                {

                    if (_charts.Count != _src.Series.Count || _first == true)
                    {
                        _first = false;

                        _legendConfiguration.ResetUnknownTraces();


                        tableLayoutPanel1.Controls.Clear();
                        //flowLayoutPanel1.Controls.Clear();

                        _charts.Clear();
                        for (int z = 0; z < _src.Series.Count; z++)
                        {
                            _charts.Add(NewChart());
                        }
                        var layout = DetermineLayout();

                        tableLayoutPanel1.ColumnCount = layout.Columns;
                        tableLayoutPanel1.RowCount = layout.Rows;
                        int r = 0; int c = 0;
                        for (int z = 0; z < _src.Series.Count; z++)
                        {
                            //flowLayoutPanel1.Controls.Add(_charts[r]);
                            tableLayoutPanel1.Controls.Add(_charts[z], c++, r);
                            if (c == layout.Columns)
                            {
                                c = 0;
                                r++;
                            }
                        }
                        tableLayoutPanel1_SizeChanged(null, null);
                    }

                    _src.PercentageFreeOnly = _percentage_free_only;
                    for (int r = 0; r < _src.Series.Count; r++)
                    {
                        RenderData(r, _charts[r]);
                    }
                    _legendConfiguration.Invalidate();
                }
            }
        }

        private void RenderData(int index, Chart target)
        {
            if (target.Series.Count.Equals(0))
            {
                target.Series.Add(_src.Series[index]);
            }
            if (target.Titles.Count.Equals(0))
            {
                target.Titles.Add(_src.Series[index]);
            }
            if (target.Series[0].ChartType != SeriesChartType.Pie)
                target.Series[0].ChartType = SeriesChartType.Pie;

            target.Series[0].Name = _src.Series[index];
            DataPointCollection dpc = target.Series[0].Points;
            dpc.Clear();
            foreach (PieChartDataPoint dp in _src[index])
            {
                dpc[dpc.AddXY(dp.SliceName, dp.Value)].LegendText = dp.SliceName;
                _ = _legendConfiguration.TryPickColor(dp.SliceName, dpc[dpc.Count - 1]);
            }

        }
        private Chart NewChart()
        {
            Chart c = new Chart();

            c.ChartAreas.Add(new ChartArea());
            c.Legends.Add(new Legend());
            c.Series.Add(new Series());
            c.Height = 300;
            c.Width = 300;

            return c;
        }

        private void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            string text = string.Empty;
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                HitTestResult h = e.HitTestResult;
                Series hovered = h.Series;
                DataPoint dp = hovered.Points[h.PointIndex];
                if (_src != null)
                {
                    double size = _src.TotalSize(hovered.Name) / 100.0;

                    if (dp.LegendText.Equals("Free") || dp.LegendText.Equals("Used"))
                    {
                        text = string.Format("{0}: {2} ({1:0.0}%)",
                            hovered.Name + " " + dp.LegendText,
                            dp.YValues[0],
                            ((long)(size * dp.YValues[0])).ToFileSizeString());
                    }
                    else
                    {
                        text = string.Format("{0}: {2} ({1:0.0}%)",
                            hovered.Name + "/" + dp.LegendText,
                            dp.YValues[0],
                            ((long)(size * dp.YValues[0])).ToFileSizeString());
                    }
                }
            }
            if (!e.Text.Equals(text)) e.Text = text;
        }
        private void chart_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (_src != null)
            {
                for (int r = 0; r < _charts.Count; r++)
                {
                    if (sender == _charts[r])
                    {
                        _legendConfiguration.AddNewTraces(r, _charts[r], _src);
                    };
                }
            }
        }
        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            _percentage_free_only = !_percentage_free_only;
            DataSource = _src;

        }
        private RectangleLayout DetermineLayout()
        {
            //  int onesquare = (int)Math.Ceiling(Math.Sqrt(count));
            // System.Diagnostics.Debug.WriteLine($"one square with {count}: {onesquare}x{onesquare}");

            var h = Convert.ToDouble(Height);
            var w = Convert.ToDouble(Width);
            int count = _src.Series.Count;

            double target_ratio = w / h;

            double pickme = double.MaxValue;
            RectangleLayout view = null;
            foreach (var layout in RectangleLayout.Layouts(count))
            {
                double ratio_match = 1.0 + Math.Abs(Math.Log10(target_ratio / layout.AspectRatio));
                double weighingfactor = layout.FillWeight * ratio_match;
#if DEBUG
                //                System.Diagnostics.Debug.WriteLine($"rows: {layout.Rows} columns: {layout.Columns} rank: {layout.Rank}  weighingfactor: {weighingfactor}");
#endif
                if (weighingfactor < pickme)
                {
                    pickme = weighingfactor;
                    view = layout;
                }
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"layout: {view.Rows} x {view.Columns}");
#endif
            return view;
        }



        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            if (_src != null)
            {
                var layout = DetermineLayout();

                if (tableLayoutPanel1.ColumnCount != layout.Columns
                    || tableLayoutPanel1.RowCount != layout.Rows)
                {
                    tableLayoutPanel1.Controls.Clear();
                    tableLayoutPanel1.ColumnCount = layout.Columns;
                    tableLayoutPanel1.RowCount = layout.Rows;
                    int r = 0; int c = 0;
                    for (int z = 0; z < _src.Series.Count; z++)
                    {
                        //flowLayoutPanel1.Controls.Add(_charts[r]);
                        tableLayoutPanel1.Controls.Add(_charts[z], c++, r);
                        if (c == layout.Columns)
                        {
                            c = 0;
                            r++;
                        }

                    }
                }

                //var p = Math.Sqrt(Height * Width / Convert.ToDouble(_src.Series.Count));
                //if (Width / Convert.ToDouble(_src.Series.Count) < p)
                //{
                //    p = Width / Convert.ToDouble(_src.Series.Count);
                //}
                for (int z = 0; z < _src.Series.Count; z++)
                {
                    _charts[z].Width = (int)(0.95 * Width / Convert.ToDouble(layout.Columns));
                    _charts[z].Height = (int)(0.95 * Height / Convert.ToDouble(layout.Rows));

                }
            }
        }
    }
}
