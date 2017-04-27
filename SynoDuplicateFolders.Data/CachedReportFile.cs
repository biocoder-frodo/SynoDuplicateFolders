using System.IO;

namespace SynoDuplicateFolders.Data
{
    internal class CachedReportFile : ICachedReportFile
    {
        private readonly string _src;
        private readonly SynoReportType _type;
        private readonly string _path;
        public CachedReportFile(string source, SynoReportType type, string path)
        {
            _path = path;
            _src = source.Substring(1);
            _type = type;
        }
        public string Source { get { return _src; } }
        public FileInfo LocalFile { get { return new FileInfo(Path.Combine(_path,_src.Replace("/", "_"))); } }
        public SynoReportType Type { get { return _type; } }
    }
}
