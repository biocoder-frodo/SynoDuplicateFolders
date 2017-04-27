using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SynoDuplicateFolders.Data
{
    public class DuplicatesFolders : SortedDictionary<string, DuplicatesFolder>
    {
        internal DuplicatesFolders()
        {            
        }

        internal DuplicatesFolder addFolder(DuplicatesFolder item)
        {
            if (base.ContainsKey(item.Name) == false)
            {
                base.Add(item.Name, item);
            }
            else
            {
                item = base[item.Name];
            }
            return item;
        }

        [Obsolete("This method is not supported", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void Add(string key, DuplicatesFolder item)
        {
            throw new NotSupportedException();
        }
    }
}
