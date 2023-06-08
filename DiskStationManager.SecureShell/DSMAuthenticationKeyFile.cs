using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Configuration;
using System.Security.Cryptography;

namespace DiskStationManager.SecureShell
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
        public Func<string, string> GetPassPhrase { get; set; }
        internal PrivateKeyFile GetKeyFile(out bool canceled)
        {
            canceled = false;
            try
            {
                return new PrivateKeyFile(FileName);
            }
            catch (SshPassPhraseNullOrEmptyException)
            {
                if (StorePassPhrases && WrappedPassPhrase.Length > 0)
                {
                    return new PrivateKeyFile(FileName, PassPhrase);
                }

                string pass = GetPassPhrase(FileName);
                canceled = string.IsNullOrEmpty(pass);
                if (canceled == false)
                {
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
               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                canceled = true;
            }
            return null;
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

