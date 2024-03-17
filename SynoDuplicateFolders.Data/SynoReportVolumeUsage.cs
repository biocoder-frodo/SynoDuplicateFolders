using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportVolumeUsage : BSynoReportTimeLine, ISynoChartData
    {
        private readonly List<string> _absolute_totals = new List<string>() { "Total Size", "Total Used" };
        public readonly Dictionary<string, string> Volumes = new Dictionary<string, string>();
        private readonly Dictionary<int, string> _volumes = new Dictionary<int, string>();

        public SynoReportVolumeUsage()
            : base()
        {
            _Timestamp = DateTime.UtcNow;
        }

        public override void LoadReport(ISynoCSVReport component)
        {
            base.LoadReport(component);

            var data = component as SynoReportVolumeUsageValues;

            foreach (string volume in data.Volumes.Keys)
            {
                if (Volumes.ContainsKey(volume) == false)
                {
                    Volumes.Add(volume, volume);
                    _volumes.Add(_volumes.Count, volume);
                }
            }
        }

        public void WriteTimeLineData(string file)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write("\t");
                for (int j = 0; j < 3; j++)
                {
                    switch (j)
                    {
                        case 0: sw.Write("Used\t"); break;
                        case 1: sw.Write("Size\t"); break;
                        default: sw.Write("Days till full\t"); break;
                    }
                    for (int i = 0; i < _volumes.Count - 1; i++)
                    {
                        sw.Write("\t");
                    }
                }

                sw.WriteLine();
                sw.Write("TimestampUtc");
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < _volumes.Count; i++)
                    {
                        string v = _volumes[i];

                        sw.Write("\t" + _volumes[i]);
                    }

                sw.WriteLine();


                foreach (DateTime ts in _list.Keys)
                {
                    var data = _list[ts] as SynoReportVolumeUsageValues;

                    sw.Write(ts);

                    for (int i = 0; i < _volumes.Count; i++)
                    {
                        string v = _volumes[i];

                        if (data.ContainsKey(v))
                        {
                            sw.Write("\t" + data[v].Used.ToString());
                        }
                        else
                        {
                            sw.Write("\t");
                        }
                    }

                    for (int i = 0; i < _volumes.Count; i++)
                    {
                        string v = _volumes[i];

                        if (data.ContainsKey(v))
                        {
                            sw.Write("\t" + data[v].Size.ToString());
                        }
                        else
                        {
                            sw.Write("\t");
                        }
                    }

                    for (int i = 0; i < _volumes.Count; i++)
                    {
                        string v = _volumes[i];

                        if (data.ContainsKey(v))
                        {
                            sw.Write("\t" + data[v].DaysTillFull.ToString());
                        }
                        else
                        {
                            sw.Write("\t");
                        }
                    }
                    sw.WriteLine();
                }               
            }

        }

        public List<string> Series
        {
            get
            {
                List<string> result = new List<string>();
                result.AddRange(Volumes.Keys);
                result.AddRange(_absolute_totals);
                return result;
            }
        }

        public List<string> ActiveSeries
        {
            get
            {
                List<string> result = new List<string>();
                result.AddRange((_list[_list.Keys.Max()] as SynoReportVolumeUsageValues).Volumes.Keys);
                result.AddRange(_absolute_totals);
                return result;
            }
        }

        public IEnumerable<IXYDataPoint> this[int index]
        {
            get
            {
                return this[Series[index]];
            }
        }

        public IEnumerable<IXYDataPoint> this[string name]
        {
            get
            {
                foreach (DateTime ts in _list.Keys)
                {
                    var data = _list[ts] as SynoReportVolumeUsageValues;
                    long size = 0;
                    switch (name)
                    {
                        case "Total Size":
                            
                            foreach (string v in Volumes.Keys)
                            {   if (data.ContainsKey(v))
                                size += data[v].Size;
                            }
                            yield return new TimeLineDataPoint<long>(ts, size);
                            break;
                        case "Total Used":

                            foreach (string v in Volumes.Keys)
                            {
                                if (data.ContainsKey(v))
                                    size += Convert.ToInt64(data[v].Used*Convert.ToDouble(data[v].Size)/100);
                            }
                            yield return new TimeLineDataPoint<long>(ts, size);
                            break;
                        default:

                            if (data.ContainsKey(name))
                            {
                                yield return new TimeLineDataPoint<float>(ts, data[name].Used);
                            }
                            break;
                    }
                }
            }
        }
    }
}