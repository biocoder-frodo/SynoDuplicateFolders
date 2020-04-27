using System.Drawing;
using System.Configuration;
using SynoDuplicateFolders.Configuration;
using System.Collections.Generic;
using SynoDuplicateFolders.Controls;
using SynoDuplicateFolders.Data.SecureShell;
using System;

namespace SynoDuplicateFolders.Properties
{
    internal class CustomSettings : ConfigurationSection, IChartConfiguration
    {
        private static readonly List<ITaggedColor> _pallete = new List<ITaggedColor>();
        private readonly List<ITaggedColor> _list = new List<ITaggedColor>();
        private static CustomSettings _default_instance = null;
        private static Func<CustomSettings> _load_method = null;
        private readonly static object _thread = new object();
        private static int _save_count = 0;
        public static void Initialize(Func<CustomSettings> method)
        {
            lock (_thread)
            {
                _load_method = method;
                _default_instance = _load_method();
            }
        }
        public static CustomSettings Profile
        {
            get { return _default_instance; }
        }
        public void Save()
        {   
            lock (_thread)
            {
                ++_save_count;
                _default_instance.CurrentConfiguration.Save();
                Reload();
                Settings.Default.Reload();
            }
        }
        public void Reload()
        {
            lock (_thread)
            {
                ConfigurationManager.RefreshSection(this.SectionInformation.Name);

                _default_instance = null;
                _default_instance = _load_method();
            }
        }
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
                return ChartLegends.Items.TryGet(key) as IChartLegend;
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
