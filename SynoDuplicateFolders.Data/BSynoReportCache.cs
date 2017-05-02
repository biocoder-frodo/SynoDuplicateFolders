using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders.Data
{
    public abstract class BSynoReportCache : ISynoReportCache
    {
        internal readonly Regex _report_ts = new Regex("^.*/([0-9]{4})-([0-9]{2})-([0-9]{2})_([0-9]{2})-([0-9]{2})-([0-9]{2})/.*$");

        internal readonly List<ICachedReportFile> _files = new List<ICachedReportFile>();
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
            return _files.Where(r => r.Type.Equals(type)).OrderByDescending(r => r.LocalFile.LastWriteTimeUtc).ToList();
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

        internal bool ParseReportFolderTimeStamp(ConsoleFileInfo file, out DateTime ts)
        {
            ts = default(DateTime);
            if (_report_ts.IsMatch(file.Path))
            {
                Match m = _report_ts.Match(file.Path);
                ts = new DateTime(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), int.Parse(m.Groups[6].Value), DateTimeKind.Local);
                return true;
            }
            return false;
        }
    
        internal void CSVToCategory(SynoReportType t, string match, string filename)
        {
            if (filename.Contains(match))
            {
                CachedReportFile rf = new CachedReportFile(filename, t, Path);

                _files.Add(rf);

                if (_report_ts.IsMatch(filename))
                {
                    Match m = _report_ts.Match(filename);
                    DateTime ts = new DateTime(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value),
                        int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), int.Parse(m.Groups[6].Value), DateTimeKind.Local);

                    if (_allreports.ContainsKey(ts) == false) _allreports.Add(ts, new Dictionary<SynoReportType, ICachedReportFile>());
                    _allreports[ts].Add(t, rf);
                }
                rf = null;
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
        
    }
}
