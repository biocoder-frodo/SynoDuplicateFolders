using SynoDuplicateFolders.Data.Core;
using System.ComponentModel;

namespace SynoDuplicateFolders.Controls
{
    public interface IDuplicateExclusionSource : IDuplicateFileInfoExclusion, INotifyPropertyChanged
    {
    }
}
