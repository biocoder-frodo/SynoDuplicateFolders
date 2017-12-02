using System;
using System.IO;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public abstract class BSynoCSVReport : ISynoCSVReport
    {
        private readonly SynoReportMode _mode;
        internal DateTime _Timestamp = default(DateTime);

        internal BSynoCSVReport(SynoReportMode mode)
        {
            _mode = mode;
        }
        public virtual void LoadReport(StreamReader source, FileInfo filename)
        {
            throw new NotImplementedException();
        }
        public virtual void LoadReport(ISynoCSVReport component)
        {
            throw new NotImplementedException();
        }

        public DateTime Timestamp
        {
            get { return _Timestamp; }
        }
    }
}
