namespace SynoDuplicateFolders.Data.Core
{
    public interface ISynoCSVReportPair
    {
        ISynoCSVReport First { get; }
        ISynoCSVReport Second { get; }
        void Initialize(ISynoCSVReport first, ISynoCSVReport second);
    }
}
