using SynoDuplicateFolders.Extensions;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Data
{
    public class DuplicatesAggregate<TKey, TValue> : SortedDictionaryOfILists<List<TValue>, TKey, TValue>
    {
        public DuplicatesAggregate(bool uniquelists = false)
            : base(uniquelists)
        {
        }
    }
}
