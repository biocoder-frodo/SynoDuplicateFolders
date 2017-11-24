using System;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Extensions;
using System.ComponentModel;
using System.Windows.Forms;
using static SynoDuplicateFolders.Controls.SortOrderManager;
using SynoDuplicateFolders.Data.ComponentModel;

namespace SynoDuplicateFolders.Controls
{
    public class SynoReportDataGridView : DataGridView
    {
        private readonly CurrentSortOrder applied_order = new CurrentSortOrder();
        private int fileSizeColumn = -1;
        private Type detailsGridType = null;

        public SynoReportDataGridView()
        {
            base.ColumnHeaderMouseClick += SynoReportDataGridView_ColumnHeaderMouseClick;
            base.ColumnHeaderMouseDoubleClick += SynoReportDataGridView_ColumnHeaderMouseDoubleClick;
            base.CellFormatting += SynoReportDataGridView_CellFormatting;
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
        public void setDataSource<T>(ISynoCSVReport rows) where T : class, ISynoReportDetail
        {
            if (rows != null)
            {
                detailsGridType = typeof(T);
                fileSizeColumn = -1;

                Visible = true;

                DataSource = (rows as ISynoReportBindingSource<T>).BindingSource;

                if (Columns.Contains("Size"))
                {
                    fileSizeColumn = Columns["Size"].Index;
                }

                ApplySortOrder<T>(this);
            }
            else
            {
                DataSource = null;
            }
        }

    }
}

