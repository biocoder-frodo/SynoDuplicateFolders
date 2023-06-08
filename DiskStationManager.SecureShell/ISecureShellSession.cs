using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiskStationManager.SecureShell
{
    public interface ISecureShellSession
    {
        event EventHandler HostKeyChange;

        ConnectionInfo ConnectionInfo { get; }
        
        DSMHost Host { get; }

        Func<DSMKeyboardInteractiveEventArgs, string> InteractiveMethod { get; }
        Func<string> GetPassword { get; }
        IProxySettings Proxy { get; }
        string Version { get; }
    }
}
