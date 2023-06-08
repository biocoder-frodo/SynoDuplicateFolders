using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Extensions;
using System.Linq;
using System.Net;

namespace DiskStationManager.SecureShell
{
    internal class DSMSession : ISecureShellSession, IDisposable
    {
        public event EventHandler HostKeyChange;

        private readonly DSMHost _host;
        private readonly ConnectionInfo _ci;
        private readonly IProxySettings _proxySettings;
        private IDSMVersion _version = null;
        private Func<DSMKeyboardInteractiveEventArgs, string> _interactiveMethod = null;
        private AuthenticationBannerEventArgs _banner;
        public void OnHostKeyChange(object sender, EventArgs e)
        {
            HostKeyChange?.Invoke(sender, e);
        }
        public DSMSession(DSMHost host, Func<string, string> getPassPhraseMethod, Func<DSMKeyboardInteractiveEventArgs, string> getInteractiveMethod, IProxySettings proxy = null)
        {
            bool canceled = false;

            //RmExecutionMode = ConsoleCommandMode.InteractiveSudo;

            _host = host ?? throw new ArgumentNullException(nameof(host));
            if (getPassPhraseMethod == null) throw new ArgumentNullException(nameof(getPassPhraseMethod));
            if (getInteractiveMethod == null) throw new ArgumentNullException(nameof(getInteractiveMethod));
            _interactiveMethod = getInteractiveMethod;
            _proxySettings = proxy;

            int i = 0;
            AuthenticationMethod[] methods = new AuthenticationMethod[host.AuthenticationSection.Count];
            foreach (var m in host.AuthenticationMethods)
            {
                methods[i++] = m.getAuthenticationMethod(host.UserName, host.StorePassPhrases, getPassPhraseMethod, getInteractiveMethod, out canceled);
            }
            if (!canceled)
            {
                if (proxy != null)
                {
                    ProxyTypes proxypath;
                    if (!Enum.TryParse(proxy.ProxyType, true, out proxypath))
                    {
                        proxypath = ProxyTypes.None;
                    }
                    _ci = new ConnectionInfo(host.Host, host.Port, host.UserName, proxypath, proxy.Host, proxy.Port, proxy.UserName, proxy.Password, methods);
                }
                else
                {
                    _ci = new ConnectionInfo(host.Host, host.Port, host.UserName, methods);
                }

                _ci.AuthenticationBanner += AuthorizationBannerAction;

                foreach (var am in _ci.AuthenticationMethods)
                {
                    KeyboardInteractiveAuthenticationMethod kb = am as KeyboardInteractiveAuthenticationMethod;
                    if (kb != null)
                    {
                        kb.AuthenticationPrompt += AuthenticationPromptAction;
                    }
                }
            }
        }
        private string KeyFingerPrint(HostKeyEventArgs e)
        {
            return e.HostKeyName + " " + e.KeyLength + " " + e.FingerPrint.ToString(':').ToLower();
        }
        private string GetHostAddress(string nameOrAddress, out bool success)
        {
            success = false;
            string address = string.Empty;
            try
            {
                var iplist = Dns.GetHostAddresses(nameOrAddress);

                foreach (var ip in iplist)
                {
                    if (string.IsNullOrEmpty(address))
                    {
                        address = ip.ToString();
                    }
                    else
                    {
                        address += ";" + ip.ToString();
                    }
                }
                success = true;

            }
            catch (Exception)
            {
                address = "0.0.0.0";
            }
            return address;
        }
        private void client_HostKeyReceived(object sender, HostKeyEventArgs e)
        {
            bool nslookup;
            DialogResult trust = DialogResult.Yes; e.CanTrust = false; //we clicked yes if the fingerprint matches
            var host = _host;
            if (host.FingerPrint.Length.Equals(0) || !host.FingerPrint.SequenceEqual(e.FingerPrint))
            {

                if (host.FingerPrint.Length.Equals(0))
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the new connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        _ci.Host, GetHostAddress(host.Host, out nslookup), KeyFingerPrint(e), _ci.ServerVersion)
                        , "New host key for " + host.Host, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                }
                else
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the changed connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        _ci.Host, GetHostAddress(host.Host, out nslookup), KeyFingerPrint(e), _ci.ServerVersion)
                        , "The host key has changed for " + host.Host, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                }
                if (trust == DialogResult.Yes)
                {
                    host.FingerPrint = e.FingerPrint;
                   
                    OnHostKeyChange(this, new EventArgs());
                }
            }
            e.CanTrust = trust.Equals(DialogResult.Yes);
        }

        public string Version
        {
            get
            {
                if (_version == null)
                {
                    using (SshClient sc = new SshClient(_ci))
                    {
                        sc.HostKeyReceived += client_HostKeyReceived;
                        GetConsole(sc);
                        sc.HostKeyReceived -= client_HostKeyReceived;
                    }
                }
                return _version.Version;
            }
        }
        internal IConsoleCommand GetConsole(SshClient client)
        {
            IConsoleCommand console;

            bool briefly = client.IsConnected == false;

           // RaiseDownloadEvent(CacheStatus.FetchingVersionInfo);

            if (briefly) client.Connect();

            console = BConsoleCommand.GetDSMConsole(client);

            if (briefly) client.Disconnect();

            _version = console.GetVersionInfo();

           // RaiseDownloadEvent(CacheStatus.FetchingVersionInfoCompleted, _version.Version);

            return console;
        }
        public ConnectionInfo ConnectionInfo => _ci;

        public DSMHost Host => _host;

     

        public Func<DSMKeyboardInteractiveEventArgs, string> InteractiveMethod => _interactiveMethod;

        public IProxySettings Proxy => _proxySettings;

        public Func<string> GetPassword { get; set; }

        private void AuthorizationBannerAction(object sender, AuthenticationBannerEventArgs e)
        {
            _banner = e;
        }
        private void AuthenticationPromptAction(object sender, AuthenticationPromptEventArgs e)
        {
            Console.WriteLine("AuthenticationPromptAction");

            foreach (var p in e.Prompts)
            {
                p.Response = _interactiveMethod(new DSMKeyboardInteractiveEventArgs(_banner, e, p));
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ci.AuthenticationBanner -= AuthorizationBannerAction;

                    foreach (var am in _ci.AuthenticationMethods)
                    {
                        KeyboardInteractiveAuthenticationMethod kb = am as KeyboardInteractiveAuthenticationMethod;
                        if (kb != null)
                        {
                            kb.AuthenticationPrompt -= AuthenticationPromptAction;
                        }
                    }
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
