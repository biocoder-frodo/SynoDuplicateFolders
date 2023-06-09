using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    public abstract partial class BConsoleCommand : IConsoleCommand
    {
        public abstract List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool Disconnect = true);
        public abstract void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);



        internal void RemoveFile(SshClient session, SynoReportViaSSH connection, ConsoleFileInfo file, ConsoleCommandMode mode)
        {
            RemoveFile(session, connection.Session, connection.SynoReportHome, file, mode);
        }

    }
}
