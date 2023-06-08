using Renci.SshNet;
using System;
using System.Configuration;
using System.Security.Cryptography;

namespace DiskStationManager.SecureShell
{
    public enum DSMAuthenticationMethod
    {
        None,
        KeyboardInteractive,
        Password,
        PrivateKeyFile
    }
    public sealed class DSMAuthentication : ConfigurationElement, IElementProvider
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
        internal AuthenticationMethod getAuthenticationMethod(string userName,
                                                                bool storePassPhrases,
                                                                Func<string, string> getPassPhrase,
                                                                Func<DSMKeyboardInteractiveEventArgs, string> getInteractiveMethod,
                                                                out bool canceled)
        {
            canceled = false;

            switch (Method)
            {
                case DSMAuthenticationMethod.KeyboardInteractive:
                    return new KeyboardInteractiveAuthenticationMethod(userName);
                case DSMAuthenticationMethod.Password:
                    return new PasswordAuthenticationMethod(userName, Password);
                case DSMAuthenticationMethod.PrivateKeyFile:
                    int i = 0;
                    PrivateKeyFile[] keys = new PrivateKeyFile[AuthenticationKeys.Items.Count];
                    foreach (DSMAuthenticationKeyFile k in AuthenticationKeys.Items)
                    {
                        bool empty = false;
                        k.GetPassPhrase = getPassPhrase;
                        k.StorePassPhrases = storePassPhrases;
                        keys[i++] = k.GetKeyFile(out empty);
                        if (empty) canceled = true;
                    }
                    return new PrivateKeyAuthenticationMethod(userName, keys);
                default:
                    return new NoneAuthenticationMethod(userName);
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
