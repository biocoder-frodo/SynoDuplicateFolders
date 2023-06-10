using Extensions;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    public abstract class BConsoleCommand : IConsoleCommand
    {
        internal string _homepath = null;
        internal Dictionary<string, string> _properties = null;
        private static readonly Random rng = new Random();
        
        public abstract List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, string rootPath, string lsPath = ".", bool disconnect = true);
        public abstract void RemoveFiles(ISecureShellSession dsm, string rootPath, IList<ConsoleFileInfo> files, string scriptName = null);


        public static string GetTempPathName()
        {
            return GetTempId().ToString("x8");
        }
        private static long GetTempId()
        {
            return Convert.ToInt64(new decimal(long.MaxValue) * (rng.Next(int.MinValue, int.MaxValue) / new decimal(int.MaxValue)));
        }
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
            var result = new Dictionary<string, string>();

            var cmd = client.RunCommand("cat /etc.defaults/VERSION");
            var properties = cmd.Result.Split('\n');
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
