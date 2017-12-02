using System;
using System.IO;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.Core
{
    public interface ISynoCSVReport
    {
        void LoadReport(StreamReader source, FileInfo filename);
        void LoadReport(ISynoCSVReport component);
        DateTime Timestamp { get; }
    }
    public interface ISynoCSVReportPair
    {
        ISynoCSVReport First { get; }
        ISynoCSVReport Second { get; }
        void Initialize(ISynoCSVReport first, ISynoCSVReport second);
    }
    public interface ISynoChartData
    {
        List<string> Series { get; }
        IEnumerable<IXYDataPoint> this[string name] { get; }
        IEnumerable<IXYDataPoint> this[int index] { get; }
    }
    public interface IVolumePieChart : ISynoChartData
    {
        bool PercentageFreeOnly { get; set; }
        long TotalSize(int index);
        long TotalSize(string volume);
    }
    public interface IXYDataPoint
    {
        object X { get; }
        object Y { get; }
    }
}
