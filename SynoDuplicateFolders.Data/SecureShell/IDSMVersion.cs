namespace SynoDuplicateFolders.Data.SecureShell
{
    public interface IDSMVersion
    {
        //majorversion="4"
        //minorversion="3"
        //buildphase="release"
        //buildnumber="3827"
        //smallfixnumber=4
        //builddate="2014/02/11"

        int MajorVersion { get; }
        int MinorVersion { get; }
        int BuildNumber { get; }
        string BuildPhase { get; }
        int PatchVersion { get; }
        string BuildDate { get; }
        string Version { get; }

    }
}
