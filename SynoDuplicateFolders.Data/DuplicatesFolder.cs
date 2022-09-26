using System.Collections.Generic;
using System.Text;

namespace SynoDuplicateFolders.Data
{
    public class DuplicatesFolder
    {
        private readonly DuplicatesFolder _parent;
        public readonly string Name;
        public readonly DuplicatesFolders Folders;
        public readonly List<DuplicateFileInfo> Files = new List<DuplicateFileInfo>();
        internal DuplicatesFolder(string name, DuplicatesFolder parent)
        {
            _parent = parent;
            Name = name;
            Folders = new DuplicatesFolders();
        }
        internal DuplicatesFolder Parent { get { return _parent; } }
        public string Path
        {
            get
            {
                DuplicatesFolder r = this;
                Stack<DuplicatesFolder> list = new Stack<DuplicatesFolder>();
                StringBuilder p =new StringBuilder();
                while (r.Parent != null)
                {
                    list.Push(r);
                    r = r.Parent;
                }
                while (list.Count>0)
                {
                    r = list.Pop();
                    p.Append("/");
                    p.Append(r.Name);
                }
                return p.ToString();
            }
        }
        public void Clear()
        {
            DuplicatesFolder root = this;
            while (root.Parent != null)
            {
                root = root.Parent;
            }
            ClearFolder(root);
        }
        private void ClearFolder(DuplicatesFolder root)
        {
            root.Files.Clear();
            foreach (DuplicatesFolder f in root.Folders.Values)
            {
                ClearFolder(f);
            }
            root.Folders.Clear();
        }
    }
}
