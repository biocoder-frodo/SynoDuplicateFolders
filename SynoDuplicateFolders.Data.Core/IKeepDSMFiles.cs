using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynoDuplicateFolders.Data.Core
{
    public interface IKeepDSMFiles
    {
        bool Custom { get; }
        bool KeepAll { get; }
        int KeepCount { get; }
    }
}
