using System;

namespace SynoDuplicateFolders.Controls
{
    public class ItemHiddenEventArgs : EventArgs
    {
        public readonly string Hostname;
        public readonly string Path;
        public readonly bool IsFile;
        public ItemHiddenEventArgs(string host, string path, bool isFile)
        {
            Hostname = host;
            Path = path;
            IsFile = isFile;
        }
    }
}