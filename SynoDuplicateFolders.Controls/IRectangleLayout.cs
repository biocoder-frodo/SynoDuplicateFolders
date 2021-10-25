namespace SynoDuplicateFolders.Controls
{
    interface IRectangleLayout
    {
        short Rows { get; }
        short Columns { get; }
        int Occupied { get; }
        int Rank { get; }
        bool LayoutFits { get; }
        double AspectRatio { get; }
        double FillWeight { get; }
    }
}
