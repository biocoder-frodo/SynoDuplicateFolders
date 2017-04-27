using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Text;
using SynoDuplicateFolders.Data;

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
    }
}

namespace SynoDuplicateFolders.Extensions
{
    public static partial class Extensions
    {
        public static void Add(this TreeView target, DuplicatesFolder folder)
        {
            Add(target, folder, null);
        }
        private static void Add(this TreeView target,  DuplicatesFolder folder, TreeNode root, string key=null)
        {
            if (key == null) key = folder.Path;
            if (root == null)
            {
                root = target.Nodes.Add(key, folder.Name);
            }
            else
            {
                root = root.Nodes.Add(key, folder.Name);
            }

            foreach (var f in folder.Folders.Values)
            {
                Add(target, f, root, key+"/"+f.Name);
            }
        }
        
    }
}
