using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System;

namespace SynoDuplicateFolders.Controls
{
#if DESIGNER_WORKAROUND
    public
#else
    internal 
#endif
        class ChartControls : GridControls<Chart>
    {
        private readonly EventHandler<ToolTipEventArgs> toolTipEvent;
        private readonly EventHandler<ChartPaintEventArgs> postPaintEvent;
        public ChartControls(MouseEventHandler mouseEventHandler, EventHandler<ToolTipEventArgs> toolTipEventHandler, EventHandler<ChartPaintEventArgs> postPaintEventHandler)
            :base(mouseEventHandler)
        {
            toolTipEvent = toolTipEventHandler;
            postPaintEvent = postPaintEventHandler;
        }
        public new void Add(Chart chart)
        {            
            chart.GetToolTipText += toolTipEvent;
            chart.PostPaint += postPaintEvent;
            base.Add(chart);
        }
        public new void Clear()
        {
            foreach (Chart c in this)
            {
                c.MouseClick -= mouseEvent;
                c.GetToolTipText -= toolTipEvent;
                c.PostPaint -= postPaintEvent;
            }
            base.Clear();
        }
    }
}
