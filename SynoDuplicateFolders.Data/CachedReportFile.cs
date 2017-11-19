using System;
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
        public FileInfo LocalFile { get { return new FileInfo(Path.Combine(_path, _src.Replace("/", "_"))); } }
        public SynoReportType Type { get { return _type; } }

        public CachedReportFile(FileInfo local, SynoReportType type, string path, string preTs, string postTs, string match, DateTime ts)
        {
            if (local.FullName.StartsWith(path) == false)
            {
                throw new ArgumentException("File is not located in configured Path", "local");
            }
            string tmp = postTs.Replace(match, match.Replace("_", ">")).Replace('_', Path.DirectorySeparatorChar).Replace(match.Replace("_", ">"), match).Replace(Path.DirectorySeparatorChar, '/');
            tmp = (preTs.Substring(path.Length).Replace('_', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/') +"/"+ ts.ToString("yyyy-MM-dd_HH-mm-ss") +"/"+ tmp;
            _path = path;
            _src = tmp.Substring(1);
            _type = type;
        }
    }
}
