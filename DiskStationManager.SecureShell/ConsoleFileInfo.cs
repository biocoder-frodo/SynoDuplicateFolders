using System;

namespace DiskStationManager.SecureShell
{
    public class ConsoleFileInfo
    {
        private readonly string _folder;
        private readonly string _filename;
        private readonly DateTime _modified;
        internal ConsoleFileInfo(string folder, string filename, DateTime modified)
        {
            _folder = folder;
            _filename = filename;
            _modified = modified;
        }
        public string Folder { get { return _folder; } }
        public string Path { get { return _folder + "/" + _filename; } }
        public string FileName { get { return _filename; } }
        public DateTime Modified { get { return _modified; } }
    }
}
