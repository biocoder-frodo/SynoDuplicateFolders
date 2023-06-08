using Extensions;
using Renci.SshNet;
using Renci.SshNet.Common;
using SynoDuplicateFolders.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace DiskStationManager.SecureShell
{
    public sealed class SynoReportViaSSH : BSynoReportCache, IDisposable, ISecureShellSession
    {
        private readonly DSMSession _session;
        public ISecureShellSession Session => _session;
        private bool disposedValue;

        public event EventHandler HostKeyChange;
        public void OnHostKeyChange(object sender, EventArgs e)
        {
            HostKeyChange?.Invoke(sender, e);
        }
        public SynoReportViaSSH(DSMHost host, Func<string, string> getPassPhraseMethod, Func<DSMKeyboardInteractiveEventArgs, string> getInteractiveMethod, IProxySettings proxy = null)
        {
            _session = new DSMSession(host, getPassPhraseMethod, getInteractiveMethod, proxy);
        }

        // public ConsoleCommandMode RmExecutionMode { get; set; }

        internal string Password
        {
            get
            {
                foreach (DSMAuthentication a in _session.Host.AuthenticationMethods)
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
                var host = _session.Host;
                string result = DSMHost.SynoReportHomeDefault(host.UserName);
                if (!string.IsNullOrWhiteSpace(host.SynoReportHome))
                {
                    if (host.SynoReportHome.StartsWith("/") && host.SynoReportHome.EndsWith("/"))
                    {
                        result = host.SynoReportHome;
                    }
                }
                return result;
            }
        }
        public string HostName
        {
            get { return _session.ConnectionInfo.Host; }
        }

        internal ConnectionInfo ConnectionInfo { get { return _session.ConnectionInfo; } }

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



        ConnectionInfo ISecureShellSession.ConnectionInfo => throw new NotImplementedException();

        public DSMHost Host => throw new NotImplementedException();

        public Func<string, string> PassPhraseMethod => throw new NotImplementedException();

        public Func<DSMKeyboardInteractiveEventArgs, string> InteractiveMethod => throw new NotImplementedException();

        public IProxySettings Proxy => throw new NotImplementedException();

        public string Version => _session.Version;

        public Func<string> GetPassword => throw new NotImplementedException();

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
            var host = _session.Host;
            if (host.FingerPrint.Length.Equals(0) || !host.FingerPrint.SequenceEqual(e.FingerPrint))
            {

                if (host.FingerPrint.Length.Equals(0))
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the new connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        HostName, GetHostAddress(host.Host, out nslookup), KeyFingerPrint(e), _session.ConnectionInfo.ServerVersion)
                        , "New host key for " + host.Host, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                }
                else
                {
                    trust = MessageBox.Show(
                        string.Format("Do you trust the changed connection with host {0}[{1}]\r\n\r\nKey fingerprint: {2}\r\n\r\nServer version: {3}",
                        HostName, GetHostAddress(host.Host, out nslookup), KeyFingerPrint(e), _session.ConnectionInfo.ServerVersion)
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

        private IConsoleCommand GetConsole(SshClient client)
        {
            RaiseDownloadEvent(CacheStatus.FetchingVersionInfo);

            var console = _session.GetConsole(client);
            var info = console.GetVersionInfo();

            RaiseDownloadEvent(CacheStatus.FetchingVersionInfoCompleted, info.Version);

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
        //private void AuthorizationBannerAction(object sender, AuthenticationBannerEventArgs e)
        //{
        //    _banner = e;
        //}
        //private void AuthenticationPromptAction(object sender, AuthenticationPromptEventArgs e)
        //{
        //    Console.WriteLine("AuthenticationPromptAction");

        //    foreach (var p in e.Prompts)
        //    {
        //        p.Response = _interactiveMethod(new DSMKeyboardInteractiveEventArgs(_banner, e, p));
        //    }
        //}
        public override void DownloadCSVFiles()
        {
            try
            {
                SortedDictionary<DateTime, List<ConsoleFileInfo>> dsm_databases = new SortedDictionary<DateTime, List<ConsoleFileInfo>>();
                IConsoleCommand console = null;

                _files.Clear();

                using (SshClient sc = new SshClient(_session.ConnectionInfo))
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
                using (ScpClient cp = new ScpClient(_session.ConnectionInfo))
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

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _session.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
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
