using SynoDuplicateFolders.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    this["paletes"] = value;
                }
            }

        }
    }

