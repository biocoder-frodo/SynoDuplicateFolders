using DiskStationManager.SecureShell;
using SynoDuplicateFolders.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using SynoDuplicateFolders.Data;

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
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Loading from {_default_instance.CurrentConfiguration.FilePath}");
#endif
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

        public List<ITaggedColor> List
        {
            get
            {
                lock (_thread)
                {
                    _list.Clear();
                    foreach (ITaggedColor l in Profile.ChartLegends.Items)
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
            var presets = new List<string>();

            foreach (var palette in Profile.ChartLegends.PresetPalettes)
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
                        newColor = presets.FirstOrDefault(p => rgbDupes[v].ContainsKey(p) == false && rgbMap[v].ContainsKey(p) == false);
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
            lock (_thread)
            {
                var ordered = new SortedDictionary<string, ITaggedColor>(List.ToDictionary(k => k.Key, v => v));

                Profile.ChartLegends.Items.Clear();

                var used = (ChartLegend)ordered[TraceName.Used]; ordered.Remove(TraceName.Used);
                var free = (ChartLegend)ordered[TraceName.Free]; ordered.Remove(TraceName.Free);
                var totalSize = (ChartLegend)ordered[TraceName.TotalSize]; ordered.Remove(TraceName.TotalSize);
                var totalUsed = (ChartLegend)ordered[TraceName.TotalUsed]; ordered.Remove(TraceName.TotalUsed);

                Profile.ChartLegends.Items.Add(free);
                Profile.ChartLegends.Items.Add(used);
                Profile.ChartLegends.Items.Add(totalUsed);
                Profile.ChartLegends.Items.Add(totalSize);

                foreach (ChartLegend legend in ordered.Values)
                    Profile.ChartLegends.Items.Add(legend);
            }

            Save();

            LegendChanged?.Invoke(this, new EventArgs());
        }
    }
}
