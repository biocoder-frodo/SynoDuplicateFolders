using SynoDuplicateFolders.Data.Core;
using System.Collections.Generic;
using System.Linq;

namespace SynoDuplicateFolders.Data
{
    internal class DuplicatesHistogramValue : IDuplicatesHistogramValue
    {
        public DuplicatesHistogramValue(IGrouping<long, List<DuplicateFileInfo>> x, long bucketSize)
        {
            Minimum = x.Key;
            Maximum = x.Key + bucketSize - 1;
            UniqueSize = x.Sum(d => d.First().Length);
            TotalSize = x.Sum(d => SumTotal(d));
        }
        private long SumTotal(List<DuplicateFileInfo> duplicates)
        {
            long sum = 0;
            foreach (DuplicateFileInfo dup in duplicates)
            {
                sum += dup.Length;
            }
            return sum;
        }
        public long Count { get; }
        public long Maximum { get; }
        public long Minimum { get; }
        public long UniqueSize { get; }
        public long TotalSize { get; }
    }
}
