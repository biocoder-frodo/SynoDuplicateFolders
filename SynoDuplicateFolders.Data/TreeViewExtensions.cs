using System.Windows.Forms;
using SynoDuplicateFolders.Data;

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
