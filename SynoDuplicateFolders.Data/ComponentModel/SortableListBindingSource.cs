using System.Windows.Forms;
using System.Data.Linq;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.ComponentModel
{   
    public class SortableListBindingSource<T> : BindingSource
        where T : class
    {
        private readonly SortableBindingList<T> _list;
        public SortableListBindingSource(IList<T> list)
            : base()
        {
            _list = new SortableBindingList<T>(list);
            DataSource = _list;
        }
        public SortableListBindingSource()
            :this(new List<T>())
        {
           
        }

        public int Add(T value)
        {
            return base.Add(value);
        }
        protected new int Add(object value)
        {
            return base.Add(value);
        }
    }

    public interface ISynoReportBindingSource<T> where T : class, ISynoReportDetail
    {
        SortableListBindingSource<T> BindingSource { get; }
    }
}
