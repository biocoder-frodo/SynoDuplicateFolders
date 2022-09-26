using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportShares : BSynoReportTimeLine, ISynoChartData
    {
        public readonly Dictionary<string, string> Shares = new Dictionary<string, string>();
        private readonly Dictionary<int, string> _shares = new Dictionary<int, string>();

        public SynoReportShares()
            : base()
        {
            _Timestamp = DateTime.UtcNow;
        }

        public override void LoadReport(ISynoCSVReport component)
        {
            base.LoadReport(component);

            var data = component as SynoReportSharesValues;

            foreach (string share in data.Shares)
            {
                if (Shares.ContainsKey(share) == false)
                {
                    Shares.Add(share, share);
                    _shares.Add(_shares.Count, share);
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
                        case 0: sw.Write("Size\t"); break;
                        case 1: sw.Write("Quota\t"); break;
                        default: sw.Write("Volume\t"); break;
                    }
                    for (int i = 0; i < _shares.Count - 1; i++)
                    {
                        sw.Write("\t");
                    }
                }

                sw.WriteLine();
                sw.Write("TimestampUtc");
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < _shares.Count; i++)
                    {
                        string v = _shares[i];

                        sw.Write("\t" + _shares[i]);
                    }

                sw.WriteLine();


                foreach (DateTime ts in _list.Keys)
                {
                    var data = _list[ts] as SynoReportSharesValues;

                    sw.Write(ts);

                    for (int i = 0; i < _shares.Count; i++)
                    {
                        string v = _shares[i];

                        if (data.Used.ContainsKey(v))
                        {
                            sw.Write("\t" + data.Used[v].ToString());
                        }
                        else
                        {
                            sw.Write("\t");
                        }
                    }

                    for (int i = 0; i < _shares.Count; i++)
                    {
                        string v = _shares[i];

                        if (data.Quota.ContainsKey(v))
                        {
                            sw.Write("\t" + data.Quota[v].ToString());
                        }
                        else
                        {
                            sw.Write("\t");
                        }
                    }

                    for (int i = 0; i < _shares.Count; i++)
                    {
                        string v = _shares[i];

                        if (data.Volumes.ContainsKey(v))
                        {
                            sw.Write("\t" + data.Volumes[v].ToString());
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
                return Shares.Keys.ToList();
            }
        }

        public List<string> ActiveSeries
        {
            get
            {
                return (_list[_list.Keys.Max()] as SynoReportSharesValues).Shares;                
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
                    var data = _list[ts] as SynoReportSharesValues;

                    if (data.Used.ContainsKey(name))
                    {
                        yield return new TimeLineDataPoint<long>(ts, data.Used[name]);
                    }
                }

            }
        }       
    }
}