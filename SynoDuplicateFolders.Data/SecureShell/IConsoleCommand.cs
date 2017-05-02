using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public enum ConsoleCommandMode
    {
        Directly,       
        InteractiveSudo,
        Sudo
    }
    internal interface IConsoleCommand
    {
        IDSMVersion GetVersionInfo(SshClient client);
        IDSMVersion GetVersionInfo();

        List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, bool Disconnect = true);

        void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);
    }
}
