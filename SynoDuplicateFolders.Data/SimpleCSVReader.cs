using SynoDuplicateFolders.Extensions;
using System.Collections.Generic;
using System.IO;

namespace SynoDuplicateFolders.Data
{
    public class SimpleCSVReader
    {
        private readonly StreamReader _src;
        private readonly char _sep;
        private readonly IList<SimpleCSVReaderColumnNameReplacer> _renames;
        private readonly Dictionary<string, int> _map = new Dictionary<string, int>();
        private string[] _columns = null;

        public SimpleCSVReader(StreamReader src, char separator)
            : this(src, separator, new List<SimpleCSVReaderColumnNameReplacer>())
        { }
        public SimpleCSVReader(StreamReader src, char separator, IList<SimpleCSVReaderColumnNameReplacer> renamedcolumns)
        {
            _src = src;
            _sep = separator;
            _renames = renamedcolumns;
            string[] columns = _src.ReadLine().ToLowerInvariant().Split(_sep);
            for (int i = columns.GetLowerBound(0); i <= columns.GetUpperBound(0); i++)
            {
                columns[i] = columns[i].Trim();
                foreach (SimpleCSVReaderColumnNameReplacer rule in renamedcolumns)
                {
                    switch (rule.Comparison)
                    {
                        case SimpleCSVReaderReplaceMode.Equals:
                            {
                                if (columns[i].Equals(rule.Match)) columns[i] = rule.ReplaceBy;
                                break;
                            }
                        case SimpleCSVReaderReplaceMode.Contains:
                            {
                                if (columns[i].Contains(rule.Match)) columns[i] = rule.ReplaceBy;
                                break;
                            }
                        default: break;
                    }
                }
                _map.Add(columns[i], i);
            }
        }

        public bool EndOfStream { get { return _src.EndOfStream; } }
        public void ReadLine()
        {
            _columns = _src.ReadLine().ToLowerInvariant().Split(_sep);
            for (int i = _columns.GetLowerBound(0); i <= _columns.GetUpperBound(0); i++)
            {
                _columns[i] = _columns[i].Trim().RemoveEnclosingCharacter("\"");
            }
        }
        public string GetValue(string column)
        {
            return _columns[_map[column]];
        }
        public Dictionary<string, int> Columns { get { return _map; } }
    }

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
