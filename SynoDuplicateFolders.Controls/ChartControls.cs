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
        public ChartControls(MouseEventHandler mouseEventHandler, EventHandler<ChartPaintEventArgs> postPaintEventHandler, EventHandler<ToolTipEventArgs> toolTipEventHandler = null)
            : base(mouseEventHandler)
        {
            toolTipEvent = toolTipEventHandler;
            postPaintEvent = postPaintEventHandler;
        }
        public new void Add(Chart chart)
        {
            if (toolTipEvent != null) chart.GetToolTipText += toolTipEvent;
            chart.PostPaint += postPaintEvent;
            base.Add(chart);
        }
        public new void Clear()
        {
            foreach (Chart c in this)
            {
                c.MouseClick -= mouseEvent;
                if (toolTipEvent != null) c.GetToolTipText -= toolTipEvent;
                c.PostPaint -= postPaintEvent;
            }
            base.Clear();
        }
    }
}
