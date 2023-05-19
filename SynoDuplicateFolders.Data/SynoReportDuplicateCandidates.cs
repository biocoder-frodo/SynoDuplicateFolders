using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Data.ComponentModel;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

namespace SynoDuplicateFolders.Data
{
    public sealed class SynoReportDuplicateCandidates : BSynoCSVReport, ISynoReportBindingSource<IDuplicateFileInfo>, ISynoReportBindingSource<IDuplicatesHistogramValue>, IDisposable
    {
        private readonly DuplicatesAggregate<long, DuplicateFileInfo> _dupes = new DuplicatesAggregate<long, DuplicateFileInfo>();
        private readonly DuplicatesAggregate<long, DuplicateFileInfo> _dupes_filtered = new DuplicatesAggregate<long, DuplicateFileInfo>();

        private readonly DuplicatesAggregate<string, long> _bypath = new DuplicatesAggregate<string, long>(true);
        private readonly DuplicatesAggregate<string, long> _byname = new DuplicatesAggregate<string, long>(true);
        private readonly DuplicatesFolder _tree = new DuplicatesFolder(string.Empty, null);

        private readonly DuplicatesAggregate<string, long> _bypath_filtered = new DuplicatesAggregate<string, long>(true);
        private readonly DuplicatesAggregate<string, long> _byname_filtered = new DuplicatesAggregate<string, long>(true);
        private readonly DuplicatesFolder _tree_filtered = new DuplicatesFolder(string.Empty, null);

        private bool filtered = false;

        private readonly List<DuplicateFileInfo> _zero = new List<DuplicateFileInfo>();
        private long _unique = -1;
        private long _total = -1;
        private long _largest = 0;

        public long UniqueSize { get { return _unique; } }
        public long TotalSize { get { return _total; } }

        private SortableListBindingSource<IDuplicateFileInfo> _files = null;
        private SortableListBindingSource<IDuplicatesHistogramValue> _histogram = null;

        public DuplicatesAggregate<long, DuplicateFileInfo> DuplicatesByGroup { get { return filtered ? _dupes_filtered : _dupes; } }
        public DuplicatesAggregate<string, long> DuplicatesGroupByName { get { return filtered ? _byname_filtered : _byname; } }
        public DuplicatesAggregate<string, long> DuplicatesGroupByPath { get { return filtered ? _bypath_filtered : _bypath; } }

        public SynoReportDuplicateCandidates() : base(SynoReportMode.SingleFile)
        {
        }

        public DuplicatesFolders Folders
        {
            get
            {
                return filtered ? _tree_filtered.Folders : _tree.Folders;
            }
        }
        public Func<DuplicateFileInfo, bool> Filter
        {
            set
            {
                // reset .BindingSource
                _files = null;

                if (value == null)
                {
                    filtered = false;
                }
                else
                {
                    filtered = true;
                    BuildIndexes(_bypath_filtered, _byname_filtered, _tree_filtered, value);
                }
            }
        }

        SortableListBindingSource<IDuplicatesHistogramValue> ISynoReportBindingSource<IDuplicatesHistogramValue>.BindingSource
        {
            get
            {
                if (_histogram == null)
                {
                    _histogram = new SortableListBindingSource<IDuplicatesHistogramValue>();
                    foreach (var h in Histogram(102400))
                    {
                        _histogram.Add(h);
                    };

                }
                return _histogram;
            }
        }

        SortableListBindingSource<IDuplicateFileInfo> ISynoReportBindingSource<IDuplicateFileInfo>.BindingSource
        {
            get
            {
                if (_files == null)
                {
                    _files = new SortableListBindingSource<IDuplicateFileInfo>();
                    DuplicatesByGroup.Keys.ToList().ForEach(group => DuplicatesByGroup[group].ForEach(dupe => _files.Add(dupe)));

                }
                return _files;
            }
        }


