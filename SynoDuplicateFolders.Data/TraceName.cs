using System;
using System.Collections.Generic;
using System.Text;

namespace SynoDuplicateFolders.Data
{
    static class TraceName
    {
        public static void Initialize(string used, string free, string totalSize, string totalUsed)
        {
            TotalSize = totalSize;
            TotalUsed = totalUsed;
            Used = used;
            Free = free;
        }

        public static string TotalSize { get; private set; }
        public static string TotalUsed { get; private set; }
        public static string Used { get; private set; }
        public static string Free { get; private set; }

        public static bool IsTotal(string trace) => trace == TotalSize || trace == TotalUsed;
        public static bool IsUsage(string trace) => trace == Used || trace == Free;
    }
}
