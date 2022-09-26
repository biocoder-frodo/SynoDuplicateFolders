namespace SynoDuplicateFolders.Data.Core
{
    public interface IVolumePieChart : ISynoChartData
    {
        bool PercentageFreeOnly { get; set; }
        long TotalSize(int index);
        long TotalSize(string volume);
    }
}
