using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders.Data
{
    public abstract class BSynoReportCache : ISynoReportCache
    {
        internal readonly Regex _report_ts = new Regex(@"(^.*)/([0-9]{4})-([0-9]{2})-([0-9]{2})_([0-9]{2})-([0-9]{2})-([0-9]{2})/(.*$)");
        internal readonly Regex _report_ts_local = new Regex(@"(^.*)_([0-9]{4})-([0-9]{2})-([0-9]{2})_([0-9]{2})-([0-9]{2})-([0-9]{2})_(.*$)");
        internal readonly Dictionary<string, SynoReportType> filenames_type = new Dictionary<string, SynoReportType>()
        {
            { "duplicate_file.csv", SynoReportType.DuplicateCandidates },
            { "file_group.csv", SynoReportType.FileGroup },
            { "file_owner.csv", SynoReportType.FileOwner },
            { "least_modify.csv", SynoReportType.LeastModified },
            { "most_modify.csv", SynoReportType.MostModified },
            { "large_file.csv", SynoReportType.LargeFiles },
            { "volume_usage.csv", SynoReportType.VolumeUsage },
            { "share_list.csv", SynoReportType.ShareList },
        };
        internal readonly CachedReportFilesDictionary _files = new CachedReportFilesDictionary();
        private readonly SortedList<DateTime, Dictionary<SynoReportType, ICachedReportFile>> _allreports = new SortedList<DateTime, Dictionary<SynoReportType, ICachedReportFile>>();

        private string _path = Environment.CurrentDirectory;
        public event SynoReportCacheDownloadEventHandler DownloadUpdate;
        protected void OnDownloadUpdate(object sender, SynoReportCacheDownloadEventArgs e)
        {
            DownloadUpdate?.Invoke(sender, e);
        }
        internal BSynoReportCache()
        {
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (new DirectoryInfo(value).Exists == false)
                {
                    Directory.CreateDirectory(value);
                }
                _path = value;
            }
        }
        public abstract void DownloadCSVFiles();

        public ISynoCSVReport GetReport(DateTime ts, SynoReportType type)
        {
            ISynoCSVReport result = null;
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            if (_allreports.ContainsKey(ts))
            {
                if (_allreports[ts].ContainsKey(type))
                {
                    result = GetReport(_allreports[ts][type]);
                }
            }
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
            return result;
        }

        public ISynoCSVReportPair GetReport(DateTime ts, SynoReportType first, SynoReportType second)
        {
            ISynoCSVReportPair result = null;
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            if (_allreports.ContainsKey(ts))
            {
                if (_allreports[ts].ContainsKey(first) && _allreports[ts].ContainsKey(second))
                {
                    switch ((1 + (int)first) * (1 + (int)second))
                    {
                        case ((1 + (int)SynoReportType.ShareList) * (1 + (int)SynoReportType.VolumeUsage)):
                            {
                                result = new SynoReportVolumePieData(GetReport(ts, first), GetReport(ts, second));
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
            return result;
        }

        public IList<ICachedReportFile> GetReports(SynoReportType type)
        {
            return _files.Values.Where(r => r.Type.Equals(type)).OrderByDescending(r => r.LocalFile.LastWriteTimeUtc).ToList();
        }
        public ISynoCSVReport GetReport(ICachedReportFile file)
        {
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            ISynoCSVReport report = null;
            switch (file.Type)
            {
                case SynoReportType.DuplicateCandidates:
                    report = SynoCSVReader<SynoReportDuplicateCandidates>.LoadReport(file.LocalFile);
                    break;

                case SynoReportType.VolumeUsage:
                    report = SynoCSVReader<SynoReportVolumeUsageValues>.LoadReport(file.LocalFile);
                    break;

                case SynoReportType.ShareList:
                    report = SynoCSVReader<SynoReportSharesValues>.LoadReport(file.LocalFile);
                    break;

                case SynoReportType.LargeFiles:
                case SynoReportType.LeastModified:
                case SynoReportType.MostModified:
                    report = SynoCSVReader<SynoReportFileDetails>.LoadReport(file.LocalFile);
                    break;

                case SynoReportType.FileGroup:
                    report = SynoCSVReader<SynoReportGroups>.LoadReport(file.LocalFile);
                    break;

                case SynoReportType.FileOwner:
                    report = SynoCSVReader<SynoReportOwners>.LoadReport(file.LocalFile);
                    break;

                default:
                    {
                        report = SynoCSVReader<SynoReportContents>.LoadReport(file.LocalFile);
                        break;
                    }
            }
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
            return report;
        }
        public ISynoCSVReport GetReport(SynoReportType type)
        {
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            ISynoCSVReport report = null;
            switch (type)
            {
                case SynoReportType.DuplicateCandidates:
                    report = SynoCSVReader<SynoReportDuplicateCandidates>.LoadReport(GetReports(type).First().LocalFile);
                    break;

                case SynoReportType.VolumeUsage:
                    report = SynoCSVReader<SynoReportVolumeUsage, SynoReportVolumeUsageValues>.LoadReport(GetReports(type));
                    break;

                case SynoReportType.ShareList:
                    report = SynoCSVReader<SynoReportShares, SynoReportSharesValues>.LoadReport(GetReports(type));
                    break;

                case SynoReportType.LargeFiles:
                case SynoReportType.LeastModified:
                case SynoReportType.MostModified:
                    report = SynoCSVReader<SynoReportContentTimeLine, SynoReportFileDetails>.LoadReport(GetReports(type));
                    break;

                case SynoReportType.FileGroup:
                    report = SynoCSVReader<SynoReportContentTimeLine, SynoReportGroups>.LoadReport(GetReports(type));
                    break;

                case SynoReportType.FileOwner:
                    report = SynoCSVReader<SynoReportContentTimeLine, SynoReportOwners>.LoadReport(GetReports(type));
                    break;

                default:
                    report = SynoCSVReader<SynoReportContentTimeLine, SynoReportContents>.LoadReport(GetReports(type));
                    break;
            }
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
            return report;
        }

        internal bool ParseTimeStamp(ConsoleFileInfo file, out DateTime ts)
        {
            string preTs;
            string postTs;
            return ParseTimeStamp(file.Path, false, out ts, out preTs, out postTs);
        }
        private bool ParseTimeStamp(string fileName, bool localFile, out DateTime ts, out string preTs, out string postTs)
        {
            ts = default(DateTime);
            preTs = string.Empty;
            postTs = string.Empty;

            Match m = localFile ? _report_ts_local.Match(fileName) : _report_ts.Match(fileName);

            if (m.Success)
            {
                ts = new DateTime(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value),
                    int.Parse(m.Groups[5].Value), int.Parse(m.Groups[6].Value), int.Parse(m.Groups[7].Value), DateTimeKind.Local);
                preTs = m.Groups[1].Value;
                postTs = m.Groups[8].Value;
                return true;
            }
            return false;
        }

        internal void CSVToCategory(string filename)
        {
            DateTime ts;
            string preTs;
            string postTs;

            foreach (string match in filenames_type.Keys)
            {
                if (filename.Contains(match))
                {
                    CachedReportFile rf = new CachedReportFile(filename, filenames_type[match], Path);

                    _files.Add(rf);

                    if (ParseTimeStamp(filename, false, out ts, out preTs, out postTs))
                    {
                        if (_allreports.ContainsKey(ts) == false) _allreports.Add(ts, new Dictionary<SynoReportType, ICachedReportFile>());
                        _allreports[ts].Add(filenames_type[match], rf);
                    }

                    break;
                }
            }
        }
        internal void CSVToCategory(FileInfo local)
        {
            DateTime ts;
            string preTs;
            string postTs;
            foreach (string match in filenames_type.Keys)
            {
                if (local.FullName.Contains(match))
                {
                    if (ParseTimeStamp(local.FullName, true, out ts, out preTs, out postTs))
                    {
                        CachedReportFile rf = new CachedReportFile(local, filenames_type[match], Path, preTs, postTs, match, ts);

                        if (_files.ContainsKey(rf.LocalFile.FullName) == false)
                        {
                            _files.Add(rf);

                            if (_allreports.ContainsKey(ts) == false) _allreports.Add(ts, new Dictionary<SynoReportType, ICachedReportFile>());
                            if (_allreports[ts].ContainsKey(filenames_type[match]) == false)
                                _allreports[ts].Add(filenames_type[match], rf);
                        }
                    }

                    break;
                }
            }
        }

        public IList<DateTime> DateRange
        {
            get
            {
                return _allreports.Keys.ToList();
            }
        }

        public int KeepAnalyzerDbCount { get; set; }

        public void ScanCachedReports()
        {
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));
            foreach (FileInfo local in new DirectoryInfo(Path).GetFiles())
            {
                CSVToCategory(local);

            }
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }
    }

    internal class CachedReportFilesDictionary : Dictionary<string, ICachedReportFile>
    {
        private new void Add(string key, ICachedReportFile item)
        {
            throw new NotSupportedException();
        }
        public void Add(ICachedReportFile item)
        {
            base.Add(item.LocalFile.FullName, item);
        }

    }
}
