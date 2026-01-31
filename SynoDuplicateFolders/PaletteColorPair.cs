using System;
using System.Collections.Generic;
using System.Drawing;

namespace SynoDuplicateFolders
{
    public struct PaletteColorPair
    {
        public readonly PaletteColor First;
        public readonly PaletteColor Second;
        public PaletteColorPair(PaletteColor first, PaletteColor second)
        {
            First = first;
            Second = second;
        }

        public double GetDistance(Dictionary<PaletteColor, Color> map)
        {
            double a1 = map[First].A / 255d;
            double r1 = map[First].R / 255d;
            double g1 = map[First].G / 255d;
            double b1 = map[First].B / 255d;

            double a2 = map[Second].A / 255d;
            double r2 = map[Second].R / 255d;
            double g2 = map[Second].G / 255d;
            double b2 = map[Second].B / 255d;
            // euclidian distance
            var d = Math.Sqrt((a1 - a2) * (a1 - a2) + (r1 - r2) * (r1 - r2) + (g1 - g2) * (g1 - g2) + (b1 - b2) * (b1 - b2));
            if (d == 0)
            {
                System.Diagnostics.Debug.WriteLine($"colors are the same {map[First]}");
            }
            if (d == 1)
            {
                System.Diagnostics.Debug.WriteLine($"unity distance for {map[First]} to {map[Second]} ");
            }

            return d;
        }

    }

}
