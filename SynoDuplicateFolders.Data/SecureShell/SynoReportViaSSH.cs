using DiskStationManager.SecureShell;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class SynoReportViaSSH : BSynoReportCache, IDisposable
    {
        private readonly SynoReportSession _session;
        
        public ISecureShellSession Session => _session;
        
        private bool disposedValue;

        public event EventHandler HostKeyChange;

        public SynoReportViaSSH(DSMHost host, IProxySettings proxy = null)
        {
            _session = new SynoReportSession(host, session_HostKeyChange, proxy);
        }

        private void session_HostKeyChange(object sender, EventArgs e)
        {
            HostKeyChange?.Invoke(sender, e);
        }

        // public ConsoleCommandMode RmExecutionMode { get; set; }


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

        private ISynoReportCommand GetConsole(SshClient client)
        {
            RaiseDownloadEvent(CacheStatus.FetchingVersionInfo);

            var console = _session.GetConsole(client);
            var info = console.GetVersionInfo();

            RaiseDownloadEvent(CacheStatus.FetchingVersionInfoCompleted, info.Version);

            return console;
        }

        public override void DownloadCSVFiles()
        {
            try
            {
                var dsm_databases = new SortedDictionary<DateTime, List<ConsoleFileInfo>>();
                ISynoReportCommand console = null;

                _files.Clear();

                _session.ClientExecute((sc) =>
                {
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
                });

                _session.ClientExecute(cp =>
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
                                _session.DownloadFile(cp, SynoReportHome + src.Source, src.LocalFile, out result);
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
                });


                var removal = new List<ConsoleFileInfo>();
                if (KeepAnalyzerDbCount >= 0)
                {
                    List<DateTime> remove = dsm_databases.Keys.Take(dsm_databases.Count - KeepAnalyzerDbCount).ToList();
                    foreach (DateTime r in remove)
                    {
                        removal.AddRange(dsm_databases[r]);
                    }
                    RaiseDownloadEvent(CacheStatus.Cleanup);
                    console.RemoveFiles(this.Session, this.SynoReportHome, removal);
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
