using Renci.SshNet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SynoDuplicateFolders.Data.SecureShell
{
    internal class ConsoleCommandDSM6 : BConsoleCommand
    {
        public ConsoleCommandDSM6(Dictionary<string, string> version, string home)
        {
            _homepath = home;
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

        public override List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, SynoReportViaSSH session, bool Disconnect = true)
        {

            List<ConsoleFileInfo> result = new List<ConsoleFileInfo>();
            var cmd2 = client.RunCommand("cd " + session.SynoReportHome + ";ls -latR --time-style=full-iso synoreport");
            string[] result2 = cmd2.Result.Split('\n');

            if (Disconnect == true) client.Disconnect();

            int row = 0;

            while (row < result2.Count() && result2[row].Length > 0)
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


                        DateTime ts;
                        DateTime.TryParse(ft, out ts);


                        result.Add(new ConsoleFileInfo(folder, file, ts.ToUniversalTime()));
                    }
                    row++;
                }
                row++;
            }
            return result;

        }

        public void WriteFile(ConnectionInfo connectionInfo, string destinationPath, Action<StreamWriter> action)
        {
            using (var ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    action(sw);
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);

                    using (ScpClient cp = new ScpClient(connectionInfo))
                    {
                        UploadFile(cp, ms, destinationPath);

                    }
                }
            }
        }
        public void WriteFile(ScpClient scpClient, string destinationPath, Action<StreamWriter> action)
        {
            using (var ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    action(sw);
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);

                    UploadFile(scpClient, ms, destinationPath);
                }
            }
        }
        public void UploadFile(ScpClient scpClient, Stream stream, string destinationPath)
        {
            bool reOpen = scpClient.IsConnected == false;
            scpClient.RemotePathTransformation = RemotePathTransformation.None;
            if (reOpen) scpClient.Connect();
            scpClient.Upload(stream, destinationPath);
            if (reOpen) scpClient.Disconnect();
        }

        public override void RemoveFiles(SynoReportViaSSH session, IList<ConsoleFileInfo> dsm_databases)
        {

            string scriptName = "./SynoDuplicateFoldersRemoveSADB.sh";
            if (dsm_databases.Count > 0)
            {
                string script = $"{this.HomePath}{scriptName}";

                WriteFile(session.ConnectionInfo, script,
                    (sw) =>
                    {

                        var folders = new Dictionary<string, ConsoleFileInfo>();
                        sw.Write("#!/bin/bash\n");
                        foreach (var file in dsm_databases)
                        {
                            if (folders.ContainsKey(file.Folder) == false)
                            {
                                folders.Add(file.Folder, file);
                            }
                            sw.Write(base.RemoveFileCommand(session, file));
                            sw.Write("\n");
                        }
                    });

                var runSession = new SudoSession(session.ConnectionInfo, () => session.Password);
                runSession.Run(new List<string>()
                    {
                        $"chmod +x {script}",
                        scriptName,
                        RemoveFileCommand(script)
                    });

            }
        }
    }
}
