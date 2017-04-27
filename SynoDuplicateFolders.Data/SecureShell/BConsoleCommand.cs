using Renci.SshNet;
using SynoDuplicateFolders.Extensions;
using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public abstract class BConsoleCommand : IConsoleCommand
    {
        internal Dictionary<string, string> _properties = null;
        internal static IConsoleCommand GetDSMConsole(SshClient client)
        {
            Dictionary<string, string> properties = GetVersionProperties(client);
            IDSMVersion version = new DSMVersion4(properties);
            if (version.MajorVersion >= 6)
            {
                return new ConsoleCommandDSM6(properties);
            }
            else
            {
                return new ConsoleCommandDSM4(properties);
            }
        }

        internal static Dictionary<string, string> GetVersionProperties(SshClient client)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var cmd2 = client.RunCommand("cat /etc.defaults/VERSION");
            string[] properties = cmd2.Result.Split('\n');
            foreach (string p in properties)
            {
                if (p.Length > 0)
                {
                    string name = p.Substring(0, p.IndexOf("="));
                    string value = p.Substring(p.IndexOf("=") + 1).Trim();

                    result.Add(name, value.RemoveEnclosingCharacter("\""));
                }
            }
            return result;
        }
        public abstract IDSMVersion GetVersionInfo();
        public abstract IDSMVersion GetVersionInfo(SshClient client);
        public abstract List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, bool Disconnect = true);
        public abstract void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases);
    }
}
