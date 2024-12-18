using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Extensions;

namespace SynoDuplicateFolders.Data
{
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

            if (map.Count.Equals(2))
            {
                map.Add("volume", 1);
                map["size"] = 2;
            }

            if (map.ContainsKey("shared folder")) map.Add("share", map["shared folder"]);
            if (map.ContainsKey("size (including recycle bins)(byte)")) map.Add("size", map["size (including recycle bins)(byte)"]);

            //Console.WriteLine("columns in {0} = {1}", fi.FullName, map.Count);

            while (src.EndOfStream == false)
            {
                columns = src.ReadLine().ToLowerInvariant().Split('\t');
                for (int i = columns.GetLowerBound(0); i <= columns.GetUpperBound(0); i++)
                {
                    columns[i] = columns[i].Trim().RemoveEnclosingCharacter("\"");
                }
                string volume = "/" + columns[map["volume"]].Replace("_", "").Replace(" ", "");
                Shares.Add(columns[map["share"]]);
                Volumes.Add(columns[map["share"]], volume);
                Used.Add(columns[map["share"]], long.Parse(columns[map["size"]]));
                if (map.ContainsKey("quota") == false)
                {
                    if (columns.Count() > 3)
                    {
                        Quota.Add(columns[map["share"]], long.Parse(columns[map["size"] + 1]));
                    }
                    else
                    {
                        Quota.Add(columns[map["share"]], 0);
                    }
                }
                else
                {
                    if (long.TryParse(columns[map["quota"]], out long quota) == false) quota = 0;

                    Quota.Add(columns[map["share"]], quota);
                }

            }
        }
    }
}