using System;
using System.Collections.Generic;
using System.Text;

namespace SynoDuplicateFolders.Controls
{
    public class ChartGridToolTipEventArgs: EventArgs
    {
        public readonly string Text;
        public readonly string Series;
        public readonly string DataPoint;
        public readonly DateTime Timestamp;
        public ChartGridToolTipEventArgs(DateTime context, string text, string series, string dataPoint)
        {
            Timestamp = context;
            Text = text;
            Series = series;
            DataPoint = dataPoint;
        }
    }
}
