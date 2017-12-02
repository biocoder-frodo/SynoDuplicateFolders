using System;

namespace SynoDuplicateFolders.Controls
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnWidthAttribute : Attribute
    {
        public ColumnWidthAttribute(int width)
        { Width = width; }

        public int Width { get; set; }
    }
}
