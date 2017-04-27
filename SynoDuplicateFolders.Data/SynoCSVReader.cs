using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SynoDuplicateFolders.Data
{
    public static class SynoCSVReader<T> where T : ISynoCSVReport, new()
    {
        public static T LoadReport(FileInfo filename)
        {
            T result = default(T);

            switch (filename.Extension.ToLowerInvariant())
            {
                case ".zip":
                    {
                        using (ZipArchive a = ZipFile.Open(filename.FullName, ZipArchiveMode.Read))
                        {
                            if (a.Entries.Count == 1)
                            {
                                using (StreamReader sr = new StreamReader(a.Entries[0].Open()))
                                {
                                    result = new T();
                                    result.LoadReport(sr, filename);
                                }
                            }
                            else
                            {
                                Console.WriteLine("huh?");                            
                            }
                        }
                        break;
                    }

                case ".csv":
                    {
                        using (StreamReader sr = new StreamReader(filename.FullName))
                        {
                            result = new T();
                            result.LoadReport(sr, filename);
                        }

                        break;
                    }

                default:
                    break;
            }
            return result;
        }
    }
    public static class SynoCSVReader<T,U> where T : ISynoCSVReport, new()
                                    where U : ISynoCSVReport, new()
    {

        public static T LoadReport(IList<ICachedReportFile> list)
        {
            T result = new T();

            foreach (ICachedReportFile file in list)
            {
                U row = SynoCSVReader<U>.LoadReport(file.LocalFile);
                result.LoadReport(row);
            }
            return result;
        }
    }
}
