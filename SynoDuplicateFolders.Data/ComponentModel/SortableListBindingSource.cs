﻿using System.Windows.Forms;
using System.Data.Linq;
using System.Collections.Generic;
using System;

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

}