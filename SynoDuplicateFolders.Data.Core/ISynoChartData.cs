using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.Core
{
    public interface ISynoChartData
    {
        List<string> ActiveSeries { get; }
        List<string> Series { get; }
        IEnumerable<IXYDataPoint> this[string name] { get; }
        IEnumerable<IXYDataPoint> this[int index] { get; }
    }
}
