using SynoDuplicateFolders.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders.Controls
{
    public partial class ChartGrid : UserControl
    {
        private IVolumePieChart _src = null;
        private bool _first = true;
        private readonly List<Chart> _charts = new List<Chart>();
        private IChartConfiguration _legends = null;
        private bool _percentage_free_only = false;
        public ChartGrid()
        {
            InitializeComponent();           
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
                }
            }
        }

        public IVolumePieChart DataSource
        {
            get { return _src; }
            set
            {
                _src = value;
                if (_src != null)
                {

                    if (_charts.Count != _src.Series.Count || _first == true)
                    {
                        _first = false;
                        //SizePanel(flowLayoutPanel1, _src.Count);
                        flowLayoutPanel1.Controls.Clear();
                        foreach (Chart c in _charts)
                        {
                            c.MouseClick -= new MouseEventHandler(chart_MouseClick);
                        }
                        _charts.Clear();
                        for (int r = 0; r < _src.Series.Count; r++)
                        {
                            _charts.Add(NewChart());
                            //_charts[r].Invalidate();
                            flowLayoutPanel1.Controls.Add(_charts[r]);
                            //flowLayoutPanel1.Controls.Add(new Button());

                        }
                    }

                    _src.PercentageFreeOnly = _percentage_free_only;
                    for (int r = 0; r < _src.Series.Count; r++)
                    {
                        RenderData(r, _charts[r]);
                    }
                }
           }
        }

        private void RenderData(int index, Chart target)
        {
            if (target.Series.Count.Equals(0))
            {
                target.Series.Add("Series Name - " + _src.Series[index]);
            }
            if (target.Titles.Count.Equals(0))
            {
                target.Titles.Add(_src.Series[index]);
            }
            if (target.Series[0].ChartType != SeriesChartType.Pie)
                target.Series[0].ChartType = SeriesChartType.Pie;


            DataPointCollection dpc = target.Series[0].Points;
            dpc.Clear();
            foreach (PieChartDataPoint dp in _src[index])
            {
                dpc.AddXY(dp.SliceName, dp.Value);

                if (_legends.ContainsKey(dp.SliceName))
                {
                    dpc[dpc.Count - 1].Color = _legends[dp.SliceName].Color;
                }
                else
                {
                  // _legends.Add(dp.SliceName, dpc[dpc.Count - 1].Color);
                }
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
            c.MouseClick += new MouseEventHandler(chart_MouseClick);
            return c;
        }
        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            _percentage_free_only = !_percentage_free_only;
            DataSource = _src;

        }
        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            if (_src != null)
            {
                var a = flowLayoutPanel1.Height;
                var b = flowLayoutPanel1.Width;

                for (int r = 0; r < _src.Series.Count; r++)
                {
                     _charts[r].Width = b/2;
                    _charts[r].Height = b/2;
                }
            }
        }
    }
}
