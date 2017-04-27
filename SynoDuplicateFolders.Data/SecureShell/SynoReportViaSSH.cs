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
        private readonly string _host;
        private readonly string _username;
        private readonly string _pw;
        private readonly int _port;

        private IDSMVersion _version = null;        

        public SynoReportViaSSH(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _pw = password;            
        }
        public string Host { get { return _host; } }
        internal string UserName { get { return _username; } }
        internal string Password { get { return _pw; } }

        private void RaiseDownloadEvent(CacheStatus status)
        {
            OnDownloadUpdate(this, new SynoReportCacheDownloadEventArgs(status));
        }
        private void RaiseDownloadEvent(CacheStatus status, string message)
        {            
            OnDownloadUpdate(this, new SynoReportCacheDownloadEventArgs(status,message));
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
                    using (SshClient sc = new SshClient(_host, _port, _username, _pw))
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
                SortedDictionary<DateTime, ConsoleFileInfo> dsm_databases = new SortedDictionary<DateTime, ConsoleFileInfo>();
                IConsoleCommand console = null;

                _files.Clear();

                using (SshClient sc = new SshClient(_host, _port, _username, _pw))
                {
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

                                CSVToCategory(SynoReportType.DuplicateCandidates, "duplicate_file.csv", file);
                                CSVToCategory(SynoReportType.FileGroup, "file_group.csv", file);
                                CSVToCategory(SynoReportType.FileOwner, "file_owner.csv", file);
                                CSVToCategory(SynoReportType.LeastModified, "least_modify.csv", file);
                                CSVToCategory(SynoReportType.MostModified, "most_modify.csv", file);
                                CSVToCategory(SynoReportType.LargeFiles, "large_file.csv", file);
                                CSVToCategory(SynoReportType.VolumeUsage, "volume_usage.csv", file);
                                CSVToCategory(SynoReportType.ShareList, "share_list.csv", file);

                            }
                            else
                            {
                                if (fi.FileName.EndsWith("analyzer.db"))
                                {
                                    dsm_databases.Add(fi.Modified, fi);
                                }
                            }
                        }
                    }

                    using (ScpClient cp = new ScpClient(_host, _port, _username, _pw))
                    {
                        cp.Connect();

                        RaiseDownloadEvent(CacheStatus.Downloading, _files.Count, 0);
                        int n = 0;
                        foreach (ICachedReportFile src in _files)
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

                }

                if (KeepAnalyzerDbCount >= 0)
                {
                    List<ConsoleFileInfo> remove = dsm_databases.Values.Take(dsm_databases.Count - KeepAnalyzerDbCount).ToList();
                    console.RemoveFiles(this, remove);
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

    public class SynoReportViaSSHException : Exception
    {
        internal SynoReportViaSSHException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
    public class SynoReportViaSSHLoginFailure : Exception
    {
        internal SynoReportViaSSHLoginFailure(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
