using Renci.SshNet;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
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
        List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, string rootPath, string lsPath = ".", bool disconnect = true);
        void RemoveFiles(ISecureShellSession dsm, string rootPath, IList<ConsoleFileInfo> files, string scriptName = null);
    }
}
