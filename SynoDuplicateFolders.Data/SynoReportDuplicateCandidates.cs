using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SynoDuplicateFolders.Extensions;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportDuplicateCandidates : BSynoCSVReport
    {
        private readonly DuplicatesAggregate<long, DuplicateFileInfo> _dupes = new DuplicatesAggregate<long, DuplicateFileInfo>();
        private readonly DuplicatesAggregate<string, long> _bypath = new DuplicatesAggregate<string, long>(true);
        private readonly DuplicatesAggregate<string, long> _byname = new DuplicatesAggregate<string, long>(true);

        private readonly List<DuplicateFileInfo> _zero = new List<DuplicateFileInfo>();
        private long _unique = -1;
        private long _total = -1;

        public long UniqueSize { get { return _unique; } }
        public long TotalSize { get { return _total; } }

        private readonly DuplicatesFolder _tree = new DuplicatesFolder(string.Empty,null);
       private readonly Dictionary<string, DuplicatesFolder> _toc = new Dictionary<string, DuplicatesFolder>();

        public DuplicatesAggregate<long, DuplicateFileInfo> DuplicatesByGroup { get { return _dupes; } }
        public DuplicatesAggregate<string, long> DuplicatesGroupByName { get { return _byname; } }
        public DuplicatesAggregate<string, long> DuplicatesGroupByPath { get { return _bypath; } }

        public SynoReportDuplicateCandidates() : base(SynoReportMode.SingleFile)
        {
        }

        public DuplicatesFolders Folders
        {
            get
            {
                return _tree.Folders;
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
                }
                else
                {
                    _zero.Add(entry);
                }
            }

            _unique = _dupes.Values.Sum(g => g.First().Length);
           
            foreach (long key in _dupes.Keys)
            {
               // Console.WriteLine("key = " + key);
                foreach (DuplicateFileInfo entry in _dupes[key])
                {
                 //   Console.WriteLine("{0}\t{1}\t{2}", entry.FullPath, entry.Path ?? "", entry.FileName ?? "");

                    if (entry.Path != null)
                    {
                        _bypath.Add(entry.Path, entry.Group);

                        var siblings = _dupes[entry.Group];
                        if (entry.FullPath.Contains("rotbeesten"))
                        {
                            int a = siblings.Count;
                        }
                        DuplicatesFolder f = _tree;
                        string path = string.Empty;
                        for (int n = 0; n < entry.FoldersInPath.Length; n++)
                        {
                            f = f.Folders.addFolder(new DuplicatesFolder(entry.FoldersInPath[n], f));
                            path += "/" + (entry.FoldersInPath[n]);
                            if (_toc.ContainsKey(path) == false)
                            {
                                _toc.Add(path, f);
                            }
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
                        _byname.Add(entry.FileName, entry.Group);
                    }
                    else
                    {
                        //identical file, but no matching path and/or name
                        var siblings = _dupes[key];
                    }
                }

            }

        }

    }
}

