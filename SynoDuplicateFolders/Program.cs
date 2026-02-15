using System;
using System.Windows.Forms;
using SynoDuplicateFolders.Data.Core;
using static SynoDuplicateFolders.Properties.Settings;

namespace SynoDuplicateFolders
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TraceName.FreeGetter = () => Default.Free;
            TraceName.TotalSizeGetter = () => Default.TotalSize;
            TraceName.TotalUsedGetter = () => Default.TotalUsed;
            TraceName.UsedGetter = () => Default.Used;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SynoReportClient());
        }
    }
}
