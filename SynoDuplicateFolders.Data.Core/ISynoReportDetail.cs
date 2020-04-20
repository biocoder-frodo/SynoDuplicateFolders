using System;
using SynoDuplicateFolders.Controls;

namespace SynoDuplicateFolders.Data.Core
{
    public interface ISynoReportDetail { }

    public interface ISynoReportFileDetail : ISynoReportDetail
    {
        string Share { get; }
        string Path { get; }
        string Name { get; }
        long Size { get; }
        DateTime? LastModified { get; }
    }
    public interface ISynoReportOwnerDetail : ISynoReportDetail
    {
        string Owner { get; }
        string Share { get; }
        long FileCount { get; }
        long Size { get; }
    }
    public interface ISynoReportGroupDetail : ISynoReportDetail
    {
        string UserName { get; }
        string Group { get; }
        string Share { get; }
        long Size { get; }
    }

    public interface IDuplicatesHistogramValue : ISynoReportDetail
    {
        long Minimum { get; }
        long Maximum { get; }
        long Count { get; }
        long UniqueSize { get; }
        long TotalSize { get; }
    }
    public interface IDuplicateFileInfo : ISynoReportDetail
    {
        long Group { get; }

        [ColumnWidth(600)]
        string FullPath { get; }

        string Extension { get; }
        long Size { get; }
        DateTime TimeStamp { get; }
    }
}
