using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SynoDuplicateFolders.Data.SecureShell
{
    internal class ConsoleCommandDSM4 : BConsoleCommand
    {
        public ConsoleCommandDSM4(Dictionary<string, string> version, string home)
        {
            _homepath = home;
            _properties = version;
        }
        public override IDSMVersion GetVersionInfo()
        {
            return new DSMVersion4(_properties);
        }
        public override IDSMVersion GetVersionInfo(SshClient client)
        {
            return new DSMVersion4(GetVersionProperties(client));
        }
        public override List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool Disconnect = true)
        {
            List<ConsoleFileInfo> result = new List<ConsoleFileInfo>();
            var cmd2 = client.RunCommand("cd " + session.SynoReportHome + ";ls -lARe synoreport/");
            string[] result2 = cmd2.Result.Split('\n');

            if (Disconnect == true) client.Disconnect();

            int row = 0;
            int chop = 57 + 12;

            while (row < result2.Count())
            {
                string folder = "/" + result2[row].Substring(0, result2[row].Length - 1);
                row++;
                while (result2[row].Length > 0)
                {

                    if (result2[row].StartsWith("d") == false)
                    {
                        //string file = folder + "/" + result2[row].Substring(chop);
                        string ft = result2[row].Substring(chop - 25 + 4, 24 - 4);
                        DateTime ts = default(DateTime);
                        DateTime.TryParseExact(ft, "MMM d HH:mm:ss yyyy", new CultureInfo("en-US"), (DateTimeStyles)((int)DateTimeStyles.AssumeLocal + DateTimeStyles.AllowInnerWhite), out ts);

                        result.Add(new ConsoleFileInfo(folder, result2[row].Substring(chop), ts.ToUniversalTime()));
                    }
                    row++;
                }
                row++;
            }
            return result;
        }

        public override void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases)
        {
            using (SshClient scr = new SshClient(session.Host, "root", session.Password))
            {
                scr.Connect();

                foreach (var db in dsm_databases)
                {
                    RemoveFile(session, db, ConsoleCommandMode.Directly, scr);
                }

                scr.Disconnect();
            }
        }
    }

}
