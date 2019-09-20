using Renci.SshNet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

                        string permission = result2[row].Substring(0, 11);
                        string parse = result2[row].Substring(11).TrimStart();
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
            if (dsm_databases.Count>0)
            { 
            string script = session.SynoReportHome.Replace("/synoreport/", "/") + "test.sh";
            using (MemoryStream ms = new MemoryStream())
            {
                    using (StreamWriter sw = new System.IO.StreamWriter(ms))
                    {
                        sw.AutoFlush = true;
                        sw.Write("#!/bin/bash\n");
                        foreach (var file in dsm_databases)
                        {
                            sw.Write(base.RemoveFileCommand(session, file));
                            sw.Write("\n");
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                        using (ScpClient cp = new ScpClient(session.ConnectionInfo))
                        {
                            cp.Connect();
                            cp.Upload(ms, script);
                            cp.Disconnect();
                        }

                        using (SshClient scr = new SshClient(session.ConnectionInfo))
                        {
                            scr.Connect();
                            RunCommand(session, "chmod +x " + script, session.RmExecutionMode, scr);
                            RunCommand(session, "./test.sh", session.RmExecutionMode, scr);
                            RunCommand(session, RemoveFileCommand(script), session.RmExecutionMode, scr);
                            scr.Disconnect();
                        }
                    }
                }
            }
        }
    }
}
