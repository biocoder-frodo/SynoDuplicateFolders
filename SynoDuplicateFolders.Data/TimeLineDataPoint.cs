using System;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
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
}
 