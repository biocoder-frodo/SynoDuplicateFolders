using System;
using System.Collections.Generic;
using System.Drawing;

namespace SynoDuplicateFolders.Controls
{
    public interface IChartConfiguration
    {
        IChartLegend this[string key]
        {
            get;
        }

        bool ContainsKey(string key);
        IChartLegend Add(string key, Color k, bool forceKnownColor = false);
        IChartLegend Add(string key, KnownColor k);

        List<ITaggedColor> List { get; }
        void SaveLegendChanges();
        event EventHandler LegendChanged;

    }
}
