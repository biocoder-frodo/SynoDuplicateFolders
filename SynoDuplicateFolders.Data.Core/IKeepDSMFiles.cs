namespace SynoDuplicateFolders.Data.Core
{
    public interface IKeepDSMFiles: ISpecificSettings
    {
        bool Custom { get; }
        bool KeepAll { get; }
        int KeepCount { get; }
    }
}
