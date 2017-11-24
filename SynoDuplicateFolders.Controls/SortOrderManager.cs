using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
namespace SynoDuplicateFolders.Controls
{
    public static class SortOrderManager
    {
        private static readonly Dictionary<Type, CurrentSortOrder> sortOrderGrid = new Dictionary<Type, CurrentSortOrder>();
        public static void SetSortOrder<T>(CurrentSortOrder column) where T: class
        {
            SetSortOrder<T>(column.Column, column.Direction);
        }
        public static void SetSortOrder<T>(string column, ListSortDirection direction) where T : class
        {
            SetSortOrder(typeof(T), column, direction);
        }
        public static void SetSortOrder(Type t, CurrentSortOrder column)
        {
            SetSortOrder(t, column.Column, column.Direction);
        }
        public static void SetSortOrder(Type t, string column, ListSortDirection direction)
        {
            if (column != null)
            {
                if (!sortOrderGrid.ContainsKey(t))
                {
                    sortOrderGrid.Add(t, new CurrentSortOrder());
                }
                sortOrderGrid[t].Column = column;
                sortOrderGrid[t].Direction = direction;
            }
        }

        private static CurrentSortOrder getSortOrder<T>(DataGridView grid) where T : class
        {
            CurrentSortOrder sort;
            if (!sortOrderGrid.ContainsKey(typeof(T)))
            {
                sort = new CurrentSortOrder();
                if (grid.SortOrder != SortOrder.None)
                {
                    sort.Column = grid.SortedColumn.Name;
                    sort.Direction = grid.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                }
                else
                {
                    sort.Column = string.Empty;
                    sort.Direction = ListSortDirection.Ascending;
                }
                sortOrderGrid.Add(typeof(T), sort);
            }
            else
            {
                sort = sortOrderGrid[typeof(T)];
            }
            return sort;
        }
        public static void ApplySortOrder<T>(DataGridView grid) where T : class
        {
            CurrentSortOrder sort = getSortOrder<T>(grid);
            if (!string.IsNullOrEmpty(sort.Column))
            {
                if (grid.Columns.Contains(sort.Column))
                {
                    grid.Sort(grid.Columns[sort.Column], sort.Direction);
                }
            }
        }
    }
    public class CurrentSortOrder
    {
        public string Column;
        public ListSortDirection Direction;
    }
 
}
