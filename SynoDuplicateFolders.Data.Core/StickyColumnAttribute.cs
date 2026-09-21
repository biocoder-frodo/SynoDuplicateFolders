using System;

namespace SynoDuplicateFolders.Data.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StickyColumnAttribute : Attribute
    {
        public StickyColumnAttribute() { }
    }
}
