using System;
using System.Collections.Generic;
using System.Configuration;
using SynoDuplicateFolders.Configuration;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class DSMProxy : ConfigurationElement, IElementProvider, IProxySettings
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

        [ConfigurationProperty("port", IsRequired = true, DefaultValue = 8080)]
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
        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get
            {
                return this["password"] as string;
            }
            set
            {
                this["passwword"] = value;
            }
        }
        [ConfigurationProperty("type", IsRequired = false)]
        public string ProxyType
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }

        string IElementProvider.GetElementName()
        {
            return "Proxy";
        }

        object IElementProvider.GetElementKey()
        {
            return Host;
        }
    }
}
