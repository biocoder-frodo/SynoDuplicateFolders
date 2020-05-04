using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SynoDuplicateFolders.Extensions;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class SynoReportViaSSH : BSynoReportCache, IDisposable
    {
        private readonly DSMHost _host;
        private readonly ConnectionInfo _ci;

        private IDSMVersion _version = null;
        private Func<DSMKeyboardInteractiveEventArgs, string> _interactiveMethod = null;
        private AuthenticationBannerEventArgs _banner;

        public event EventHandler HostKeyChange;
        public void OnHostKeyChange(object sender, EventArgs e)
        {
            HostKeyChange?.Invoke(sender, e);
        }
        public SynoReportViaSSH(DSMHost host, Func<string, string> getPassPhraseMethod, Func<DSMKeyboardInteractiveEventArgs, string> getInteractiveMethod, IProxySettings proxy = null)
        {
            bool canceled = false;

            RmExecutionMode = ConsoleCommandMode.InteractiveSudo;

            _host = host ?? throw new ArgumentNullException(nameof(host));
            if (getPassPhraseMethod == null) throw new ArgumentNullException(nameof(getPassPhraseMethod));
            if (getInteractiveMethod == null) throw new ArgumentNullException(nameof(getInteractiveMethod));
            _interactiveMethod = getInteractiveMethod;

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

        public ConsoleCommandMode RmExecutionMode { get; set; }

        internal string Password
        {
            get
            {
                foreach (DSMAuthentication a in _host.AuthenticationMethods)
                {
                    if (a.Method == DSMAuthenticationMethod.Password)
                    {
                        return a.Password;
                    }
                }
                return string.Empty;
            }
        }
        public string SynoReportHome
        {
            get
            {
                string result = DSMHost.SynoReportHomeDefault(_host.UserName);
                if (!string.IsNullOrWhiteSpace(_host.SynoReportHome))
                {
                    if (_host.SynoReportHome.StartsWith("/") && _host.SynoReportHome.EndsWith("/"))
                    {
                        result = _host.SynoReportHome;
                    }
                }
                return result;
            }
        }
        public string Host
        {
            get { return _ci.Host; }
        }

        internal ConnectionInfo ConnectionInfo { get { return _ci; } }

        private void RaiseDownloadEvent(CacheStatus status)
        {
            OnDownloadUpdate(this, new SynoReportCacheDownloadEventArgs(status));
        }
        private void RaiseDownloadEvent(CacheStatus status, string message)
        {
            OnDownloadUpdate(this, new SynoReportCacheDownloadEventArgs(status, message));
        }
        private void RaiseDownloadEvent(CacheStatus status, int totalFiles, int file)
        {
            OnDownloadUpdate(this, new SynoReportCacheDownloadEventArgs(status, totalFiles, file));
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

        private string KeyFingerPrint(HostKeyEventArgs e)
        {
            return e.HostKeyName + " " + e.KeyLength + " " + e.FingerPrint.ToString(':').ToLower();
        }
        private string GetHostAddress(string nameOrAddress, out bool success)
        {
            success = false;
            string address = string.Empty ;
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

            if (_host.FingerPrint.Length.Equals(0) || !_host.FingerPrint.SequenceEqual(e.FingerPrint))
            {

                if (_host.FingerPrint.Length.Equals(0))
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the new connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        _ci.Host, GetHostAddress(_host.Host, out nslookup), KeyFingerPrint(e), _ci.ServerVersion)
                        , "New host key for "+_host.Host, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                }
                else
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the changed connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        _ci.Host, GetHostAddress(_host.Host, out nslookup), KeyFingerPrint(e), _ci.ServerVersion)
                        , "The host key has changed for " + _host.Host, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                }
                if (trust == DialogResult.Yes)
                {
                    _host.FingerPrint = e.FingerPrint;
                    OnHostKeyChange(this, new EventArgs());
                }
            }
            e.CanTrust = trust.Equals(DialogResult.Yes);
        }

        private IConsoleCommand GetConsole(SshClient client)
        {
            IConsoleCommand console;

            bool briefly = client.IsConnected == false;

            RaiseDownloadEvent(CacheStatus.FetchingVersionInfo);

            if (briefly) client.Connect();

            console = BConsoleCommand.GetDSMConsole(client);

            if (briefly) client.Disconnect();

            _version = console.GetVersionInfo();

            RaiseDownloadEvent(CacheStatus.FetchingVersionInfoCompleted, _version.Version);

            return console;
        }
        private bool DownloadFile(ScpClient client, string source, FileInfo localfile)
        {
            try
            {
                if (localfile.Exists == false)
                {
                    client.Download(source, localfile);
                }
                else
                {

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public void AuthorizationBannerAction(object sender, AuthenticationBannerEventArgs e)
        {
            _banner = e;
        }
        public void AuthenticationPromptAction(object sender, AuthenticationPromptEventArgs e)
        {
            Console.WriteLine("AuthenticationPromptAction");

            foreach (var p in e.Prompts)
            {
                p.Response = _interactiveMethod(new DSMKeyboardInteractiveEventArgs(_banner, e, p));
            }
        }
        public override void DownloadCSVFiles()
        {
            try
            {
                SortedDictionary<DateTime, List<ConsoleFileInfo>> dsm_databases = new SortedDictionary<DateTime, List<ConsoleFileInfo>>();
                IConsoleCommand console = null;

                _files.Clear();

                using (SshClient sc = new SshClient(_ci))
                {
                    sc.HostKeyReceived += client_HostKeyReceived;

                    RaiseDownloadEvent(CacheStatus.FetchingDirectoryInfo);

                    sc.Connect();

                    console = GetConsole(sc);

                    List<ConsoleFileInfo> files = console.GetDirectoryContentsRecursive(sc, this);

                    foreach (ConsoleFileInfo fi in files)
                    {

                        string file = fi.Path;

                        if (!file.Contains("/tmp."))
                        {
                            if (file.Contains("/csv/"))
                            {
                                CSVToCategory(file);
                            }
                            else
                            {
                                if (fi.FileName.EndsWith(".db") || fi.FileName.Equals("INFO"))
                                {
                                    DateTime folder;
                                    if (ParseTimeStamp(fi, out folder))
                                    {
                                        if (dsm_databases.ContainsKey(folder) == false)
                                        {
                                            dsm_databases.Add(folder, new List<ConsoleFileInfo>());
                                        }
                                        dsm_databases[folder].Add(fi);
                                    }
                                }
                            }
                        }
                    }
                    sc.HostKeyReceived -= client_HostKeyReceived;
                }
                using (ScpClient cp = new ScpClient(_ci))
                {
                    cp.HostKeyReceived += client_HostKeyReceived;
                    cp.Connect();

                    RaiseDownloadEvent(CacheStatus.Downloading, _files.Count, 0);
                    int n = 0;
                    foreach (ICachedReportFile src in _files.Values)
                    {
                        if (src.Type != SynoReportType.Unknown)
                        {
                            int attempts = 0;
                            bool result = false;

                            while (result == false && attempts < 2)
                            {
                                attempts++;
                                result = DownloadFile(cp, SynoReportHome + src.Source, src.LocalFile);
                            }

                            if (result == false)
                            {
                                cp.Disconnect();
                                cp.Connect();
                            }
                        }

                        RaiseDownloadEvent(CacheStatus.Downloading, _files.Count, ++n);
                    }

                    cp.Disconnect();
                    cp.HostKeyReceived -= client_HostKeyReceived;
                }


                List<ConsoleFileInfo> removal = new List<ConsoleFileInfo>();
                if (KeepAnalyzerDbCount >= 0)
                {
                    List<DateTime> remove = dsm_databases.Keys.Take(dsm_databases.Count - KeepAnalyzerDbCount).ToList();
                    foreach (DateTime r in remove)
                    {
                        removal.AddRange(dsm_databases[r]);
                    }
                    RaiseDownloadEvent(CacheStatus.Cleanup);
                    console.RemoveFiles(this, removal);
                }

            }
            catch (SshAuthenticationException ex)
            {
                throw new SynoReportViaSSHLoginFailure("The login failed.", ex);
            }
            catch (Exception ex)
            {
                throw new SynoReportViaSSHException("An error occured while refreshing the report cache. (" + ex.Message + ")", ex);
            }
            finally
            {
                RaiseDownloadEvent(CacheStatus.Idle);
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
    [Serializable]
    public class SynoReportViaSSHException : Exception
    {
        internal SynoReportViaSSHException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
    [Serializable]
    public class SynoReportViaSSHLoginFailure : Exception
    {
        internal SynoReportViaSSHLoginFailure(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
