using DiskStationManager.SecureShell;
using Renci.SshNet;
using System;

namespace SynoDuplicateFolders.Data.SecureShell
{
    class SynoReportSession : DSMSession
    {
        public SynoReportSession(DSMHost host, EventHandler hostKeyChange, IProxySettings proxy = null) : base(host, hostKeyChange, proxy)
        {
        }
        internal new ISynoReportCommand GetConsole(SshClient client)
        {
            ISynoReportCommand console = null;

            EnsureConnection(client, ssh =>
            {
                console = BSynoReportCommand.GetDSMConsole(ssh);
                _version = console.GetVersionInfo();
            });

            return console;
        }
    }
}
