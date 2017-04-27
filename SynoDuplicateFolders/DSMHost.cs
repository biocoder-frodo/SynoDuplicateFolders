using System.Configuration;
using SynoDuplicateFolders.Configuration;

namespace SynoDuplicateFolders.Properties
{
    internal class DSMHost : ConfigurationElement, IElementProvider
    {
        [ConfigurationProperty("host", IsRequired = true, IsKey = true)]
        public string Host
        {
            get
            {
                return this["host"] as string;
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 22)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("user", IsRequired = true)]
        public string UserName
        {
            get
            {
                return this["user"] as string;
            }
            set
            {
                this["user"] = value;
            }
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return this["password"] as string;
            }
            set
            {
                this["password"] = value;
            }
        }
        [ConfigurationProperty("home", IsRequired = false)]
        public string SynoReportHome
        {
            get
            {
                return this["home"] as string;
            }
            set
            {
                this["home"] = value;
            }
        }

        public object GetElementKey()
        {
            return Host;
        }

        public string GetElementName()
        {
            return "DSMHost";
        }
    }
}
