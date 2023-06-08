using System;
using System.Collections.Generic;
using System.Configuration;

namespace DiskStationManager.SecureShell
{
    public sealed partial class DSMHost : ConfigurationElement, IElementProvider
    {
        public byte[] FingerPrint
        {
            get
            {
                try
                {
                    return Convert.FromBase64String(Fp);
                }
                catch (FormatException)
                {
                    return new byte[] { };
                }
                catch (ArgumentNullException)
                {
                    return new byte[] { };
                }
            }
            set
            {
                Fp = Convert.ToBase64String(value);
            }
        }

        [ConfigurationProperty("fingerprint", IsRequired = false, DefaultValue = "")]
        private string Fp
        {
            get
            {
                return this["fingerprint"] as string;
            }
            set
            {
                this["fingerprint"] = value;
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

        public static string DefaultUserName
        {
            get
            {
                return "admin";
            }
        }

        [ConfigurationProperty("AuthenticationMethods")]
        public BasicConfigurationElementMap<DSMAuthentication> AuthenticationSection
        {
            get
            {
                return this["AuthenticationMethods"] as BasicConfigurationElementMap<DSMAuthentication>;
            }
        }
        internal IEnumerable<DSMAuthentication> AuthenticationMethods
        {
            get
            {
                foreach (DSMAuthentication am in AuthenticationSection)
                {
                    yield return am;
                }
            }
        }
        public DSMAuthentication GetAuthenticationMethod(DSMAuthenticationMethod method)
        {
            return AuthenticationSection.TryGet(method);
        }

        public DSMAuthentication GetOrAddAuthenticationMethod(DSMAuthenticationMethod method)
        {
            DSMAuthentication am = AuthenticationSection.TryGet(method);
            if (am == null)
            {
                am = new DSMAuthentication(method);
                AuthenticationSection.Add(am);
            }
            return am;
        }
        public DSMAuthentication UpdateAuthenticationMethod(DSMAuthenticationMethod method, bool add)
        {
            if (!add) RemoveAuthenticationMethod(method);
            return add ? GetOrAddAuthenticationMethod(method) : null;
        }
        public void RemoveAuthenticationMethod(DSMAuthenticationMethod method)
        {
            DSMAuthentication am = AuthenticationSection.TryGet(method);
            if (am != null)
            {
                AuthenticationSection.Remove(am);
            }
        }
        public bool HasAuthenticationMethod(DSMAuthenticationMethod method)
        {
            return AuthenticationSection.ContainsKey(method);
        }
        public bool StorePassPhrases { get; set; }
        public DSMProxy Proxy
        {
            get
            {
                if (ProxyBacking.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ProxyBacking[0];
                }
            }
            set
            {
                ProxyBacking.Clear();
                ProxyBacking.Add(value);
            }
        }
        [ConfigurationProperty("Proxy")]
        private BasicConfigurationElementMap<DSMProxy> ProxyBacking
        {
            get
            {
                return this["Proxy"] as BasicConfigurationElementMap<DSMProxy>;
            }
        }

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
        object IElementProvider.GetElementKey()
        {
            return Host;
        }

        string IElementProvider.GetElementName()
        {
            return "DSMHost";
        }
    }
}
