namespace SynoDuplicateFolders.Data
{
    public enum SimpleCSVReaderReplaceMode
    {
        Equals,
        Contains
    }
    public class SimpleCSVReaderColumnNameReplacer
    {
        public readonly string Match;
        public readonly string ReplaceBy;
        public readonly SimpleCSVReaderReplaceMode Comparison;
        public SimpleCSVReaderColumnNameReplacer(SimpleCSVReaderReplaceMode comparison, string match, string replaceby)
        {
            Match = match;
            Comparison = comparison;
            ReplaceBy = replaceby;
        }
    }
}