        public override void LoadReport(StreamReader src, FileInfo fi)
        {
            _Timestamp = fi.LastWriteTimeUtc;

            _total = 0;

            string line = src.ReadLine();
            while (src.EndOfStream == false)
            {
                string t = src.ReadLine();
                DuplicateFileInfo entry = new DuplicateFileInfo(t);

                _total += entry.Length;

                if (entry.Length > 0)
                {
                    _dupes.Add(entry.Group, entry);
                    _dupes[entry.Group][0].Parse(entry);

                    if (entry.Length > _largest) _largest = entry.Length;
                }
                else
                {
                    _zero.Add(entry);
                }
            }

            _unique = _dupes.Values.Sum(g => g.First().Length);

            BuildIndexes(_bypath, _byname, _tree);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildIndexes(long key, DuplicatesAggregate<string, long> bypath, DuplicatesAggregate<string, long> byname, DuplicatesFolder tree)
        {
            foreach (DuplicateFileInfo entry in _dupes[key])
            {
                if (entry.Path != null)
                {
                    bypath.Add(entry.Path, entry.Group);

                    DuplicatesFolder f = tree;
                    string path = string.Empty;
                    for (int n = 0; n < entry.FoldersInPath.Length; n++)
                    {
                        f = f.Folders.addFolder(new DuplicatesFolder(entry.FoldersInPath[n], f));
                        path += "/" + (entry.FoldersInPath[n]);
                        //if (toc.ContainsKey(path) == false)
                        //{
                        //    toc.Add(path, f);
                        //}
                        if (path.Equals(entry.Path))
                        {
                            f.Files.Add(entry);

                            break;
                        }
                    }
                }
                else
                {
                    //identical file, but no matching path and/or name
                    var siblings = _dupes[key];

                }
                if (entry.FileName != null)
                {
                    byname.Add(entry.FileName, entry.Group);
                }
                else
                {
                    //identical file, but no matching path and/or name
                    var siblings = _dupes[key];
                }
            }

        }
        private void BuildIndexes(DuplicatesAggregate<string, long> bypath, DuplicatesAggregate<string, long> byname, DuplicatesFolder tree, Func<DuplicateFileInfo, bool> predicate = null)
        {
            bypath.Clear();
            byname.Clear();
            tree.Clear();
            _dupes_filtered.Clear();

            if (predicate == null)
            {
                foreach (long key in _dupes.Keys)
                {
                    BuildIndexes(key, bypath, byname, tree);
                }
            }
            else
            {
                foreach (long key in _dupes.Keys)
                {
                    if (predicate(_dupes[key].First()))
                    {
                        _dupes_filtered.Add(key, _dupes[key]);

                        BuildIndexes(key, bypath, byname, tree);
                    }
                }
            }
        }

        public List<IDuplicatesHistogramValue> Histogram(long bucketsize)
        {

            var query = from y in (from x in _dupes.Values
                                   group x by x.First().Length - (x.First().Length % bucketsize) into x //
                                   select new DuplicatesHistogramValue()
                                   {
                                       Minimum = x.Key,
                                       Maximum = x.Key + bucketsize - 1,
                                       Count = x.Count(),
                                       UniqueSize = x.Sum(d => d.First().Length),
                                       TotalSize = x.Sum(d => d.Sum(f => f.Length)),

                                   } as IDuplicatesHistogramValue)
                        orderby y.Minimum
                        select y;

            return query.ToList();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (_files != null) _files.Dispose();
                    if (_histogram != null) _histogram.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SynoReportDuplicateCandidates()
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

        private static string GetUNCPathUnchecked(string host, string path)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException("The HostName property must be set, it cannot be empty.");
            return string.Format("{0}{1}", Path.DirectorySeparatorChar, Path.DirectorySeparatorChar) + host + path.Replace('/', Path.DirectorySeparatorChar);
        }
        public static FileInfo GetUNCPath(string host, string path, out bool location, out bool file, out bool isFile)
        {
            FileInfo result = null;
            try
            {
                path = RemoveVolumeFromPath(path);

                if (PathCanBeOpened(host, path, out location, out file, out isFile, out result) == false)
                {
                    string[] homes = path.Split('/');
                    if (homes.Length > 2)
                    {
                        if (homes[1] == "homes")
                        {
                            path = "/home" + path.Substring(2 + homes[1].Length + homes[2].Length);
                            PathCanBeOpened(host, path, out location, out file, out isFile, out result);
                        }
                    }
                }

                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", System.DateTime.UtcNow.Ticks, ex.Message));

                location = false;
                file = false;
                isFile = false;
                return null;
            }
        }

        private static bool PathCanBeOpened(string host, string path, out bool openFileLocation, out bool openFile, out bool isFile, out FileInfo result)
        {
            openFileLocation = false;
            openFile = false;
            isFile = false;

            var uncpath = GetUNCPathUnchecked(host, path);

            result = null;
            try
            {
                if (Directory.Exists(uncpath))
                {
                    openFileLocation = false;
                    openFile = true;
                    var test = Directory.GetFiles(uncpath);
                    isFile = false;
                }
                if (File.Exists(uncpath))
                {
                    openFileLocation = true;
                    openFile = true;
                    using (var test = File.OpenRead(uncpath))
                    { }
                    isFile = true;
                }
                result = new FileInfo(uncpath);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", System.DateTime.UtcNow.Ticks, ex.Message));
            }
            return openFile || openFileLocation;
        }

        private static string RemoveVolumeFromPath(string path)
        {
            return path.Substring(path.IndexOf('/'));
        }

    }



    internal class DuplicatesHistogramValue : IDuplicatesHistogramValue
    {
        public long Count
        {
            get; internal set;
        }

        public long Maximum
        {
            get; internal set;

        }

        public long Minimum
        {
            get; internal set;

        }

        public long UniqueSize
        {
            get; internal set;
        }

        public long TotalSize
        {
            get; internal set;

        }
    }
}

