using System;
using SynoDuplicateFolders.Extensions;
using SynoDuplicateFolders.Data.SecureShell;
using static SynoDuplicateFolders.Properties.Settings;

namespace SynoDuplicateFolders.Properties
{
    internal class DefaultProxy : IProxySettings
    {
        private readonly WrappedPassword<DefaultProxy> wrapped;

        public DefaultProxy()
        {
            wrapped = new WrappedPassword<DefaultProxy>("WrappedPassword", this);
        }
        public string Host
        {
            get
            {
                return Default.ProxyHost;
            }
            set
            {
                Default.ProxyHost = value;
            }
        }

        public string Password
        {
            get
            {
                return wrapped.Password;
            }
            set
            {
                wrapped.Password = value;
            }
        }
        public string WrappedPassword
        {
            get
            {
                return Default.ProxyPassword;
            }
            set
            {
                Default.ProxyPassword = value;
            }
        }

        public int Port
        {
            get
            {
                return Default.ProxyPort;
            }
            set
            {
                Default.ProxyPort = value;
            }
        }

        public string ProxyType
        {
            get
            {
                return Default.ProxyMethod;
            }
            set
            {
                Default.ProxyMethod = value;
            }
        }

        public string UserName
        {
            get
            {
                return Default.ProxyUser;
            }
            set
            {
                Default.ProxyUser = value;
            }
        }
    }
}
