using System.Collections.Generic;

namespace DiskStationManager.SecureShell
{
    public abstract class BDSMVersion : IDSMVersion
    {
        internal readonly Dictionary<string, string> _props;
        public BDSMVersion(Dictionary<string, string> properties)
        {
            _props = properties;
        }
        public int MajorVersion { get { return int.Parse(_props["majorversion"]); } }
        public int MinorVersion { get { return int.Parse(_props["minorversion"]); } }
        public int BuildNumber { get { return int.Parse(_props["buildnumber"]); } }
        public int PatchVersion { get { return int.Parse(_props["smallfixnumber"]); } }
        public string BuildPhase { get { return _props["buildphase"]; } }
        public string BuildDate { get { return _props["builddate"]; } }

        public abstract string Version { get; }

    }
}
