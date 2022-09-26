using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynoDuplicateFolders.Data
{
    public class SynoReportContents : BSynoCSVReport
    {
        public string Contents;
        public SynoReportContents()
            : base(SynoReportMode.SingleFile)
        { }

        public override void LoadReport(StreamReader source, FileInfo filename)
        {
            _Timestamp = filename.LastWriteTimeUtc;
            Contents = source.ReadToEnd();
            if (Contents.Equals("Group, Share, Size\n") == false)
            {
                Console.WriteLine(Contents);
            }
        }
    }
}
