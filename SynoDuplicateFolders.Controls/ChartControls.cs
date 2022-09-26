﻿using System.Windows.Forms;
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
}
