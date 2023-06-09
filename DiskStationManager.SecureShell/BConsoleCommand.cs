using Extensions;
using Renci.SshNet;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    public abstract partial class BConsoleCommand : IConsoleCommand
    {
        internal string _homepath = null;
        internal Dictionary<string, string> _properties = null;

        private static string GetHomePath(SshClient client)
        {
            return client.RunCommand("readlink -f ~").Result.Split('\n')[0];
        }
        internal static IConsoleCommand GetDSMConsole(SshClient client)
        {
            string home = GetHomePath(client);
            if (!home.EndsWith("/")) home += "/";
            Dictionary<string, string> properties = GetVersionProperties(client);
            IDSMVersion version = new DSMVersion4(properties);
            if (version.MajorVersion >= 6)
            {
                return new ConsoleCommandDSM6(properties, home);
            }
            else
            {
                return new ConsoleCommandDSM4(properties, home);
            }
        }
        internal string HomePath
        {
            get { return _homepath; }
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
        internal void RemoveFile(SshClient session, ISecureShellSession dsm, string rootPath, ConsoleFileInfo file, ConsoleCommandMode mode)
        {
            SudoSession.RunCommand(session, RemoveFileCommand(rootPath, file), mode, dsm.GetPassword);
        }

    }
}
