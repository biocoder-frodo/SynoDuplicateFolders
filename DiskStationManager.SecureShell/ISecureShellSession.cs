using Renci.SshNet;
using System;
using System.IO;

namespace DiskStationManager.SecureShell
{
    public interface ISecureShellSession
    {
        event EventHandler HostKeyChange;

        ConnectionInfo ConnectionInfo { get; }
        void ClientExecute(Action<SshClient> action);
        void ClientExecute(Action<ScpClient> action);

        [Obsolete("This method is only present to support DSM versions below 6.x")]
        void ClientExecuteAsRoot(Action<SshClient> action);

        DSMHost Host { get; }
        Func<string> GetPassword { get; }
        IProxySettings Proxy { get; }
        string Version { get; }

        void UploadFile(string destinationPath, Action<StreamWriter> action);
        void UploadFile(ScpClient scpClient, string destinationPath, Action<StreamWriter> action);
        void UploadFile(ScpClient scpClient, Stream stream, string destinationPath);
        void DownloadFile(ScpClient client, string source, FileInfo localfile);
    }
}
