using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SynoDuplicateFolders.Data
{
    public class SynoReportVolumeUsageValue
    {
        public readonly string Volume;
        public readonly long Size;
        public readonly float Used;
        public readonly int? DaysTillFull;
        public readonly bool IsAggregate;
        public SynoReportVolumeUsageValue(string volume, long size, float used, int? daysTillFull, bool isAggregate = false)
        {
            Volume = volume;
            Size = size;
            Used = used;
            DaysTillFull = daysTillFull;
            IsAggregate = isAggregate;
        }
    }

    public class SynoReportVolumeUsageValueDictionary : Dictionary<string, SynoReportVolumeUsageValue>
    {
        private readonly List<string> keys = new List<string>();
        public SynoReportVolumeUsageValueDictionary()
        {

        }
        private new void Add(string key, SynoReportVolumeUsageValue value)
        {
            throw new NotImplementedException();
        }
        public void Add(SynoReportVolumeUsageValue value)
        {
            keys.Add(value.Volume);
            base.Add(value.Volume, value);
        }
        public SynoReportVolumeUsageValue this[int index] => this[keys[index]];
        public IList<string> KeyList => keys;
    }
}
