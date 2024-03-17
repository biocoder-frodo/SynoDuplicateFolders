using SynoDuplicateFolders.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace System.Configuration
{
    class ChartLegends : ConfigurationElement
    {
        [ConfigurationProperty("", IsRequired = true, IsKey = true, IsDefaultCollection = true)]
        public BasicConfigurationElementMap<ChartLegend> Items
        {
            get { return ((BasicConfigurationElementMap<ChartLegend>)(base[""])); }
            set { base[""] = value; }
        }
        [ConfigurationProperty("palettes")]
        public string Palettes
        {
            get
            {
                return this["palettes"] as string;
            }
            set
            {
                this["palettes"] = value;
            }
        }

        public List<ChartColorPalette> PresetPalettes
        {
            get
            {
                try
                {
                    return Palettes.Split(';').Select(s => (ChartColorPalette)Enum.Parse(typeof(ChartColorPalette), s, true)).ToList();
                }
                catch
                {
                    return new List<ChartColorPalette>() { ChartColorPalette.Bright, ChartColorPalette.Excel };
                }
            }
            set
            {
                Palettes = string.Join(";", value.Select(p => ((int)p).ToString()));
            }
        }

    }
}

