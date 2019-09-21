using System;
using System.Globalization;
using SynoDuplicateFolders.Data.Core;
using SynoDuplicateFolders.Extensions;
using System.Linq;
namespace SynoDuplicateFolders.Data
{
    public sealed class DuplicateFileInfo : IDuplicateFileInfo
    {
        private const char tab = '\t';
        private const char pathsep = '/';
        private static readonly char[] pathsepsplit = new char[1] { pathsep };
        private static readonly char[] tabsplit = new char[1] { tab };
        private static readonly CultureInfo _ci = new CultureInfo("en-US");

        public readonly long Group;
        public readonly string Share;
        public readonly string FullPath;
        public readonly long Length;

        private DateTime _ts;

        private string _path;
        private string _fileName;
        private readonly string _extension = string.Empty;

        public string Path
        {
            get
            {
                return _path;
            }
            private set
            {
                _path = value;
            }
        }
        public string FileName
        {
            get
            {
                return _fileName;
            }
            private set
            {
                _fileName = value;
            }
        }
        public string Extension
        {
            get
            {
                if (_extension == null)
                {
                }
                return _extension;
            }
        }

        internal readonly string[] FoldersInPath;

        public DuplicateFileInfo(string synorow)
        {
            string[] columns = new string[5];
            columns = synorow.Split(tabsplit, 5);

            for (int i = 0; i < 5; i++)
            {
                columns[i] = columns[i].RemoveEnclosingCharacter("\"");
            }

            Group = long.Parse(columns[0]);
            Share = columns[1];
            FullPath = columns[2];
            Length = long.Parse(columns[3]);
            _ts = DateTime.ParseExact(columns[4], "yyyy/MM/dd HH:mm:ss", _ci, (DateTimeStyles)((int)DateTimeStyles.AssumeLocal + DateTimeStyles.AllowInnerWhite));

            string[] p = FullPath.Substring(1).Split(pathsep);
            FoldersInPath = p.Take(p.Count() - 1).ToArray();

            int dot = p[p.Count()-1].LastIndexOf('.');
            if (dot > -1)
            {
                _extension = p[p.Count()-1].Substring(dot);
            }

        }
        public DateTime TimeStamp
        {
            get
            {
                return _ts;
            }
        }

        long IDuplicateFileInfo.Group
        {
            get
            {
                return this.Group;
            }
        }

        long IDuplicateFileInfo.Size
        {
            get
            {
                return this.Length;
            }
        }

        string IDuplicateFileInfo.FullPath
        {
            get
            {
                return this.FullPath;
            }
        }

        string IDuplicateFileInfo.Extension
        {
            get
            {
                return this.Extension;
            }
        }


        internal void Parse(DuplicateFileInfo entry)
        {
            if (this != entry)
            {
                string[] folders1 = this.FoldersInPath;
                string[] folders2 = entry.FoldersInPath;
                int p1 = folders1.GetUpperBound(0);
                int p2 = folders2.GetUpperBound(0);
                while (p1 >= 0 && p2 >= 0 && folders1[p1] == folders2[p2])
                {
                    p1--; p2--;
                }
                if (this.Path == null)
                {
                    for (int p = 0; p <= p1; p++)
                    {
                        _path += pathsep + folders1[p];
                    }
                    if (this.Path != null)
                    {
                        _fileName = this.FullPath.Substring(Path.Length);
                    }
                }
                for (int p = 0; p <= p2; p++)
                {
                    entry.Path += pathsep + folders2[p];
                }
                if (entry.Path != null)
                    entry.FileName = entry.FullPath.Substring(entry.Path.Length);

            }
        }
    }
}
