using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportVolumeUsageValues : BSynoCSVReport
    {
        public readonly SynoReportVolumeUsageValueDictionary Volumes = new SynoReportVolumeUsageValueDictionary();

        public SynoReportVolumeUsageValues()
            : base(SynoReportMode.SingleFile)
        {
        }
        public SynoReportVolumeUsageValue this[int index] => Volumes[index];
        public SynoReportVolumeUsageValue this[string volume] => Volumes[volume];

        public bool ContainsKey(string volume)
        {
            return Volumes.ContainsKey(volume);
        }

        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            SimpleCSVReader r = new SimpleCSVReader(src, new char[] { '\t', ',' },
                                                    new List<SimpleCSVReaderColumnNameReplacer>()
                                                    {
                                                        new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "daty to full", "days till full"),
                                                        new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "days to full", "days till full")
                                                    });

            while (r.EndOfStream == false)
            {
                int? daysTillFull = null;
                r.ReadLine();
                var daysText = r.GetValue("days till full");
                if (daysText.Equals("-") == false)
                {
                    if (int.TryParse(daysText, out int days)) daysTillFull = days;
                    if (daysTillFull.HasValue || daysText == "not available")
                    {
                        string pct = r.GetValue("used").Replace("%", "");
                        string volume = r.GetValue("volume");
                        if (volume.Contains(" "))
                        {
                            volume = "/" + volume.Replace(" ", "");
                        }

                        Volumes.Add(new SynoReportVolumeUsageValue(volume,
                            long.Parse(r.GetValue("size")),
                            float.Parse(pct, System.Globalization.CultureInfo.InvariantCulture),
                            daysTillFull));
                    }
                    else
                    {
                        Console.WriteLine("Unable to parse 'days till full' for {0} - volume has been removed?", fi.Name);
                    }
                }


            }
            src.Close();

            if (Volumes.Count > 1)
            {
                var total = Volumes.Sum(v => v.Value.Size);
                var used = Volumes.Select(v => Convert.ToInt64(((Convert.ToDecimal(v.Value.Used) / 100m) * Convert.ToDecimal(v.Value.Size)))).Sum(sum => sum);
                var usage = Convert.ToSingle(100m * (Convert.ToDecimal(used) / Convert.ToDecimal(total)));
                var daysTillFull = Volumes.Min(v => v.Value.DaysTillFull);
                var s = new SynoReportVolumeUsageValue("/volumes", total, usage, daysTillFull);
                Volumes.Add(s);
               
            }
        }
    }
}