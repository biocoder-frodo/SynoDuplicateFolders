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

        /// <summary>
        /// Perform ls in a folder that you cd into first.
        /// </summary>
        /// <param name="client">Pass in an open SshClient instance</param>
        /// <param name="rootPath">This path should exist, otherwise the current directory will be queried.</param>
        /// <param name="lsPath">A subfolder specification</param>
        /// <param name="disconnect">Whether the SshClient should be closed</param>
        /// <returns></returns>
        List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, string rootPath, string lsPath = ".", bool disconnect = true);
        void RemoveFiles(ISecureShellSession dsm, string rootPath, IList<ConsoleFileInfo> files, string scriptName = null);
    }
}
