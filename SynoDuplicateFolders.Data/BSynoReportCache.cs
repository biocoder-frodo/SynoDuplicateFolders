using DiskStationManager.SecureShell;
using SynoDuplicateFolders.Data.Core;
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
        private R ShowProcessing<R>(Func<R> func) where R : class
        {
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));

            var result = func();

            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
            return result;
        }
        private void ShowProcessing(Action action)
        {
            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Processing));

            action();

            DownloadUpdate?.Invoke(this, new SynoReportCacheDownloadEventArgs(CacheStatus.Idle));
        }

        internal BSynoReportCache()
        {
        }
        public abstract void DownloadCSVFiles();

        public IList<DateTime> DateRange => _allreports.Keys.ToList();

        public int KeepAnalyzerDbCount { get; set; }

        public void ScanCachedReports()
        {
            ShowProcessing(() =>
            {
                foreach (FileInfo local in new DirectoryInfo(Path).GetFiles())
                {
                    CSVToCategory(local);

                }
            });
        }
        public string Path
        {
            get => _path;
            set
            {
                if (new DirectoryInfo(value).Exists == false)
                {
                    Directory.CreateDirectory(value);
                }
                _path = value;
            }
        }

        public ISynoCSVReport GetReport(DateTime ts, SynoReportType type)
        {
            if (_allreports.ContainsKey(ts) && _allreports[ts].ContainsKey(type))
            {
                return ShowProcessing(() => GetReport(_allreports[ts][type]));
            }
            return null;
        }

        public ISynoCSVReportPair GetReport(DateTime ts, SynoReportType first, SynoReportType second)
        {

            if (_allreports.ContainsKey(ts) && _allreports[ts].ContainsKey(first) && _allreports[ts].ContainsKey(second))
            {
                switch ((1 + (int)first) * (1 + (int)second))
                {
                    case ((1 + (int)SynoReportType.ShareList) * (1 + (int)SynoReportType.VolumeUsage)):

                        return ShowProcessing(() => new SynoReportVolumePieData(GetReport(ts, first), GetReport(ts, second)));

                    default:
                        break;
                }
            }

            return null;
        }

        public IList<ICachedReportFile> GetReports(SynoReportType type) => _files.Values
            .Where(r => r.Type.Equals(type))
            .OrderByDescending(r => r.LocalFile.LastWriteTimeUtc)
            .ToList();

        public ISynoCSVReport GetReport(ICachedReportFile file) => ShowProcessing(() => GetReport(file.Type, file.LocalFile));
        public ISynoCSVReport GetReport(SynoReportType type) => ShowProcessing(() => GetReport(type, null));
        private ISynoCSVReport GetReport(SynoReportType type, FileInfo localFile)
        {
            switch (type)
            {
                case SynoReportType.VolumeUsage:
                    if (localFile is null) return SynoCSVReader<SynoReportVolumeUsage, SynoReportVolumeUsageValues>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportVolumeUsageValues>.LoadReport(localFile);

                case SynoReportType.ShareList:
                    if (localFile is null) return SynoCSVReader<SynoReportShares, SynoReportSharesValues>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportSharesValues>.LoadReport(localFile);

                case SynoReportType.FileOwner:
                    if (localFile is null) return SynoCSVReader<SynoReportContentTimeLine, SynoReportOwners>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportOwners>.LoadReport(localFile);

                case SynoReportType.FileGroup:
                    if (localFile is null) SynoCSVReader<SynoReportContentTimeLine, SynoReportGroups>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportGroups>.LoadReport(localFile);

                case SynoReportType.DuplicateCandidates:
                    if (localFile is null) return SynoCSVReader<SynoReportDuplicateCandidates>.LoadReport(GetReports(type).First().LocalFile);
                    return SynoCSVReader<SynoReportDuplicateCandidates>.LoadReport(localFile);

                case SynoReportType.LargeFiles:
                case SynoReportType.MostModified:
                case SynoReportType.LeastModified:
                    if (localFile is null) return SynoCSVReader<SynoReportContentTimeLine, SynoReportFileDetails>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportFileDetails>.LoadReport(localFile);

                default:
                    if (localFile is null) return SynoCSVReader<SynoReportContentTimeLine, SynoReportContents>.LoadReport(GetReports(type));
                    return SynoCSVReader<SynoReportContents>.LoadReport(localFile);
            }
        }

        private bool ParseTimeStamp(string fileName, bool localFile, out DateTime ts, out string preTs, out string postTs)
        {
            ts = default;
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
        internal bool ParseTimeStamp(ConsoleFileInfo file, out DateTime ts) => ParseTimeStamp(file.Path, false, out ts, out string _, out string _);

        internal void CSVToCategory(string filename)
        {
            foreach (string match in filenames_type.Keys)
            {
                if (filename.Contains(match))
                {
                    CachedReportFile rf = new CachedReportFile(filename, filenames_type[match], Path);

                    _files.Add(rf);

                    if (ParseTimeStamp(filename, false, out DateTime ts, out string _, out string _))
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
            foreach (string match in filenames_type.Keys)
            {
                if (local.FullName.Contains(match))
                {
                    if (ParseTimeStamp(local.FullName, true, out DateTime ts, out string preTs, out string postTs))
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
    }
}
