using System;
namespace SynoDuplicateFolders.Data.Core
{
    public interface IVolumePieChart : ISynoChartData
    {
        DateTime ContextTime { get; }
        bool PercentageFreeOnly { get; set; }
        long TotalSize(int index);
        long TotalSize(string volume);
    }
}
