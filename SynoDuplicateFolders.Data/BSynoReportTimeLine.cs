using System;
using System.Collections.Generic;
using SynoDuplicateFolders.Data.Core;

namespace SynoDuplicateFolders.Data
{
    public abstract class BSynoReportTimeLine : BSynoCSVReport
    {
        public readonly Dictionary<DateTime, ISynoCSVReport> _list = new Dictionary<DateTime, ISynoCSVReport>();
        public BSynoReportTimeLine()
            : base(SynoReportMode.TimeLine)
        { }

        public override void LoadReport(ISynoCSVReport component)
        {
            _list.Add(component.Timestamp, component);
        }
    }
}
