using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SynoDuplicateFolders.Data.Core
{
    public class RelativeFileInfo
    {
        public RelativeFileInfo(DirectoryInfo root, FileInfo fileInfo)
        {
            SourcePath = fileInfo;
            RelativePath = fileInfo.FullName.Substring(root.FullName.Length);
        }
        public string RelativePath { get; private set; }
        public FileInfo SourcePath { get; private set; }

        public static Dictionary<string, RelativeFileInfo> GetFiles(DirectoryInfo root)
        {
            var result = new Dictionary<string, RelativeFileInfo>();
            foreach (var file in root.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var ri = new RelativeFileInfo(root, file);
                result.Add(ri.RelativePath, ri);
            }
            return result;
        }
    }
}
