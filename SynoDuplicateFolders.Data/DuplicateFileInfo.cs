using System;
using System.Globalization;
using SynoDuplicateFolders.Extensions;
using System.Linq;
namespace SynoDuplicateFolders.Data
{
    public class DuplicateFileInfo
    {
        private const char tab = '\t';
        private const char pathsep = '/';
        private static readonly char[] pathsepsplit = new char[1] { pathsep };
        private static readonly char[] tabsplit = new char[1] { tab };

        public readonly long Group;
        public readonly string Share;
        public readonly string FullPath;
        public readonly long Length;
        private readonly string _Timestamp;
        public string Path;
        public string FileName;
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
            _Timestamp = columns[4];
            string[] p = FullPath.Substring(1).Split(pathsep);
            FoldersInPath = p.Take(p.Count()-1).ToArray();
        }
        public DateTime TimeStamp {
            get
            {
                DateTime ts = default(DateTime);
                DateTime.TryParseExact(_Timestamp, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), (DateTimeStyles)((int)DateTimeStyles.AssumeLocal + DateTimeStyles.AllowInnerWhite), out ts);
                return ts;
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
                        Path += pathsep + folders1[p];
                    }
                    if (this.Path != null)
                    {
                        FileName = this.FullPath.Substring(Path.Length);
                    }
                }
                for (int p = 0; p <= p2; p++)
                {
                    entry.Path += pathsep + folders2[p];
                }
                if (entry.Path != null)
                    entry.FileName = entry.FullPath.Substring(entry.Path.Length);
                //else
                //{
                //    for (int p = 0; p < FoldersInPath.Length; p++)
                //    {
                //        entry.Path += pathsep + folders1[p];
                //    }
                //    entry.FileName = entry.FullPath.Substring(entry.Path.Length+1);
                //}
            }
        }
    }
}
