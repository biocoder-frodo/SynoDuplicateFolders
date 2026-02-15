using System;
using System.Collections.Generic;
using System.Text;

namespace SynoDuplicateFolders.Data.Core
{
    public static class TraceName
    {
        public static Func<string> TotalSizeGetter { set; private get; }
        public static Func<string> TotalUsedGetter { set; private get; }
        public static Func<string> UsedGetter { set; private get; }
        public static Func<string> FreeGetter { set; private get; }

        public static string TotalSize => ReturnName(TotalSizeGetter, "Total Size");
        public static string TotalUsed => ReturnName(TotalUsedGetter, "Total Used");
        public static string Used => ReturnName(UsedGetter, "Used");
        public static string Free => ReturnName(FreeGetter, "Free");

        public static bool IsTotal(string trace) => trace == TotalSize || trace == TotalUsed;
        public static bool IsUsage(string trace) => trace == Used || trace == Free;

        private static string ReturnName(Func<string> getter, string defaultName)
        {
            if (getter is null) return defaultName;
            string name = getter();
            return string.IsNullOrWhiteSpace(name) ? defaultName : name;
        }
    }
}
