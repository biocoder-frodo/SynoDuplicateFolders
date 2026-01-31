using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders
{
    public class PaletteRow
    {
        private readonly PaletteColor paletteColor;
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

}
