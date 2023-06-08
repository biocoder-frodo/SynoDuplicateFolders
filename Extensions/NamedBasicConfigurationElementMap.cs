using System.Configuration;

namespace System.Configuration
{
    public class NamedBasicConfigurationElementMap<T> : ConfigurationElement where T : ConfigurationElement, IElementProvider, new()
    {
        [ConfigurationProperty("", IsRequired = true, IsKey = true, IsDefaultCollection = true)]
        public BasicConfigurationElementMap<T> Items
        {
            get { return ((BasicConfigurationElementMap<T>)(base[""])); }
            set { base[""] = value; }
        }
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

    }
}
