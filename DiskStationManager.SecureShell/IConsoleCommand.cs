using Renci.SshNet;

namespace DiskStationManager.SecureShell
{
    public enum ConsoleCommandMode
    {
        Directly,       
        InteractiveSudo,
        Sudo
    }
    internal partial interface IConsoleCommand
    {
        IDSMVersion GetVersionInfo(SshClient client);
        IDSMVersion GetVersionInfo();
    }
}
