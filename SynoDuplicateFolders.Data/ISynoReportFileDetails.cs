using System;
using SynoDuplicateFolders.Data.ComponentModel;

namespace SynoDuplicateFolders.Data
{

    //public class NullableComparer<T> : IComparable
    //      where T : struct, IComparable<T>
    //{

    //    public int Compare(T? x, T? y)
    //    {
    //        //Compare nulls acording MSDN specification

    //        //Two nulls are equal
    //        if (!x.HasValue && !y.HasValue)
    //            return 0;

    //        //Any object is greater than null
    //        if (x.HasValue && !y.HasValue)
    //            return 1;

    //        if (y.HasValue && !x.HasValue)
    //            return -1;

    //        //Otherwise compare the two values
    //        return x.Value.CompareTo(y.Value);
    //    }

    //    public int CompareTo(object obj)
    //    {
    //        return Compare((T?)this,(T?)obj);
    //    }

    //    public static explicit operator T? (NullableComparer<T> v)
    //    {
    //     return (T?)v.MemberwiseClone();
    //    }
    //}

    public interface ISynoReportBindingSource<T> where T:class, ISynoReportDetail
    {
       SortableListBindingSource<T> BindingSource { get; }
    }
    public interface ISynoReportDetail
    {
      
    }

    public interface ISynoReportFileDetail : ISynoReportDetail
    {
        string Share { get; }
        string Path { get; }
        string Name { get; }
        long Size { get; }
        DateTime? LastModified {get;}
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
        string Group { get; }
        string Share { get; }
        long Size { get; }
    }

}
