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
            ISynoReportCommand console;

            bool briefly = client.IsConnected == false;

            if (briefly) client.Connect();

            console = BSynoReportCommand.GetDSMConsole(client);

            if (briefly) client.Disconnect();

            _version = console.GetVersionInfo();

            return console;
        }
    }
}
