using DiskStationManager.SecureShell;
using Renci.SshNet;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.SecureShell
{
    internal class SynoReportCommandDSM4 : BSynoReportCommand
    {
        public SynoReportCommandDSM4(ConsoleCommandDSM4 consoleCommand) : base(consoleCommand) { }
        public override List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool disconnect = true)
        {
            return GetDirectoryContentsRecursive(client, session.SynoReportHome, "synoreport/", disconnect);
        }

        public override void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases)
        {
            RemoveFiles(session.Session, session.SynoReportHome, dsm_databases);
        }
    }
}
