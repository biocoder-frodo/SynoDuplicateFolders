using Extensions;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net;

namespace DiskStationManager.SecureShell
{
    internal class DSMSession : ISecureShellSession, IDisposable
    {
        public event EventHandler HostKeyChange;

        private readonly DSMHost _host;
        private readonly ConnectionInfo _ci;
        private readonly IProxySettings _proxySettings;
        protected IDSMVersion _version = null;

        private readonly EventHandler _hostKeyChange;

        public static bool ConsoleUI { get; set; }

        private AuthenticationBannerEventArgs _banner;
        public void OnHostKeyChange(object sender, EventArgs e)
        {
            HostKeyChange?.Invoke(sender, e);
        }
        public DSMSession(DSMHost host, EventHandler hostKeyChange, IProxySettings proxy = null)
        {
            bool canceled = false;

            if (hostKeyChange == null) throw new ArgumentNullException(nameof(hostKeyChange));
            _hostKeyChange = hostKeyChange;
            HostKeyChange += _hostKeyChange;

            //RmExecutionMode = ConsoleCommandMode.InteractiveSudo;

            _host = host ?? throw new ArgumentNullException(nameof(host));

            _proxySettings = proxy;

            int i = 0;
            AuthenticationMethod[] methods = new AuthenticationMethod[host.AuthenticationSection.Count];
            foreach (var m in host.AuthenticationMethods)
            {
                methods[i++] = m.getAuthenticationMethod(host.UserName, host.StorePassPhrases, GetPassPhrase, GetInteractiveMethod, out canceled);
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
        [Obsolete("This method is only present to support DSM versions below 6.x")]
        public void ClientExecuteAsRoot(Action<SshClient> action)
        {
            using (SshClient sc = new SshClient(_host.Host, "root", GetPassword()))
            {
                sc.HostKeyReceived += client_HostKeyReceived;
                action(sc);
                sc.HostKeyReceived -= client_HostKeyReceived;
            }
        }
        public void ClientExecute(Action<SshClient> action)
        {
            using (SshClient sc = new SshClient(_ci))
            {
                sc.HostKeyReceived += client_HostKeyReceived;
                action(sc);
                sc.HostKeyReceived -= client_HostKeyReceived;
            }
        }
        public void ClientExecute(Action<ScpClient> action)
        {
            using (ScpClient sc = new ScpClient(_ci))
            {
                sc.HostKeyReceived += client_HostKeyReceived;
                action(sc);
                sc.HostKeyReceived -= client_HostKeyReceived;
            }
        }

        public string Version
        {
            get
            {
                if (_version == null)
                {
                    ClientExecute(sc => GetConsole(sc));
                }
                return _version.Version;
            }
        }
        internal IConsoleCommand GetConsole(SshClient client)
        {
            IConsoleCommand console;
            bool briefly = client.IsConnected == false;

            if (briefly) client.Connect();
            console = BConsoleCommand.GetDSMConsole(client);
            if (briefly) client.Disconnect();
            _version = console.GetVersionInfo();
            return console;
        }

        public ConnectionInfo ConnectionInfo => _ci;

        public DSMHost Host => _host;



        public IProxySettings Proxy => _proxySettings;

        public Func<string> GetPassword => returnPassword;
        private string returnPassword()
        {
            foreach (DSMAuthentication a in _host.AuthenticationMethods)
            {
                if (a.Method == DSMAuthenticationMethod.Password)
                {
                    return a.Password;
                }
            }
            return GetInteractiveMethod();
            return string.Empty;
        }
        private void AuthorizationBannerAction(object sender, AuthenticationBannerEventArgs e)
        {
            _banner = e;
        }
        private void AuthenticationPromptAction(object sender, AuthenticationPromptEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AuthenticationPromptAction");

            foreach (var p in e.Prompts)
            {
                p.Response = GetInteractiveMethod(new DSMKeyboardInteractiveEventArgs(_banner, e, p));
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

                    HostKeyChange -= _hostKeyChange;
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        private string GetPassPhrase(string fileName)
        {
            string result;
            using (PassPhrase dialog = new PassPhrase(fileName))
            {
                dialog.ShowDialog();
                result = dialog.Password;
            }
            return result;
        }
        private string GetInteractiveMethod(DSMKeyboardInteractiveEventArgs e)
        {
            var banner = new List<string>();
            banner.AddRange(e.Banner.Split('\n'));
            banner.AddRange(e.Instruction.Split('\n'));

            string result;
            using (PassPhrase dialog = new PassPhrase(e.Username, banner.ToArray(), e.Id + ": " + e.Request))
            {
                dialog.ShowDialog();
                result = dialog.Password;
            }
            return result;
        }
        private string GetInteractiveMethod()
        {


            string result;
            using (PassPhrase dialog = new PassPhrase("you", new string[0], "ww"))
            {
                dialog.ShowDialog();
                result = dialog.Password;
            }
            return result;
        }
        public void UploadFile(string destinationPath, string sourcePath)
        {
            UploadFile(destinationPath, new FileInfo(sourcePath));
        }
        public void UploadFile(ScpClient scpClient, string destinationPath, string sourcePath)
        {
            UploadFile(scpClient, destinationPath, new FileInfo(sourcePath));
        }
        public void UploadFile(string destinationPath, FileInfo fileInfo)
        {
            ClientExecute((cp) => UploadFile(cp, new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read), destinationPath));
        }
        public void UploadFile(ScpClient scpClient, string destinationPath, FileInfo fileInfo)
        {
            UploadFile(scpClient, new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read), destinationPath);
        }
        public void UploadFile(string destinationPath, Action<StreamWriter> action)
        {
            using (var ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    action(sw);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    ClientExecute((cp) => UploadFile(cp, ms, destinationPath));
                }
            }
        }
        public void UploadFile(ScpClient scpClient, string destinationPath, Action<StreamWriter> action)
        {
            using (var ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    action(sw);
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);

                    UploadFile(scpClient, ms, destinationPath);
                }
            }
        }
        public void UploadFile(ScpClient scpClient, Stream stream, string destinationPath)
        {
            System.Diagnostics.Debug.WriteLine($"Upload to {destinationPath}");
            bool reOpen = scpClient.IsConnected == false;
            scpClient.RemotePathTransformation = RemotePathTransformation.None;
            if (reOpen) scpClient.Connect();
            scpClient.Upload(stream, destinationPath);
            if (reOpen) scpClient.Disconnect();
        }

