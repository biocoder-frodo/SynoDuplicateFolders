using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynoDuplicateFolders.Controls
{
    public class ItemStatusUpdateEventArgs : EventArgs
    {
        public readonly string Status;
        internal ItemStatusUpdateEventArgs(string status)
        {
            Status = status;
        }
    }
}
