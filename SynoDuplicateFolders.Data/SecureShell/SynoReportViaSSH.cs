using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class SynoReportViaSSH : BSynoReportCache
    {
        private readonly DSMHost _host;
        private readonly ConnectionInfo _ci;

        private IDSMVersion _version = null;

        public SynoReportViaSSH(DSMHost host, Func<string, string> getPassPhraseMethod, IProxySettings proxy = null)
        {
            bool canceled = false;

            RmExecutionMode = ConsoleCommandMode.InteractiveSudo;

            _host = host ?? throw new ArgumentNullException(nameof(host));
            if (getPassPhraseMethod == null) throw new ArgumentNullException(nameof(getPassPhraseMethod));

            int i = 0;
            AuthenticationMethod[] methods = new AuthenticationMethod[host.AuthenticationSection.Count];
            foreach (var m in host.AuthenticationMethods)
            {
                methods[i++] = m.getAuthenticationMethod(host.StorePassPhrases, getPassPhraseMethod, out canceled);
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
            }
        }

        public ConsoleCommandMode RmExecutionMode { get; set; }

        internal string Password
        {
            get
            {
                foreach (DSMAuthentication a in _host.AuthenticationMethods)
                {
                    if (a.Method == DSMAuthenticationMethod.Password && _ci.Username.Equals(a.UserName))
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
                    if (_host.SynoReportHome.StartsWith("/") && _host.SynoReportHome.EndsWith("/synoreport/"))
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
                        GetConsole(sc);
                    }
                }
                return _version.Version;
            }
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

        public override void DownloadCSVFiles()
        {
            try
            {
                SortedDictionary<DateTime, List<ConsoleFileInfo>> dsm_databases = new SortedDictionary<DateTime, List<ConsoleFileInfo>>();
                IConsoleCommand console = null;

                _files.Clear();

                using (SshClient sc = new SshClient(_ci))
                {
                    //_ci.AuthenticationBanner += delegate (object sender, AuthenticationBannerEventArgs e)
                    //  {
                    //      Console.WriteLine(e.BannerMessage);
                    //      Console.WriteLine(e.Username);
                    //  };
                    //var kb = _ci.AuthenticationMethods[0] as KeyboardInteractiveAuthenticationMethod;
                    //if (kb!=null)kb.AuthenticationPrompt += delegate(object sender, AuthenticationPromptEventArgs e)
                    //{
                    //    Console.WriteLine(e.Instruction);
                    //    foreach (var p in e.Prompts)
                    //    {
                    //        Console.WriteLine(p.Request);
                    //        p.Response ="1";

                    //    }
                    //};                 
                    RaiseDownloadEvent(CacheStatus.FetchingDirectoryInfo);

                    sc.Connect();

                    console = GetConsole(sc);

                    List<ConsoleFileInfo> files = console.GetDirectoryContentsRecursive(sc);

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
                }
                using (ScpClient cp = new ScpClient(_ci))
                {
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
                                result = DownloadFile(cp, src.Source, src.LocalFile);
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
