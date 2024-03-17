using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SynoDuplicateFolders.Data;
using SynoDuplicateFolders.Data.Core;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using SynoDuplicateFolders.Properties;
using System.Reflection;
namespace SynoDuplicateFolders.Controls
{
#if DESIGNER_WORKAROUND
    public
#else
    internal 
#endif
     class LegendConfiguration
    {
        private readonly List<string> _unknownTraces = new List<string>();
        private readonly IChartConfiguration _legends;
        private bool _invalidated;

        public LegendConfiguration(IChartConfiguration configuration)
        {
            _legends = configuration;
        }
        public void ResetUnknownTraces()
        {
            _unknownTraces.Clear();
        }

        public void AddNewTraces(int index, Chart chart, IVolumePieChart volumePie)
        {
            if (_invalidated && _unknownTraces.Count > 0)
            {
                DataPointCollection dpc = chart.Series[0].Points;
                
                var legendMap = new Dictionary<string,DataPoint>();
                foreach (var dp in dpc)
                {
                    if (legendMap.ContainsKey(dp.LegendText) == false)
                        legendMap.Add(dp.LegendText, dp);
                }
                var colorMap = new Dictionary<string, Color>();
                //int slice = 0;
                foreach (PieChartDataPoint dp in volumePie[index])
                {
                    if (legendMap.ContainsKey(dp.SliceName))
                    colorMap.Add(dp.SliceName, legendMap[dp.SliceName].Color);
                }

                AddNewTraces(colorMap.Count, (idx) => colorMap.Keys.ToList()[idx], (idx) => colorMap[colorMap.Keys.ToList()[idx]]);
            }
            _invalidated = false;
        }
        public void AddNewTraces(int allTraceCount, Func<int, string> traceName, Func<int, Color> traceColor)
        {
            
            if (_invalidated && _unknownTraces.Count > 0)
            {
                int index = 0;
                bool changed = false;
                for (index = 0; index < allTraceCount; index++)
                {
                    string trace = traceName(index);
                    if (_unknownTraces.Contains(trace) && _legends.ContainsKey(trace) == false)
                    {
                        _legends.Add(trace, traceColor(index), false);
                        changed = true;
                    }
                   
                }
                if (changed) _legends.SaveLegendChanges();
            }
            _invalidated = false;
        }
        public void Invalidate()
        {
            _invalidated = true;
        }
        public bool TryPickColor(string traceName, DataPointCustomProperties dpcp)
        {
            Console.Write("trace " + traceName + ": ");
            if (_legends.ContainsKey(traceName))
            {
               
                var legend = _legends[traceName];
                Console.WriteLine("picking dictionary color");
                dpcp.Color = legend.Color;
                return true;
            }
            else
            {
                if (_unknownTraces.Contains(traceName)==false)_unknownTraces.Add(traceName);
                Console.WriteLine("picking default color");
            }
            return false;
        }
    }
}
