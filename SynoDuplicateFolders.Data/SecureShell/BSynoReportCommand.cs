using DiskStationManager.SecureShell;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public abstract class BSynoReportCommand : ISynoReportCommand
    {
        private readonly BConsoleCommand bConsole;
        internal BSynoReportCommand(BConsoleCommand consoleCommand)
        {
            bConsole = consoleCommand;
        }
        public abstract List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool disconnect = true);
        public abstract void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);

        public List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, string rootPath, string lsPath = ".", bool disconnect = true)
        {
            return bConsole.GetDirectoryContentsRecursive(client, rootPath, lsPath, disconnect);
        }
        public void RemoveFiles(ISecureShellSession dsm, string rootPath, IList<ConsoleFileInfo> files, string scriptName = null)
        {
            bConsole.RemoveFiles(dsm, rootPath, files, scriptName);
        }
        internal static ISynoReportCommand GetDSMConsole(SshClient client)
        {
            var console = BConsoleCommand.GetDSMConsole(client);

            if (console is ConsoleCommandDSM6)
                return new SynoReportCommandDSM6(console as ConsoleCommandDSM6);
            else
                return new SynoReportCommandDSM4(console as ConsoleCommandDSM4); ;
        }

        public IDSMVersion GetVersionInfo(SshClient client)
        {
            return bConsole.GetVersionInfo(client);
        }

        public IDSMVersion GetVersionInfo()
        {
            return bConsole.GetVersionInfo();
        }
    }
}
