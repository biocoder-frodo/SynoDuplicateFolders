using System;
using System.Configuration;
using SynoDuplicateFolders.Configuration;
using SynoDuplicateFolders.Extensions;
using Renci.SshNet;
namespace SynoDuplicateFolders.Data.SecureShell
{
    public enum DSMAuthenticationMethod
    {
        None,
        KeyboardInteractive,
        Password,
        PrivateKeyFile
    }
    public class DSMAuthentication : ConfigurationElement, IElementProvider
    {
        private readonly WrappedPassword<DSMAuthentication> wrapped;

        public DSMAuthentication(DSMAuthenticationMethod method)
            : this()
        {
            Method = method;
        }
        public DSMAuthentication()
            : base()
        {
            wrapped = new WrappedPassword<DSMAuthentication>("WrappedPassword", this);
        }

        [ConfigurationProperty("method", IsRequired = true, IsKey = false)]
        public DSMAuthenticationMethod Method
        {
            get
            {
                return (DSMAuthenticationMethod)(this["method"]);
            }
            set
            {
                this["method"] = value;
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
        internal string WrappedPassword
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
        public string Password
        {
            internal get { return wrapped.Password; }
            set { wrapped.Password = value; }
        }
        internal AuthenticationMethod getAuthenticationMethod()
        {
            switch (Method)
            {
                case DSMAuthenticationMethod.KeyboardInteractive:
                    return new KeyboardInteractiveAuthenticationMethod(UserName);
                case DSMAuthenticationMethod.Password:
                    return new PasswordAuthenticationMethod(UserName, Password);
                case DSMAuthenticationMethod.PrivateKeyFile:
                    int i = 0;
                    PrivateKeyFile[] keys = new PrivateKeyFile[AuthenticationKeys.Items.Count];
                    foreach (DSMAuthenticationKeyFile k in AuthenticationKeys.Items)
                    {
                        keys[i++] = k.getKeyFile();
                    }
                    return new PrivateKeyAuthenticationMethod(UserName, keys);
                default:
                    return new NoneAuthenticationMethod(UserName);
            }
        }
        [ConfigurationProperty("AuthenticationKeys")]
        public NamedBasicConfigurationElementMap<DSMAuthenticationKeyFile> AuthenticationKeys
        {
            get
            {
                return this["AuthenticationKeys"] as NamedBasicConfigurationElementMap<DSMAuthenticationKeyFile>;
            }
        }
        object IElementProvider.GetElementKey()
        {
            return Method;
        }

        string IElementProvider.GetElementName()
        {
            return "AuthenticationMethod";
        }
    }
}
