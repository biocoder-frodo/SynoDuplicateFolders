using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SynoDuplicateFolders.Controls
{
    public class RectangleLayout : IRectangleLayout, IComparable, IComparable<RectangleLayout>
    {
        public static List<RectangleLayout> Layouts(int count)
        {
            var result = new List<RectangleLayout>();
            for (short i = 1; i <= count + 1; i++)
            {
                for (short j = 1; j <= count + 1; j++)
                {
                    RectangleLayout layout = new RectangleLayout(i, j, count);
                    if (layout.LayoutFits)
                    {
                        result.Add(layout);
                    }

                }
            }
            result.Sort();
            return result;
        }
        public RectangleLayout(short rows, short columns, int occupied)
        {
            Rows = rows;
            Columns = columns;
            Occupied = occupied;
        }

        public short Rows { get; }
        public short Columns { get; }
        public int Occupied { get; }
        public int Rank => Rows * Columns - Occupied;
        public bool LayoutFits => Rank >= 0;
        public double AspectRatio => Columns / Convert.ToDouble(Rows);

        public double FillWeight => 1.0 + (Rank / Convert.ToDouble(Rank + Occupied));

        public int CompareTo(object obj)
        {
            return CompareTo(obj as RectangleLayout);
        }
        public int CompareTo(RectangleLayout other)
        {
            if (other is null) throw new ArgumentNullException(nameof(other));
            return this.Rank - other.Rank;
        }
        public override string ToString()
        {
            return $"{Rows}x{Columns}:{Occupied}";
        }
    }
}