        public void DownloadFile(ScpClient client, string source, FileInfo localfile)
        {
            System.Diagnostics.Debug.WriteLine($"Download from {source}");
            if (localfile.Exists == false)
            {
                bool reOpen = client.IsConnected == false;
                client.RemotePathTransformation = RemotePathTransformation.None;
                if (reOpen) client.Connect();
                client.Download(source, localfile);
                if (reOpen) client.Disconnect();
            }
            else
            {
                throw new FileNotFoundException("File already exists", localfile.FullName);
            }
        }
        public void DownloadFile(string source, FileInfo localfile)
        {
            ClientExecute(scp => DownloadFile(scp, source, localfile));
        }
        public void DownloadFile(string source, out MemoryStream stream)
        {
            MemoryStream ms = null;
            ClientExecute(scp =>
            {
                scp.RemotePathTransformation = RemotePathTransformation.None;
                scp.Connect();
                ms = DownloadStream(scp, source);
                scp.Disconnect();
            });
            stream = ms;
        }
        private MemoryStream DownloadStream(ScpClient client, string source)
        {
            var result = new MemoryStream();

            client.Download(source, result);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
        public void DownloadFile(ScpClient client, string source, out MemoryStream stream)
        {
            stream = DownloadStream(client, source);
        }
    }
}
