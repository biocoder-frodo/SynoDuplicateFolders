using Renci.SshNet;
using Renci.SshNet.Common;
using SynoDuplicateFolders.Extensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public abstract class BConsoleCommand : IConsoleCommand
    {
        internal static readonly string sudo = "sudo {0}";
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

        internal string RemoveFileCommand(string path)
        {
            return string.Format("rm {0}", path);
        }
        internal string RemoveFileCommand(SynoReportViaSSH connection, ConsoleFileInfo file)
        {
            return RemoveFileCommand(connection.SynoReportHome.Replace("/synoreport/", "") + file.Path);
        }
        internal void RemoveFile(SynoReportViaSSH connection, ConsoleFileInfo file, ConsoleCommandMode mode, SshClient session = null)
        {
            RunCommand(connection, RemoveFileCommand(connection, file), mode, session);
        }

        internal string RunCommand(SynoReportViaSSH connection, string command, ConsoleCommandMode mode = ConsoleCommandMode.Directly, SshClient session = null)
        {

            switch (mode)
            {
                case ConsoleCommandMode.Sudo:
                    return session.RunCommand(string.Format(sudo, command)).Result;
                case ConsoleCommandMode.InteractiveSudo:
                    ExperimentalSudo(connection, command);
                    return string.Empty;
                default:
                    return session.RunCommand(command).Result;
            }
        }
        private void ExperimentalSudo(SynoReportViaSSH session, string command)
        {
            try
            {
                using (SshClient sshClient = new SshClient(session.ConnectionInfo))
                {

                    sshClient.Connect();
                    IDictionary<TerminalModes, uint> termkvp = new Dictionary<TerminalModes, uint>
                {
                    { TerminalModes.ECHO, 53 }
                };

                    ShellStream shellStream = sshClient.CreateShellStream("xterm", 80, 24, 800, 600, 1024, termkvp);


                    //Get logged in
                    string rep = shellStream.Expect(new Regex(@"[$>]")); //expect user prompt
                                                                         //this.writeOutput(results, rep);
                                                                         //
                                                                         //send command
                    shellStream.WriteLine("sudo " + command);
                    rep = shellStream.Expect(new Regex(@"([$#>:])")); //expect password or user prompt
                                                                      //this.writeOutput(results, rep);

                    //check to send password
                    if (rep.Contains(":"))
                    {
                        //send password
                        shellStream.WriteLine(session.Password);
                        rep = shellStream.Expect(new Regex(@"[$#>]"), new TimeSpan(0, 0, 10)); //expect user or root prompt
                                                                                               //this.writeOutput(results, rep);
                        if (rep == null)
                        {
                            System.Diagnostics.Debug.WriteLine("sudo action failed?");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("{0}\r\n{1}", command, rep);
                        }
                    }

                    sshClient.Disconnect();
                }
            }//try to open connection
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                throw ;
            }

        }
    }
}
