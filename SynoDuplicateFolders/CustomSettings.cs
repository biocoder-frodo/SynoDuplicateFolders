using System.Drawing;
using System.Configuration;
using SynoDuplicateFolders.Configuration;
using System.Collections.Generic;
using SynoDuplicateFolders.Controls;

namespace SynoDuplicateFolders.Properties
{
    internal class CustomSettings : ConfigurationSection, IChartConfiguration
    {
        private static readonly List<ITaggedColor> _pallete = new List<ITaggedColor>();
        private readonly List<ITaggedColor> _list = new List<ITaggedColor>();

        public CustomSettings() : base()
        {
            lock (_pallete)
            {
                if (_pallete.Count == 0)
                {
                    for (KnownColor k = KnownColor.AliceBlue; k < KnownColor.YellowGreen; k++)
                    {
                        _pallete.Add(new ChartLegend(k));
                    }
                }
            }
        }

        [ConfigurationProperty("DSMHosts")]
        public NamedBasicConfigurationElementMap<DSMHost> DSMHosts
        {
            get
            {
                return this["DSMHosts"] as NamedBasicConfigurationElementMap<DSMHost>;
            }
        }

        IChartLegend IChartConfiguration.this[string key]
        {
            get
            {
                return ChartLegends.Items[key] as IChartLegend;
            }
        }

        [ConfigurationProperty("ChartLegends")]
        public NamedBasicConfigurationElementMap<ChartLegend> ChartLegends
        {
            get
            {
                return this["ChartLegends"] as NamedBasicConfigurationElementMap<ChartLegend>;
            }
        }

        public List<ITaggedColor> List
        {
            get
            {
                _list.Clear();
                foreach (ITaggedColor l in ChartLegends.Items)
                {
                    _list.Add(l);
                }
                return _list;
            }
        }

        public List<ITaggedColor> Pallete
        {
            get
            {
                return _pallete;
            }
        }

        public bool ContainsKey(string key)
        {
            return ChartLegends.Items.ContainsKey(key);
        }
        public IChartLegend Add(string key, Color k, bool forceKnownColor = false)
        {
            IChartLegend l = new ChartLegend(key, k, forceKnownColor);
            ChartLegends.Items.Add(l as ChartLegend);
            return l;
        }

        public IChartLegend Add(string key, KnownColor k)
        {
            IChartLegend l = new ChartLegend(key, k);
            ChartLegends.Items.Add(l as ChartLegend);
            return l;
        }
    }
}
