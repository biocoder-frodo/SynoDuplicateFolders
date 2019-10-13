using System;
using System.Configuration;
using SynoDuplicateFolders.Configuration;
using SynoDuplicateFolders.Extensions;
using Renci.SshNet;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class DSMAuthenticationKeyFile : ConfigurationElement, IElementProvider
    {
        private readonly WrappedPassword<DSMAuthenticationKeyFile> wrapped;

        public DSMAuthenticationKeyFile()
            : base()
        {
            wrapped = new WrappedPassword<DSMAuthenticationKeyFile>("WrappedPassPhrase", this);
        }

        [ConfigurationProperty("filename", IsRequired = true, IsKey = true)]
        public string FileName
        {
            get
            {
                return this["filename"] as string;
            }
            set
            {
                this["filename"] = value;
            }
        }
        [ConfigurationProperty("usePassPhrase", IsRequired = false, DefaultValue = false)]
        public bool UsePassphrase
        {
            get
            {
                return (bool)this["usePassPhrase"];
            }
            set
            {
                this["usePassPhrase"] = value;
            }
        }

        [ConfigurationProperty("passPhrase", IsRequired = false)]
        internal string WrappedPassPhrase
        {
            get
            {
                return this["passPhrase"] as string;
            }
            set
            {
                this["passPhrase"] = value;
            }
        }
        public string PassPhrase
        {
            internal get { return wrapped.Password; }
            set { wrapped.Password = value; }
        }
        public bool StorePassPhrases { get; set; }
        public Func<string,string> GetPassPhrase { get; set; }
        internal PrivateKeyFile getKeyFile(out bool canceled)
        {
            canceled = false;
            if (UsePassphrase && GetPassPhrase != null)
            {
                string pass = GetPassPhrase(FileName);
                canceled = string.IsNullOrEmpty(pass);

                if (StorePassPhrases)
                {
                    if (WrappedPassPhrase.Length == 0)
                    {
                        PassPhrase = pass;
                    }
                    return new PrivateKeyFile(FileName, PassPhrase);
                }
                else
                {
                    return new PrivateKeyFile(FileName, pass);
                }
            }
            else
            {
                return new PrivateKeyFile(FileName);
            }
        }
        object IElementProvider.GetElementKey()
        {
            return FileName;
        }

        string IElementProvider.GetElementName()
        {
            return "AuthenticationKey";
        }
    }
}

