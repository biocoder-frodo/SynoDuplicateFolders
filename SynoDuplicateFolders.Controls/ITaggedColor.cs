using System.Drawing;

namespace SynoDuplicateFolders.Controls
{
    public interface ITaggedColor
    {
        string Key { get; set; }
        Color Color { get; set; }
    }
}
