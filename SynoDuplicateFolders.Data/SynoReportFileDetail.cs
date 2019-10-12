using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Data.ComponentModel;

namespace SynoDuplicateFolders.Data
{
    public class FileDetail : ISynoReportFileDetail
    {
        private readonly string _share;
        private readonly string _path;
        private readonly string _name;
        private readonly DateTime? _ts;
        private readonly long _size;

        public FileDetail(string share, string path, DateTime? ts, long size)
        {
            _share = share;
            _path = path.Substring(0, path.LastIndexOf('/'));
            _name = path.Substring(path.LastIndexOf('/') + 1); ;
            _ts = ts;
            _size = size;
        }

        public string Share { get { return _share; } }
        public string Path { get { return _path; } }
        public string Name { get { return _name; } }
        public DateTime? LastModified { get { return _ts; } }
        public long Size { get { return _size; } }
    }
    public class SynoReportFileDetails : BSynoCSVReport, ISynoReportBindingSource<ISynoReportFileDetail>, IDisposable
    {

        private readonly SortableListBindingSource<ISynoReportFileDetail> _files = new SortableListBindingSource<ISynoReportFileDetail>();

        public SortableListBindingSource<ISynoReportFileDetail> BindingSource { get { return _files; } }

        public SynoReportFileDetails()
                : base(SynoReportMode.SingleFile)
        {
        }

        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            SimpleCSVReader r = new SimpleCSVReader(src, '\t', new List<SimpleCSVReaderColumnNameReplacer>
                        {
                            new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Contains, "size", "size"),
                            new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "shared folder", "share"),
                            new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals, "modified time", "modify time"),
                        });

            while (r.EndOfStream == false)
            {
                r.ReadLine();

                DateTime? ts = null;

                if (r.Columns.ContainsKey("modify time")) ts = DateTime.Parse(r.GetValue("modify time"));
                _files.Add(new FileDetail(r.GetValue("share"), r.GetValue("file"), ts, long.Parse(r.GetValue("size"))));
            }

            src.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_files != null) _files.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SynoReportFileDetails()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
    public class SynoReportGroups : BSynoCSVReport, ISynoReportBindingSource<ISynoReportGroupDetail>, IDisposable
    {
        private class GroupDetail : ISynoReportGroupDetail
        {
            private readonly string _group;
            private readonly string _share;
            private readonly long _size;

            internal GroupDetail(string group, string share, long size)
            {
                _group = group;
                _share = share;
                _size = size;
            }
            public string Group { get { return _group; } }
            public string Share { get { return _share; } }
            public long Size { get { return _size; } }
        }

        private readonly SortableListBindingSource<ISynoReportGroupDetail> _files = new SortableListBindingSource<ISynoReportGroupDetail>();

        public SortableListBindingSource<ISynoReportGroupDetail> BindingSource { get { return _files; } }

        public SynoReportGroups()
                : base(SynoReportMode.SingleFile)
        {
        }

        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            SimpleCSVReader r = new SimpleCSVReader(src, ',');
            while (r.EndOfStream == false)
            {
                r.ReadLine();

                _files.Add(new GroupDetail(r.GetValue("group"), r.GetValue("share"), long.Parse(r.GetValue("size"))));
            }

            src.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_files != null) _files.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SynoReportGroups()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class SynoReportOwners : BSynoCSVReport, ISynoReportBindingSource<ISynoReportOwnerDetail>, IDisposable
    {
        private class OwnerDetail : ISynoReportOwnerDetail
        {
            private readonly string _owner;
            private readonly string _share;
            private readonly long _filecount;
            private readonly long _size;

            internal OwnerDetail(string owner, string share, long filecount, long size)
            {
                _owner = owner;
                _share = share;
                _filecount = filecount;
                _size = size;
            }
            public string Owner { get { return _owner; } }
            public string Share { get { return _share; } }
            public long FileCount { get { return _filecount; } }
            public long Size { get { return _size; } }
        }

        private readonly SortableListBindingSource<ISynoReportOwnerDetail> _files = new SortableListBindingSource<ISynoReportOwnerDetail>();

        public SortableListBindingSource<ISynoReportOwnerDetail> BindingSource { get { return _files; } }

        public SynoReportOwners()
                : base(SynoReportMode.SingleFile)
        {
        }
        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            SimpleCSVReader r = new SimpleCSVReader(src, '\t',
                new List<SimpleCSVReaderColumnNameReplacer>()
                {
                    new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Contains,"size","size"),
                    new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals,"username","owner"),
                    new SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode.Equals,"shared folder","share"),
                }
                );

            while (r.EndOfStream == false)
            {
                r.ReadLine();

                var detail = new OwnerDetail(r.GetValue("owner"), r.GetValue("share"), long.Parse(r.GetValue("file count")), long.Parse(r.GetValue("size")));
                _files.Add(detail);
            }

            src.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_files != null) _files.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SynoReportOwners()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
