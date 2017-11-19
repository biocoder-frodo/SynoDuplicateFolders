using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SynoDuplicateFolders.Data
{
    public enum SynoReportType
    {
        VolumeUsage,
        ShareList,
        FileOwner,
        FileGroup,
        DuplicateCandidates,
        LargeFiles,
        MostModified,
        LeastModified,
        Unknown
    }
    public enum SynoReportMode
    { 
        SingleFile,
        TimeLine
    }
    public interface ISynoReportCache
    {
        event SynoReportCacheDownloadEventHandler DownloadUpdate;

        string Path { get; set; }
        void DownloadCSVFiles();

        void ScanCachedReports();

        ISynoCSVReport GetReport(SynoReportType type);
        ISynoCSVReport GetReport(DateTime ts, SynoReportType type);
        ISynoCSVReportPair GetReport(DateTime ts, SynoReportType first, SynoReportType second);

        ISynoCSVReport GetReport(ICachedReportFile file);
        IList<ICachedReportFile> GetReports(SynoReportType type);
        IList<DateTime> DateRange { get; }

        int KeepAnalyzerDbCount { get; set; }
    }
}
