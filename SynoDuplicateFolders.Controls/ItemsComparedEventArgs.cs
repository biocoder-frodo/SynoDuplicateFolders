using System;
using System.Collections.Generic;

namespace SynoDuplicateFolders.Controls
{
    public class ItemsComparedEventArgs : EventArgs
    {
        private IList<string> items = null;
        internal ItemsComparedEventArgs(IList<string> paths)
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
