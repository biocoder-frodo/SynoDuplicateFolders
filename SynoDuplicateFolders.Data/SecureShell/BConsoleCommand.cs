using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    public abstract partial class BConsoleCommand : IConsoleCommand
    {
        public abstract List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool Disconnect = true);
        public abstract void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);


        internal string RemoveFileCommand(string path)
        {
            return $"rm {path}";
        }
        internal string RemoveFileCommand(string rootPath, ConsoleFileInfo file)
        {

            return rootPath.EndsWith("/") && file.Path.StartsWith("/")
                                                ? RemoveFileCommand(rootPath + file.Path.Substring(1))
                                                : RemoveFileCommand(rootPath + file.Path);
        }
        internal void RemoveFile(SshClient session, SynoReportViaSSH connection, ConsoleFileInfo file, ConsoleCommandMode mode)
        {
            var dsm = connection as ISecureShellSession;
            SudoSession.RunCommand(session, RemoveFileCommand(connection.SynoReportHome, file), mode, dsm.GetPassword);
        }

    }
}
