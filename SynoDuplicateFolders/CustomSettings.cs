using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using SynoDuplicateFolders.Controls;
using DiskStationManager.SecureShell;
using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SynoDuplicateFolders.Properties
{
    internal class CustomSettings : ConfigurationSection, IChartConfiguration
    {
        private readonly List<ITaggedColor> _list = new List<ITaggedColor>();
        private static CustomSettings _default_instance = null;
        private static Func<CustomSettings> _load_method = null;
        private readonly static object _thread = new object();

        public event EventHandler LegendChanged;

        public static void Initialize(Func<CustomSettings> method)
        {
            lock (_thread)
            {
                _load_method = method;
                _default_instance = _load_method();
            }
        }
        public static CustomSettings Profile => _default_instance;
        public void Save()
        {
            lock (_thread)
            {

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
                lock (_thread)
                {
                    return Profile.ChartLegends.Items.TryGet(key) as IChartLegend;
                }
            }
        }

        [ConfigurationProperty("ChartLegends")]
        public ChartLegends ChartLegends => this["ChartLegends"] as ChartLegends;

        //public NamedBasicConfigurationElementMap<ChartLegend> ChartLegends
        //{
        //    get
        //    {
        //        return this["ChartLegends"] as NamedBasicConfigurationElementMap<ChartLegend>;
        //    }
        //}

        public List<ITaggedColor> List
        {
            get
            {
                lock (_thread)
                {
                    _list.Clear();
                    var chartLegends = Profile.ChartLegends;
                    foreach (ITaggedColor l in chartLegends.Items)
                    {
                        _list.Add(l);
                    }
                    return _list;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            lock (_thread)
            {
                return Profile.ChartLegends.Items.ContainsKey(key);
            }
        }

        public IChartLegend Add(string key, Color k, bool forceKnownColor = false)
        {
            lock (_thread)
            {
                return Add(new ChartLegend(key, k, forceKnownColor));
            }
        }

        public IChartLegend Add(string key, KnownColor k)
        {
            lock (_thread)
            {
                return Add(new ChartLegend(key, k));
            }
        }
        private IChartLegend Add(ChartLegend chartLegend)
        {
            var presetPalettes = Profile.ChartLegends.Palettes.Split(';').Select(p => (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), p, true)).ToList();
            var presets = new List<string>();
            foreach (var palette in presetPalettes)
            {
                foreach (var preset in ChartLegend.PaletteMap[palette])
                {
                    presets.Add(ColorTranslator.ToHtml(Color.FromArgb(preset.ToArgb())));
                }
            }

            var rgbMap = new Dictionary<int, Dictionary<string, string>>()
            {
                { 0, new Dictionary<string, string>() },
                { 1, new Dictionary<string, string>() }
            };
            var rgbDupes = new Dictionary<int, Dictionary<string, List<string>>>()
            {
                { 0, new Dictionary<string, List<string>>() },
                { 1, new Dictionary<string, List<string>>() }
            };
            int volume;
            foreach (ChartLegend item in Profile.ChartLegends.Items)
            {
                volume = item.Key.StartsWith("/") ? 1 : 0;

                string colorName = item.ColorName.StartsWith("#")
                    ? item.ColorName
                    : ColorTranslator.ToHtml(Color.FromArgb(item.Color.ToArgb()));

                if (rgbMap[volume].ContainsKey(colorName) == false)
                {
                    rgbMap[volume].Add(colorName, item.Key);
                }
                else
                {
                    if (rgbDupes[volume].ContainsKey(colorName) == false)
                    {
                        rgbDupes[volume].Add(colorName, new List<string>());
                    }
                    rgbDupes[volume][colorName].Add(item.Key);

                }
            }
            string newColor = ColorTranslator.ToHtml(Color.FromArgb(chartLegend.Color.ToArgb()));
            volume = chartLegend.Key.StartsWith("/") ? 1 : 0;

            if (rgbMap[volume].ContainsKey(newColor) != false || rgbDupes[volume].ContainsKey(newColor) != false)
            {
                newColor = presets.FirstOrDefault(p => rgbDupes[volume].ContainsKey(p) == false && rgbMap[volume].ContainsKey(p));
                if (string.IsNullOrWhiteSpace(newColor) == false)
                {
                    chartLegend = new ChartLegend(chartLegend.Key, ColorTranslator.FromHtml(newColor));
                }
            }
            foreach (var v in rgbDupes.Keys)
            {
                foreach (var dupe in rgbDupes[v].Keys)
                {
                    foreach (var key in rgbDupes[v][dupe])
                    {
                        newColor = presets.FirstOrDefault(p => rgbDupes[v].ContainsKey(p) == false && rgbMap[v].ContainsKey(p)==false);
                        if (string.IsNullOrWhiteSpace(newColor) == false)
                        {
                            var wohoo = Profile.ChartLegends.Items.TryGet(key);
                            wohoo.ColorName = newColor;
                        }
                    }
                }
            }

            Profile.ChartLegends.Items.Add(chartLegend);
            return chartLegend;
        }
        public void SaveLegendChanges()
        {
            Save();

            LegendChanged?.Invoke(this, new EventArgs());
        }
    }
}
