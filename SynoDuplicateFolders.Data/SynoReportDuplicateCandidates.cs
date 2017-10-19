using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SynoDuplicateFolders.Extensions;

using System.Runtime.CompilerServices;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportDuplicateCandidates : BSynoCSVReport
    {
        private readonly DuplicatesAggregate<long, DuplicateFileInfo> _dupes = new DuplicatesAggregate<long, DuplicateFileInfo>();

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



        public DuplicatesAggregate<long, DuplicateFileInfo> DuplicatesByGroup { get { return _dupes; } }
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
            
            BuildIndexes(_bypath, _byname, _tree );        
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
        private void BuildIndexes(DuplicatesAggregate<string,long> bypath, DuplicatesAggregate<string, long> byname, DuplicatesFolder tree, Func<DuplicateFileInfo, bool> predicate = null)
        {
            bypath.Clear();
            byname.Clear();
            tree.Clear();

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

