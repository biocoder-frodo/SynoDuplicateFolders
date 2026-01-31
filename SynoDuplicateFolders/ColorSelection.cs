using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders
{

    internal partial class ColorSelection : Form
    {
        public bool Canceled { get; private set; }
        private readonly ChartLegends config;
   
        private readonly IReadOnlyDictionary<ChartColorPalette, IReadOnlyList<Color>> _map;
        private readonly Dictionary<ChartColorPalette, CheckBox> checkBoxes;

        private Color selection;
        private Color? presetColorHovered = null;

        public Color Selection 
        { 
            get => selection; 
            private set  
            {
                selection = value;
                pictureBox1.BackColor = selection;
            }
        }

        public ColorSelection(IReadOnlyDictionary<ChartColorPalette, IReadOnlyList<Color>> map, ChartLegends config, Color current)
        {
            InitializeComponent();

            Selection = current;
            this.config = config;
            pictureBox2.BackColor = current;

            _map = map;
            Canceled = true;
            checkBoxes = new Dictionary<ChartColorPalette, CheckBox>()
            {
                { ChartColorPalette.Bright,          checkBox1  },
                { ChartColorPalette.Grayscale,       checkBox2  },
                { ChartColorPalette.Excel,           checkBox3  },
                { ChartColorPalette.Light,           checkBox4  },
                { ChartColorPalette.Pastel,          checkBox5  },
                { ChartColorPalette.EarthTones,      checkBox6  },
                { ChartColorPalette.SemiTransparent, checkBox7  },
                { ChartColorPalette.Berry,           checkBox8  },
                { ChartColorPalette.Chocolate,       checkBox9  },
                { ChartColorPalette.Fire,            checkBox10 },
                { ChartColorPalette.SeaGreen,        checkBox11 },
                { ChartColorPalette.BrightPastel,    checkBox12 },
            };

            checkBoxes.Keys.ToList().ForEach(p => { checkBoxes[p].Tag = p; checkBoxes[p].Text = p.ToString(); });

            foreach (var p in config.PresetPalettes)
            {
                checkBoxes[p].Checked = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            UpdateColorWheel();

        }
        private void UpdateColorWheel()
        {
            chart1.Series.Clear();
            chart1.Legends.Clear();
            var unique = new List<int>();
            var colors = new Dictionary<PaletteColor, Color>();
            var distanceMap = new Dictionary<PaletteColorPair, double>();
            foreach (var p in _map.Keys)
            {
                if (checkBoxes[p].Checked)
                {
                    for (int idx = 0; idx < _map[p].Count; idx++)
                    {
                        Color color = _map[p][idx];

                        if (unique.Contains(color.ToArgb()) == false)
                        {
                            unique.Add(color.ToArgb());
                            colors.Add(new PaletteColor(p, idx), color);
                            if (colors.Count > 1)
                            {
                                for (int idxd = 0; idxd < colors.Count - 1; idxd++)
                                {
                                    var pair = new PaletteColorPair(colors.Keys.Last(), colors.Keys.ElementAt(idxd));
                                    distanceMap.Add(pair, pair.GetDistance(colors));
                                }
                            }
                        }
                    }
                }
            }
            distanceMap = distanceMap.OrderBy(p => p.Value).ToDictionary(k => k.Key, v => v.Value);
            var plotted = new List<int>();
            var series = new Series();
            chart1.Series.Add(series);

            series.ChartType = SeriesChartType.Pie;
            series["PieLabelStyle"] = "Disabled";
            chart1.BackSecondaryColor = chart1.BackColor;
            DataPointCollection dpc = series.Points;
            int i = 0;
            foreach (var pair in distanceMap.Keys)
            {
                if (plotted.Contains(colors[pair.First].ToArgb()) == false)
                {
                    dpc.AddXY($"{pair.First.Palette}-{pair.First.Index}", 1);
                    dpc[i++].Color = colors[pair.First];
                    plotted.Add(dpc[i - 1].Color.ToArgb());
                }

                if (plotted.Contains(colors[pair.Second].ToArgb()) == false)
                {
                    dpc.AddXY($"{pair.Second.Palette}-{pair.Second.Index}", 1);
                    dpc[i++].Color = colors[pair.Second];
                    plotted.Add(dpc[i - 1].Color.ToArgb());
                }
            }
            foreach (var key in colors.Keys)
            {
                if (plotted.Contains(colors[key].ToArgb()) == false)
                {
                    dpc.AddXY($"{key.Palette}-{key.Index}", 1);
                    dpc[i++].Color = colors[key];
                    plotted.Add(dpc[i - 1].Color.ToArgb());
                }
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            var ctl = sender as CheckBox;

            UpdateColorWheel();

            var prefs = new List<ChartColorPalette>();
            foreach (var p in checkBoxes.Keys)
            {
                if (checkBoxes[p].Checked) prefs.Add(p);
            }
            config.PresetPalettes = prefs;

        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            presetColorHovered = null;
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                HitTestResult h = e.HitTestResult;
                Series hovered = h.Series;
                DataPoint dp = hovered.Points[h.PointIndex];

                presetColorHovered = dp.Color;
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            if (presetColorHovered.HasValue)
            {
                Selection = presetColorHovered.Value;
            }
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            colorDialog1.AnyColor = false;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Selection = colorDialog1.Color;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            Canceled = false;
            Hide();
        }
    }

}
