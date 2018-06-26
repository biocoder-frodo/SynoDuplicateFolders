using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using System.Data.Linq;

namespace SynoDuplicateFolders.Test
{
    [TestFixture]
    public class TestSynoReports
    {
        #region Input files
        private readonly Dictionary<SynoReportType, List<string>> input = new Dictionary<SynoReportType, List<string>>()
        {
            {
                SynoReportType.DuplicateCandidates, new List<string>() {
                "synoreport_Duplicates_2015-10-31_10-46-27_csv_duplicate_file.csv.zip",
                "duplicate_file.csv",
                Path.Combine("reports-2-0-0-0164","duplicate_file.csv"),
                Path.Combine("reports-2-0-1-0198","duplicate_file.csv")
                }
            },
            {
                SynoReportType.FileGroup, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_file_group.csv.zip",
                "file_group.csv",
                Path.Combine("reports-2-0-0-0164","file_group.csv"),
                Path.Combine("reports-2-0-1-0198","file_group.csv")
                }
            },
            {
                SynoReportType.FileOwner, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_file_owner.csv.zip",
                "file_owner.csv",
                Path.Combine("reports-2-0-0-0164","file_owner.csv"),
                Path.Combine("reports-2-0-1-0198","file_owner.csv")
                }
            },
            {
                SynoReportType.LargeFiles, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_large_file.csv.zip",
                "large_file.csv",
                Path.Combine("reports-2-0-0-0164","large_file.csv"),
                Path.Combine("reports-2-0-1-0198","large_file.csv")
                }
            },
            {
                SynoReportType.LeastModified, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_least_modify.csv.zip",
                "least_modify.csv",
                Path.Combine("reports-2-0-0-0164","least_modify.csv"),
                Path.Combine("reports-2-0-1-0198","least_modify.csv")
                }
            },
            {
                SynoReportType.MostModified, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_most_modify.csv.zip",
                "most_modify.csv",
                Path.Combine("reports-2-0-0-0164","most_modify.csv"),
                Path.Combine("reports-2-0-1-0198","most_modify.csv")
                }
            },
            {
                SynoReportType.ShareList, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_share_list.csv.zip",
                "share_list.csv",
                Path.Combine("reports-2-0-0-0164","share_list.csv"),
                Path.Combine("reports-2-0-1-0198","share_list.csv")
                }
            },
            {
                SynoReportType.VolumeUsage, new List<string>() {
                "synoreport_MyFirstReport_2013-11-17_21-13-16_csv_volume_usage.csv.zip",
                "volume_usage.csv",
                Path.Combine("reports-2-0-0-0164","volume_usage.csv"),
                Path.Combine("reports-2-0-1-0198","volume_usage.csv")
                }
            }

        };
        #endregion

        #region Test loading files from ZIP container and directly
        [Test]
        public void TestDuplicates()
        {
            foreach (var report in TestHelper<SynoReportDuplicateCandidates>.LoadTests(SynoReportType.DuplicateCandidates, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                Assert.AreEqual(1, report.Folders.Count);
                Assert.AreEqual(report.UniqueSize * 2, report.TotalSize);
            }
        }

        [Test]
        public void TestFileGroup()
        {
            foreach (var report in TestHelper<SynoReportGroups>.LoadTests(SynoReportType.FileGroup, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                var rows = report.BindingSource.DataSource as SortableBindingList<ISynoReportGroupDetail>;
                Assert.NotNull(rows, "The binding source should have a datasource instance.");
                if (rows.Count == 0) throw new InconclusiveException("The number of rows should be larger than zero.");
                Assert.AreEqual(0, rows.Count, "The number of rows does not match.");

            }
        }

        [Test]
        public void TestFileOwner()
        {
            foreach (var report in TestHelper<SynoReportOwners>.LoadTests(SynoReportType.FileOwner, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                var rows = report.BindingSource.DataSource as SortableBindingList<ISynoReportOwnerDetail>;
                Assert.NotNull(rows, "The binding source should have a datasource instance.");
                Assert.AreEqual(9, rows.Count, "The number of rows does not match.");
            }
        }

        [Test]
        public void TestLargeFiles()
        {
            foreach (var report in TestHelper<SynoReportFileDetails>.LoadTests(SynoReportType.LargeFiles, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                var rows = report.BindingSource.DataSource as SortableBindingList<ISynoReportFileDetail>;
                Assert.NotNull(rows, "The binding source should have a datasource instance.");
                Assert.AreEqual(4, rows.Count, "The number of rows does not match.");
            }
        }

        [Test]
        public void TestLeastModified()
        {
            foreach (var report in TestHelper<SynoReportFileDetails>.LoadTests(SynoReportType.LeastModified, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                var rows = report.BindingSource.DataSource as SortableBindingList<ISynoReportFileDetail>;
                Assert.NotNull(rows, "The binding source should have a datasource instance.");
                Assert.AreEqual(2, rows.Count, "The number of rows does not match.");
            }
        }
        [Test]
        public void TestMostModified()
        {
            foreach (var report in TestHelper<SynoReportFileDetails>.LoadTests(SynoReportType.MostModified, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                var rows = report.BindingSource.DataSource as SortableBindingList<ISynoReportFileDetail>;
                Assert.NotNull(rows, "The binding source should have a datasource instance.");
                Assert.AreEqual(13, rows.Count, "The number of rows does not match.");
            }
        }
        [Test]
        public void TestShareList()
        {
            foreach (var report in TestHelper<SynoReportSharesValues>.LoadTests(SynoReportType.ShareList, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                Assert.AreEqual(7, report.Shares.Count, "The amount of shares does not match the expected count.");
                Assert.AreEqual(report.Shares.Count, report.Quota.Count, "There should be as many Quota values as there are Shares.");
                Assert.AreEqual(report.Shares.Count, report.Used.Count, "There should be as many Used values as there are Shares.");
                Assert.AreEqual(report.Shares.Count, report.Volumes.Count, "There should be as many Volumes values as there are Shares.");


                foreach (string share in report.Shares)
                {
                    Assert.AreEqual(0, report.Quota[share], "The quota had an unexpected value.");
                    Assert.AreNotEqual(0, report.Used[share], "The usage value had an unexpected value.");
                    Assert.AreNotEqual(0, report.Volumes[share].Length, "The volume name had an unexpected length.");
                }
            }
        }
        [Test]
        public void TestVolumeUsage()
        {
            foreach (var report in TestHelper<SynoReportVolumeUsageValues>.LoadTests(SynoReportType.VolumeUsage, input))
            {
                Assert.NotNull(report, "The report should not be a null reference.");
                Assert.AreEqual(2, report.Volumes.Count, "The number of volumes does not match.");

                foreach (string volume in report.Volumes)
                {
                    Assert.AreNotEqual(0, report.Size[volume], "The Size value had an unexpected value.");
                    Assert.AreEqual(0, report.DaysTillFull[volume], "The DaysTillFull value had an unexpected value.");
                    Assert.AreNotEqual(0, report.Used[volume], "The usage value had an unexpected value.");
                    Assert.AreNotEqual(0, volume.Length, "The volume name had an unexpected length.");
                }
            }
        }
    }

    #endregion

    internal static class TestHelper<T> where T : ISynoCSVReport, new()
    {
        private static FileInfo From(string name)
        {
            return new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\Data", name));
        }
        public static IEnumerable<T> LoadTests(SynoReportType run, Dictionary<SynoReportType, List<string>> tests)
        {
            foreach (string file in tests[run])
            {
                yield return SynoCSVReader<T>.LoadReport(From(file));
            }
        }
    }
}
