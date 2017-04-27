using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public sealed class DSMVersion4 : BDSMVersion
    {
        public DSMVersion4(Dictionary<string, string> properties)
            : base(properties)
        {
        }
        public override string Version { get { return string.Format("DSM {0}.{1}-{2}", MajorVersion, MinorVersion, BuildNumber) + (PatchVersion > 0 ? string.Format(" Update {0}", PatchVersion) : ""); } }

    }
}
