using System;
using System.Collections.Generic;
using System.IO;

namespace SynoDuplicateFolders.Data
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

    public struct TimeLineDataPoint<T> : IXYDataPoint where T : struct
    {
        public readonly DateTime TimeStamp;
        public readonly T Value;
        public TimeLineDataPoint(DateTime ts, T value)
        {
            TimeStamp = ts;
            Value = value;
        }

        public object X
        {
            get
            {
                return TimeStamp;
            }
        }

        public object Y
        {
            get
            {
                return Value;
            }
        }
    }
    public struct PieChartDataPoint : IXYDataPoint
    {
        public readonly string SliceName;
        public readonly float Value;

        public PieChartDataPoint(string slice, float value)
        {
            SliceName = slice;
            Value = value;
        }

        public object X
        {
            get
            {
                return SliceName;
            }
        }

        public object Y
        {
            get
            {
                return Value;
            }
        }
    }
}
 
