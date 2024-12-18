using System;
using System.Collections.Generic;
using System.Linq;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportVolumePieData : SynoCSVReportPair<SynoReportSharesValues, SynoReportVolumeUsageValues>, IVolumePieChart, ISynoChartData
    {
        private SynoReportSharesValues _shares;
        private SynoReportVolumeUsageValues _volumes;
        private bool _render_volume_only;


        public SynoReportVolumePieData(ISynoCSVReport first, ISynoCSVReport second)
            : base(first, second)
        {
            _shares = First;
            _volumes = Second;
            _render_volume_only = false;
        }

        public bool PercentageFreeOnly
        {
            get { return _render_volume_only; }
            set { _render_volume_only = value; }
        }

        public List<string> Series
        {
            get
            {
                return _volumes.Volumes.Keys.ToList();
            }
        }

        public List<string> ActiveSeries
        {
            get
            {
                return Series;
            }
        }

        public IEnumerable<IXYDataPoint> this[int index]
        {
            get
            {
                float unity = 100;

                if (_render_volume_only)
                {
                    yield return new PieChartDataPoint(TraceName.Used, _volumes[index].Used);
                    yield return new PieChartDataPoint(TraceName.Free, unity - _volumes[index].Used);

                }
                else
                {
                    yield return new PieChartDataPoint(TraceName.Free, unity - _volumes[index].Used);

                    foreach (string s in _shares.Shares)
                    {
                        if (_shares.Volumes[s]==_volumes[index].Volume)
                        {
                            long u = _shares.Used[s];
                            long sz = _volumes[index].Size;
                            yield return new PieChartDataPoint(s, (float)(Convert.ToDouble(unity) * Convert.ToDouble(u) / Convert.ToDouble(sz)));
                        }
                    }
                }
            }
        }

        public IEnumerable<IXYDataPoint> this[string name]
        {
            get
            {
                return this[_volumes.Volumes.KeyList.IndexOf(name)];
            }
        }

        public long TotalSize(int index)
        {
             return _volumes[index].Size;
        }

        public long TotalSize(string volume)
        {
            return _volumes[volume].Size;
        }
    }
}
    

