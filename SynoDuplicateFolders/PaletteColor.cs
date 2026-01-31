using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders
{
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

}
