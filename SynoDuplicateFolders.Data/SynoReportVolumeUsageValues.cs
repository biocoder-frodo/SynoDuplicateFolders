using System;
using System.Collections.Generic;
using System.IO;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportVolumeUsageValues : BSynoCSVReport
    {
        public readonly List<string> Volumes = new List<string>();
        public readonly Dictionary<string, long> Size = new Dictionary<string, long>();
        public readonly Dictionary<string, float> Used = new Dictionary<string, float>();
        public readonly Dictionary<string, int> DaysTillFull = new Dictionary<string, int>();

        public SynoReportVolumeUsageValues()
            : base(SynoReportMode.SingleFile)
        {
        }

        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            SimpleCSVReader r = new SimpleCSVReader(src, new char[]{ '\t', ','} ,
                                                    new List<SimpleCSVReaderColumnNameReplacer>()
                                                    {
                                                        new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "daty to full", "days till full"),
                                                        new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "days to full", "days till full")
                                                    });

            while (r.EndOfStream == false)
            {
                int days = 0;
                r.ReadLine();

                if (r.GetValue("days till full").Equals("-") == false)
                {
                    if (int.TryParse(r.GetValue("days till full"), out days) == false)
                    {
                        Console.WriteLine("Unable to parse 'days till full' for {0} - volume has been removed?", fi.Name);
                    }
                    else
                    {
                        string pct = r.GetValue("used").Replace("%", "");
                        string volume = r.GetValue("volume");
                        if (volume.Contains(" "))
                        { 
                            volume = "/" + volume.Replace(" ", "");
                        }
                        Volumes.Add(volume);
                        Size.Add(volume, long.Parse(r.GetValue("size")));
                        Used.Add(volume, float.Parse(pct, System.Globalization.CultureInfo.InvariantCulture));
                        DaysTillFull.Add(volume, days);
                    };
                }


            }
            src.Close();            
        }
    }
}