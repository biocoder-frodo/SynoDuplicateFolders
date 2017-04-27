using System.IO;

namespace SynoDuplicateFolders.Data
{
    public interface ICachedReportFile
    {
        string Source { get; }
        FileInfo LocalFile { get; }
        SynoReportType Type { get; }
    }
}
