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
            using (SshClient scr = new SshClient(session.ConnectionInfo))
            {
                scr.Connect();

                foreach (var db in dsm_databases)
                {
                    RemoveFile(session, db, session.RmExecutionMode, scr);
                }

                scr.Disconnect();
            }
        }
    }
}
