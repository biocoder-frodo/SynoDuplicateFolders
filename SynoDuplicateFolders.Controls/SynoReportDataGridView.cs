using Extensions;
using SynoDuplicateFolders.Data.ComponentModel;
using SynoDuplicateFolders.Data.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using static SynoDuplicateFolders.Controls.SortOrderManager;

namespace SynoDuplicateFolders.Controls
{
    public class SynoReportDataGridView : DataGridView
    {
        private readonly CurrentSortOrder applied_order = new CurrentSortOrder();
        private int fileSizeColumn = -1;
        private Type detailsGridType = null;
        private readonly Dictionary<Type, List<ISynoReportDetail>> previousRowValues = new Dictionary<Type, List<ISynoReportDetail>>();
        private readonly Dictionary<Type, List<PropertyInfo>> stickyColumns = new Dictionary<Type, List<PropertyInfo>>();
        public SynoReportDataGridView()
        {
            base.AllowDrop = false;
            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = false;
            base.AllowUserToResizeColumns = false;
            base.AllowUserToResizeRows = false;
            base.AllowUserToOrderColumns = false;
            
            base.ColumnHeaderMouseClick += SynoReportDataGridView_ColumnHeaderMouseClick;
            base.ColumnHeaderMouseDoubleClick += SynoReportDataGridView_ColumnHeaderMouseDoubleClick;
            base.CellFormatting += SynoReportDataGridView_CellFormatting;
            base.SelectionChanged += SynoReportDataGridView_SelectionChanged;
        }

        private void SynoReportDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.SelectedRows != null && SelectedRows.Count > 0)
            {
                var previousSelection = previousRowValues[detailsGridType];
                previousSelection.Clear();
                foreach (DataGridViewRow row in this.SelectedRows)
                    previousSelection.Add(row.DataBoundItem as ISynoReportDetail);
            }
        }

        private void SynoReportDataGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);
            dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.None;
            SynoReportDataGridView_ColumnHeaderMouseClick(dgv, e);
        }

        private void SynoReportDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            DataGridView dgv = ((DataGridView)sender);

            switch (dgv.SortOrder)
            {
                case SortOrder.Descending:
                    applied_order.Direction = ListSortDirection.Descending;
                    applied_order.Column = dgv.SortedColumn.Name;
                    break;
                case SortOrder.Ascending:
                    applied_order.Direction = ListSortDirection.Ascending;
                    applied_order.Column = dgv.SortedColumn.Name;
                    break;
                default:
                    applied_order.Direction = ListSortDirection.Ascending;
                    applied_order.Column = string.Empty;
                    break;
            }

            SetSortOrder(detailsGridType, applied_order);
        }

        private void SynoReportDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == fileSizeColumn)
            {
                e.Value = ((long)e.Value).ToFileSizeString();
                e.FormattingApplied = true;
            }
            else
            {
                e.FormattingApplied = false;
            }

        }
        public List<T> GetSelection<T>() where T : class, ISynoReportDetail
        {
            if (previousRowValues.ContainsKey(typeof(T)) == false) return new List<T>();
            return previousRowValues[typeof(T)].Select(s => (T)s).ToList();
        }
        public void TryReselection<T>(IReadOnlyList<T> list) where T : class, ISynoReportDetail
        {
            Type t = typeof(T);
            if (stickyColumns.ContainsKey(t) == false)
                stickyColumns.Add(t, t.GetProperties().Where(p => p.GetCustomAttribute<StickyColumnAttribute>() != null).ToList());
            for (int idx = 0; idx < this.Rows.Count; idx++)
            {
                T row = (T)this.Rows[idx].DataBoundItem;
                if (CompareSelection(list, row, stickyColumns[t]))
                    this.SetSelectedRowCore(idx, true);
                else
                    this.SetSelectedRowCore(idx, false);
            }
        }
        private bool CompareSelection<T>(IReadOnlyList<T> list, T row, IReadOnlyList<PropertyInfo> properties) where T : class, ISynoReportDetail
        {
            bool match = false;
            var candidateRowValues = properties.ToList().Select(p => p.GetValue(row, null)).ToArray();
            foreach (var selected in list)
            {
                match = true;
                var selectedValues = properties.ToList().Select(p => p.GetValue(selected, null)).ToArray();
                for (int idx = 0; idx < selectedValues.Length; idx++)
                {
                    if (selectedValues[idx].Equals(candidateRowValues[idx]) == false)
                    {
                        match = false;
                        break;
                    }
                }
                if (match) break;
            }
            return match;
        }
        public void setDataSource<T>(ISynoCSVReport rows) where T : class, ISynoReportDetail
        {
            if (rows != null)
            {
                detailsGridType = typeof(T);
                if (previousRowValues.ContainsKey(detailsGridType) == false) previousRowValues.Add(detailsGridType, new List<ISynoReportDetail>());
                fileSizeColumn = -1;

                Visible = true;

                DataSource = (rows as ISynoReportBindingSource<T>).BindingSource;
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    var a = p.GetCustomAttribute<ColumnWidthAttribute>();
                    if (a != null)
                    {
                        Columns[p.Name].Width = a.Width;
                    }
                }
                ;

                if (Columns.Contains("Size"))
                {
                    fileSizeColumn = Columns["Size"].Index;
                }

                ApplySortOrder<T>(this);
                this.ClearSelection();
            }
            else
            {
                DataSource = null;
            }
        }
    }
}

