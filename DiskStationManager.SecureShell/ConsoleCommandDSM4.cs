using Renci.SshNet;
using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    internal partial class ConsoleCommandDSM4 : BConsoleCommand
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
    }
}
