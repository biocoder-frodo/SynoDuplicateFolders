using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Controls
{
    public class ItemsComparedEventArgs : EventArgs
    {
        private readonly IList<string> items = null;

#if DESIGNER_WORKAROUND
        public
#else
    internal 
#endif
        ItemsComparedEventArgs(IList<string> paths)
        {
            items = new List<string>(paths);
        }

        public IReadOnlyList<string> Items
        {
            get
            {
                return items as IReadOnlyList<string>;
            }
        }
    }
}
