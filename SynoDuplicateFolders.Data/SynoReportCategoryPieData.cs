using SynoDuplicateFolders.Data.ComponentModel;
using SynoDuplicateFolders.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportCategoryPieData : SynoCSVReportPair<SynoReportSharesValues, SynoReportGroups>, IVolumePieChart, ISynoChartData
    {
        private readonly SynoReportSharesValues _shares;
        private readonly IList<ISynoReportGroupDetail> _groups;
        private readonly SynoReportVolumeUsageValues _volumes;
        private bool _render_volume_only;
        private string _share;

        public SynoReportCategoryPieData(ISynoCSVReport first, ISynoCSVReport second, ISynoCSVReport volumeReport)
            : base(first, second)
        {
            _shares = First;
            _groups = Second.BindingSource.List as IList<ISynoReportGroupDetail>;
            _volumes = volumeReport as SynoReportVolumeUsageValues;
            _render_volume_only = false;
            ContextTime = Second.Timestamp;
        }

        public string Share

        {
            get => _share;
            set
            {
                _share = value;
            }
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
                return new List<string>() { "By content type" };
            }
        }

        public List<string> ActiveSeries
        {
            get
            {
                return Series;
            }
        }

        public DateTime ContextTime { get; }

        public IEnumerable<IXYDataPoint> this[int index]
        {
            get
            {
                float unity = 100;
                PercentageFreeOnly = TraceName.IsUsage(Share);
                var scope = PercentageFreeOnly ? _groups : _groups.Where(r => r.Share == Share).ToList();
                var grouping = scope.GroupBy(r => r.Group).ToDictionary(g => g.Key, v => v.ToList());
                var totalSize = scope.Sum(r => (decimal)r.Size);
                var totalByGroup =  grouping.Keys.ToDictionary(k => k, v => grouping[v].Sum(r => (decimal)r.Size)).OrderByDescending(kvp=> kvp.Value).ToDictionary(k=>k.Key,v=>v.Value);
                if (_render_volume_only)
                {
                    yield return new PieChartDataPoint(TraceName.Free, unity - _volumes[index].Used);
                    yield return new PieChartDataPoint(TraceName.Used, _volumes[index].Used);

                }
                else
                {
                    //yield return new PieChartDataPoint(TraceName.Free, unity - _volumes[index].Used);

                    foreach (string s in totalByGroup.Keys)
                    {
                    
                            long u = Convert.ToInt64(totalByGroup[s]);
                            
                            yield return new PieChartDataPoint(s, (float)(Convert.ToDouble(unity) * Convert.ToDouble(u) / Convert.ToDouble(totalSize)));
                       
                    }
                }

                //List<ISynoReportGroupDetail> scope;

                //scope = TraceName.IsUsage(e.DataPoint) ? list : list.Where(r => r.Share == e.DataPoint).ToList();



                //System.Diagnostics.Debug.WriteLine($"{e.Timestamp} {e.Series} {e.DataPoint} {e.Text}");


                //var grouping = scope.GroupBy(r => r.Group).ToDictionary(g => g.Key, v => v.ToList());
                //var totalSize = scope.Sum(r => (decimal)r.Size);
                //var totalByGroup = grouping.Keys.ToDictionary(k => k, v => grouping[v].Sum(r => (decimal)r.Size));
                //foreach (var pair in grouping)
                //{
                //    var fileType = pair.Key;

                //    System.Diagnostics.Debug.WriteLine($"{fileType}  {totalByGroup[fileType]} / {totalSize}");

                //}
            }
        }

        public IEnumerable<IXYDataPoint> this[string name] => this[_volumes.Volumes.KeyList.IndexOf(name)];

        public long TotalSize(int index) => _volumes[index].Size;

        public long TotalSize(string volume) => _volumes[volume].Size;
    }
}
