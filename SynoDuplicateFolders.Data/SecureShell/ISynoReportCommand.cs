using System;
using System.Collections.Generic;
using System.Text;
using DiskStationManager.SecureShell;
using Renci.SshNet;

namespace SynoDuplicateFolders.Data.SecureShell
{
    interface ISynoReportCommand : IConsoleCommand
    {
        List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool disconnect = true);
        void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);
    }
}
