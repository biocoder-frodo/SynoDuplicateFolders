using System;

namespace SynoDuplicateFolders.Data
{
    public enum CacheStatus
    {
        Idle,
        FetchingVersionInfo,
        FetchingVersionInfoCompleted,
        FetchingDirectoryInfo,
        Downloading,
        Processing,
        Cleanup
    }
    public delegate void SynoReportCacheDownloadEventHandler(object sender, SynoReportCacheDownloadEventArgs e);

    public class SynoReportCacheDownloadEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly CacheStatus Status;
        public readonly int TotalFiles;
        public readonly int FilesFetched;

        public SynoReportCacheDownloadEventArgs()
        {
            Status = CacheStatus.Idle;
        }
        public SynoReportCacheDownloadEventArgs(CacheStatus status)
        {
            Status = status;
        }
        public SynoReportCacheDownloadEventArgs(CacheStatus status, string message)
        {
            Status = status;
            Message = message;
        }
        public SynoReportCacheDownloadEventArgs(CacheStatus status, int totalFiles, int file)
        {
            Status = status;
            TotalFiles = totalFiles;
            FilesFetched = file;
        }
    }
}
