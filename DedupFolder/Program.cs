using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SynoDuplicateFolders.Data.Core.Deduplication;

namespace DedupFolder
{

    class Program
    {

        static void Main(string[] args)
        {
            var paths = new Dictionary<string, DirectoryInfo>();
            var parse = new Queue<string>(args);
            DirectoryInfo folder = null;
            DirectoryInfo keepPath = null;

            while (parse.Count > 0)
            {
                string path = parse.Dequeue();
                if (File.Exists(path))
                {
                    var f = new FileInfo(path);
                    folder = new DirectoryInfo(f.Directory.FullName);
                    paths.Add(folder.FullName, folder);
                    if (keepPath is null) keepPath = folder;
                }
                else
                {
                    if (Directory.Exists(path))
                    {
                        folder = new DirectoryInfo(path);
                        paths.Add(folder.FullName, folder);
                        if (keepPath is null) keepPath = folder;
                    }
                }
            }
            if (paths.Count < 2)
            {
                return;
            }

            DeduplicateFiles(paths.Values.ToList());
        }
    }
}
