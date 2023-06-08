using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiskStationManager.SecureShell
{
    internal partial class ConsoleCommandDSM6 : BConsoleCommand
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
    }
}
