using Renci.SshNet;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    internal partial interface IConsoleCommand
    {
        List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool Disconnect = true);
        void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);
    }
}
