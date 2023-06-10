using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskStationManager.SecureShell
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
        public override List<ConsoleFileInfo> GetDirectoryContentsRecursive(SshClient client, string rootPath, string lsPath = ".", bool disconnect = true)
        {
            List<ConsoleFileInfo> result = new List<ConsoleFileInfo>();
            var cmd2 = client.RunCommand("cd " + rootPath + ";ls -latR --time-style=full-iso "+lsPath);
            string[] result2 = cmd2.Result.Split('\n');

            if (disconnect) client.Disconnect();

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
        public override void RemoveFiles(ISecureShellSession dsm, string rootPath, IList<ConsoleFileInfo> files, string scriptName = null)
        {
            if (string.IsNullOrWhiteSpace(scriptName)) scriptName = GetTempPathName();

            if (files.Count > 0)
            {
                string script = $"{this.HomePath}./{scriptName}.sh";

                dsm.UploadFile(script, sw =>
                {
                    var folders = new Dictionary<string, ConsoleFileInfo>();
                    sw.Write("#!/bin/bash\n");
                    foreach (var file in files)
                    {
                        if (folders.ContainsKey(file.Folder) == false)
                        {
                            folders.Add(file.Folder, file);
                        }
                        sw.Write(RemoveFileCommand(rootPath, file));
                        sw.Write("\n");
                    }
                });

                var runSession = new SudoSession(dsm);
                runSession.Run(new string[]
                    {
                        $"chmod +x {script}",
                        $"./{scriptName}.sh",
                        RemoveFileCommand(script)
                    });

            }
        }
    }
}
