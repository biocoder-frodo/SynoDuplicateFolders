using Renci.SshNet;
using System.Collections.Generic;

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

    }
}
