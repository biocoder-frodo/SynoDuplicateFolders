using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
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
