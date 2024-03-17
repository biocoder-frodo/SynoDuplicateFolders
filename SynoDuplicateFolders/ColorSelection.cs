using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Configuration;

namespace SynoDuplicateFolders
{

    internal partial class ColorSelection : Form
    {
        public bool Canceled { get; private set; }
       
   
        private readonly IReadOnlyDictionary<ChartColorPalette, IReadOnlyList<Color>> _map;
        private readonly Dictionary<ChartColorPalette, CheckBox> checkBoxes;

        private Color selection;
        private Color? presetColorHovered = null;

        public Color Selection { get => selection; private set  { selection = value; pictureBox1.BackColor = selection; } }

        public ColorSelection(IReadOnlyDictionary<ChartColorPalette, IReadOnlyList<Color>> map, ChartLegends config, Color current)
        {
            InitializeComponent();
            Selection = current;
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

            foreach (var p in config.Palettes.Split(';').Select(s => (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), s, true)))
            {
                checkBoxes[p].Checked = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
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

    public class PaletteRow
    {
        private PaletteColor paletteColor;
        public PaletteRow(ChartColorPalette palette, int index, Color color)
        {
            this.paletteColor = new PaletteColor(palette, index);
            Color = color;
        }
        public string Name => paletteColor.Palette.ToString();

        public int Index => paletteColor.Index;
        public PaletteColor PaletteColor => paletteColor;
        public Color Color { get; }
    }
    public struct PaletteColor
    {
        public readonly ChartColorPalette Palette;
        public readonly int Index;
        public PaletteColor(ChartColorPalette palette, int index)
        {
            Palette = palette;
            Index = index;
        }
    }
    public struct PaletteColorPair
    {
        public readonly PaletteColor First;
        public readonly PaletteColor Second;
        public PaletteColorPair(PaletteColor first, PaletteColor second)
        {
            First = first;
            Second = second;
        }

        public double GetDistance(Dictionary<PaletteColor, Color> map)
        {
            double a1 = map[First].A / 255d;
            double r1 = map[First].R / 255d;
            double g1 = map[First].G / 255d;
            double b1 = map[First].B / 255d;

            double a2 = map[Second].A / 255d;
            double r2 = map[Second].R / 255d;
            double g2 = map[Second].G / 255d;
            double b2 = map[Second].B / 255d;
            // euclidian distance
            var d = Math.Sqrt((a1 - a2) * (a1 - a2) + (r1 - r2) * (r1 - r2) + (g1 - g2) * (g1 - g2) + (b1 - b2) * (b1 - b2));
            if (d == 0)
            {
                System.Diagnostics.Debug.WriteLine($"colors are the same {map[First]}");
            }
            if (d == 1)
            {
                System.Diagnostics.Debug.WriteLine($"unity distance for {map[First]} to {map[Second]} ");
            }

            return d;
        }

    }

}
