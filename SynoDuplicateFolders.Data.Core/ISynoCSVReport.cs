using System;
using System.IO;

namespace SynoDuplicateFolders.Data.Core
{
    public interface ISynoCSVReport
    {
        void LoadReport(StreamReader source, FileInfo filename);
        void LoadReport(ISynoCSVReport component);
        DateTime Timestamp { get; }
    }
}
