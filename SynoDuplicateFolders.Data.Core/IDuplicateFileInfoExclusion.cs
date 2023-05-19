using System.Collections.Generic;

namespace SynoDuplicateFolders.Data.Core
{
    public interface IDuplicateFileInfoExclusion
    {
        void AddExclusion(string path);
        void RemoveExclusion(string path);
        void RemoveAllExclusions();
        IReadOnlyList<string> Paths { get; }
        void AttachDetach();
    }
}
