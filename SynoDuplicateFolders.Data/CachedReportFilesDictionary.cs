using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data
{
    internal class CachedReportFilesDictionary : Dictionary<string, ICachedReportFile>
    {
        private new void Add(string key, ICachedReportFile item)
        {
            throw new NotSupportedException();
        }
        public void Add(ICachedReportFile item)
        {
            base.Add(item.LocalFile.FullName, item);
        }

    }
}
