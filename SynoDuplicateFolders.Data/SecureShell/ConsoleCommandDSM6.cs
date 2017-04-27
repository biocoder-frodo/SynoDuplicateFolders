using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SynoDuplicateFolders.Data.SecureShell
{
    internal class ConsoleCommandDSM6 : BConsoleCommand
    {
        public ConsoleCommandDSM6(Dictionary<string, string> version)
        {
            _properties = version;
        }
        public override IDSMVersion GetVersionInfo()
        {
            return new DSMVersion6(_properties);
        }

        public override IDSMVersion GetVersionInfo(SshClient client)
        {
            return new DSMVersion6(GetVersionProperties(client));
        }

        public override List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, bool Disconnect = true)
        {

            List<ConsoleFileInfo> result = new List<ConsoleFileInfo>();
            var cmd2 = client.RunCommand("ls -latR --time-style=full-iso synoreport");
            string[] result2 = cmd2.Result.Split('\n');

            if (Disconnect == true) client.Disconnect();

            int row = 0;

            while (row < result2.Count())
            {
                string folder = "/" + result2[row].Substring(0, result2[row].Length - 1);
                row++; row++;
                while (result2[row].Length > 0)
                {

                    if (result2[row].StartsWith("d") == false)
                    {
                        //System.Diagnostics.Debug.WriteLine(result2[row]);

                        string permission = result2[row].Substring(0, 10);
                        string parse = result2[row].Substring(10).TrimStart();
                        string grp = parse.Substring(0, parse.IndexOf(' ', 0));
                        parse = parse.Substring(parse.IndexOf(' ', 0)).TrimStart();
                        string uid1 = parse.Substring(0, parse.IndexOf(' ', 0));
                        parse = parse.Substring(parse.IndexOf(' ', 0)).TrimStart();
                        string uid2 = parse.Substring(0, parse.IndexOf(' ', 0));
                        parse = parse.Substring(parse.IndexOf(' ', 0)).TrimStart();
                        long filesize = long.Parse(parse.Substring(0, parse.IndexOf(' ', 0)));
                        parse = parse.Substring(parse.IndexOf(' ', 0)).TrimStart();
                        string ft = parse.Substring(0, 35);
                        string file = parse.Substring(35).TrimStart();


                        DateTime ts = default(DateTime);
                        DateTime.TryParse(ft, out ts);


                        result.Add(new ConsoleFileInfo(folder, file, ts.ToUniversalTime()));
                    }
                    row++;
                }
                row++;
            }
            return result;

        }
        public override void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases)
        {
            //using (SshClient scr = new SshClient(session.Host, session.UserName, session.Password))
            //{
            //    scr.Connect();
           // ExperimentalSudo(session.Host, session.UserName, session.Password, "ls");

           
            foreach (var db in dsm_databases)
            {
                    string deleteme = "/volume1/homes/" + session.UserName + "/" + db.Path.Substring(1);
                    Console.WriteLine("removing " + deleteme);
                    //var cmd = scr.RunCommand("sudo rm " + deleteme);

                    ExperimentalSudo(session.Host, session.UserName, session.Password, "rm " + deleteme);

                    //if (cmd.ExitStatus != 0)
                    //{
                    //    System.Windows.Forms.MessageBox.Show(string.Format("User '{0}' needs to be member of the sudoers group.\r\n{1}", session.UserName, cmd.Error), "DSM 6 Security");
                    //}
                    //Console.WriteLine(cmd.Result);

            }

            //}

        }

        private void ExperimentalSudo(string address, string login, string password, string command)
        {
            try
            {
                SshClient sshClient = new SshClient(address, login, password);

                sshClient.Connect();
                IDictionary<TerminalModes, uint> termkvp = new Dictionary<TerminalModes, uint>();
                termkvp.Add(TerminalModes.ECHO, 53);

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
                    //password = "fail";
                    //send password
                    shellStream.WriteLine(password);
                    rep = shellStream.Expect(new Regex(@"[$#>]"), new TimeSpan(0, 0, 10)); //expect user or root prompt
                                                                                           //this.writeOutput(results, rep);
                    if (rep == null)
                    {
                        System.Diagnostics.Debug.WriteLine("sudo action failed?");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("{0}\r\n{1}",command,rep);
                    }
                }

                sshClient.Disconnect();
            }//try to open connection
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                throw ex;
            }

        }

    }
}
