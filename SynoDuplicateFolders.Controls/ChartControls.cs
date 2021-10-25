using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System;
namespace SynoDuplicateFolders.Controls
{
    internal class ChartControls : GridControls<Chart>
    {
        private readonly EventHandler<ToolTipEventArgs> toolTipEvent;
        public ChartControls(MouseEventHandler mouseEventHandler, EventHandler<ToolTipEventArgs> toolTipEventHandler)
            :base(mouseEventHandler)
        {
            toolTipEvent = toolTipEventHandler;
        }
        public new void Add(Chart chart)
        {            
            chart.GetToolTipText += toolTipEvent;
            base.Add(chart);
        }
        public new void Clear()
        {
            foreach (Chart c in this)
            {
                c.MouseClick -= mouseEvent;
                c.GetToolTipText -= toolTipEvent;
            }
            base.Clear();
        }
    }

    internal class GridControls<T> : List<T> where T : Control
    {
        private readonly List<T> controls = new List<T>();
        protected MouseEventHandler mouseEvent;
        public GridControls(MouseEventHandler mouseEventHandler)
        {
            mouseEvent = mouseEventHandler;
        }
        public new void Add(T control)
        {
            control.MouseClick += mouseEvent;            
            base.Add(control);
        }
        public new void Clear()
        {
            foreach (T c in this)
            {
                c.MouseClick -= mouseEvent;
            }
            base.Clear();
        }
    }
}
