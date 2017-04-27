﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using SynoDuplicateFolders.Extensions;
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
                sw.Close();
            }

        }

        public List<string> Series
        {
            get
            {
                return Shares.Keys.ToList();
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


    public class SynoReportSharesValues : BSynoCSVReport
    {
        public readonly List<string> Shares = new List<string>();
        public readonly Dictionary<string, string> Volumes = new Dictionary<string, string>();
        public readonly Dictionary<string, long> Used = new Dictionary<string, long>();
        public readonly Dictionary<string, long> Quota = new Dictionary<string, long>();

        public SynoReportSharesValues()
            : base(SynoReportMode.SingleFile)
        {
        }

        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            Dictionary<string, int> map = new Dictionary<string, int>();
            string[] columns = src.ReadLine().ToLowerInvariant().Split('\t');
            for (int i = columns.GetLowerBound(0); i <= columns.GetUpperBound(0); i++)
            {
                columns[i] = columns[i].Trim();
                map.Add(columns[i], i);
            }
            while (src.EndOfStream == false)
            {
                columns = src.ReadLine().ToLowerInvariant().Split('\t');
                for (int i = columns.GetLowerBound(0); i <= columns.GetUpperBound(0); i++)
                {
                    columns[i] = columns[i].Trim().RemoveEnclosingCharacter("\"");
                }
                try
                {
                    Shares.Add(columns[map["share"]]);
                    Volumes.Add(columns[map["share"]], "/"+columns[map["share"] + 1].Replace("_",""));
                    Used.Add(columns[map["share"]], long.Parse(columns[2 * map["size"]]));
                    if (columns.Count() > 3)
                    {
                        Quota.Add(columns[map["share"]], long.Parse(columns[2 * map["size"] + 1]));
                        System.Diagnostics.Debug.Assert(Quota[columns[map["share"]]].Equals(0));
                    }
                    else
                    {
                        Quota.Add(columns[map["share"]], 0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Volumes.Add(columns[map["volume"]]);
                //Size.Add(columns[map["volume"]], long.Parse(columns[map["size"]]));
                //Used.Add(columns[map["volume"]], float.Parse(columns[map["used"]].Replace("%", "")));
                //DaysTillFull.Add(columns[map["volume"]], int.Parse(columns[map["days till full"]]));
            }
        }
    }
}