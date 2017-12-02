using System;

namespace SynoDuplicateFolders.Controls
{
    public class ItemOpenedEventArgs : EventArgs
    {
        public readonly string Path;
        public readonly bool OpenLocation;
        public readonly bool IsFile;
        internal ItemOpenedEventArgs(string path, bool openLocation, bool isFile)
        {
            Path = path;
            OpenLocation = openLocation;
            IsFile = isFile;
        }
    }
}
