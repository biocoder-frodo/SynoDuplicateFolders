using System.Drawing;

namespace SynoDuplicateFolders.Controls
{
    public interface IChartLegend : ITaggedColor
    {
        Color DefaultColor { get; }
        string ColorName { get; set; }
    }

}
