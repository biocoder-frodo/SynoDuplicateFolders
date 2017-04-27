using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.Drawing;

namespace SynoDuplicateFolders.Extensions
{
    public static partial class Extensions
    {
        public static KnownColor ToClosestKnownColor(this Color value)
        {
            double distance;
            double min = -1;
            const KnownColor first = KnownColor.AliceBlue;
            const KnownColor last = KnownColor.YellowGreen;
            KnownColor m = first;
            Color p;
            for (KnownColor k = first; k < last; k++)
            {
                p = Color.FromKnownColor(k);
                distance = Math.Sqrt(Math.Pow(p.A - value.A, 2) + Math.Pow(p.B - value.B, 2) + Math.Pow(p.R - value.R, 2) + Math.Pow(p.G - value.G, 2));
                if (min < 0 || distance < min)
                {
                    min = distance;
                    m = k;
                }
            }
            return m;
        }
        public static string RemoveEnclosingCharacter(this string text, string character)
        {
            if (character.Length != 1)
                throw new ArgumentException("The argument only allows a single character to be passed", "character");

            if (text.StartsWith(character) && text.EndsWith(character))
            {
                switch (text.Length)
                {
                    case 1:
                        return text;
                    case 2:
                        return string.Empty;
                    default:
                        StringBuilder sb = new StringBuilder(text);
                        sb.Remove(sb.Length - 1, 1);
                        sb.Remove(0, 1);
                        return sb.ToString();
                }
            }
            return text;

        }
        public static string RemoveEnclosingCharacter(this string text, char character)
        {
            string c = new string(character, 1);
            return RemoveEnclosingCharacter(text, c);
        }

        public static void Add(this TreeView target, string path)
        {
            string[] folders = path.Split('/');
            string parent = '/' + folders[1];
            string key = string.Empty;

            for (int i = 1; i <= folders.GetUpperBound(0); i++)
            {
                key += '/' + folders[i];

                TreeNode[] nodes = target.Nodes.Find(parent, true);
                if (nodes.Count() == 0)
                {
                    target.Nodes.Add(key, folders[i]);
                    parent = key;
                }
                else
                {
                    if (target.Nodes.Find(key, true).Count() == 0)
                        nodes[0].Nodes.Add(key, folders[i]);
                    parent = key;
                }

            }
        }
    }

    public class SortedDictionaryOfILists<L, TKey, TValue> : SortedDictionary<TKey, L>
        where L : IList<TValue>, new()

    {
        private readonly bool _unique_add_in_list;

        public SortedDictionaryOfILists(bool uniquelists)
        {
            _unique_add_in_list = uniquelists;
        }

        public void Add(TKey key, TValue value)
        {
            if (base.ContainsKey(key) == false)
            {
                base.Add(key, new L());
                base[key].Add(value);
            }
            else
            {
                if (_unique_add_in_list)
                {
                    if (base[key].Contains(value) == false)
                    {
                        base[key].Add(value);
                    }
                }
                else
                {
                    base[key].Add(value);
                }
            }

        }

    }
}

