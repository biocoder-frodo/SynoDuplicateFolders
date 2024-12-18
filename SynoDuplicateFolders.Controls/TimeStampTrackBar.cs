using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SynoDuplicateFolders.Controls
{
    public partial class TimeStampTrackBar : UserControl
    {
        public event EventHandler ValueChanged;

        private SortedList<DateTime, DateTime> _daterange = null;
        public TimeStampTrackBar()
        {
            InitializeComponent();
        }

        public IList<DateTime> DateRange
        {
            set
            {
                _daterange = new SortedList<DateTime, DateTime>(value.Count);
                foreach (DateTime ts in value)
                {
                    _daterange.Add(ts, ts);
                }

                trackBar1.Minimum = 0;
                trackBar1.Maximum = _daterange.Count - 1;

                trackBar1.SmallChange = 1;
                trackBar1.LargeChange = _daterange.Count / 20;
                trackBar1.TickFrequency = _daterange.Count / 20;
                lblStart.Text = _daterange.First().Value.ToString();
                lblEnd.Text = _daterange.Last().Value.ToString();
                trackBar1.Value = trackBar1.Maximum;                
            }
        }
        public DateTime Value
        {
            get
            {
                if (_daterange != null)
                {
                    return _daterange.Values[trackBar1.Value];
                }
                else
                {
                    return default(DateTime);
                }
            }
            set
            {
                if (_daterange != null)
                {
                    var idx = _daterange.IndexOfKey(value);
                    if (idx != -1) trackBar1.Value = idx;

                }
                    
            }
        }

        //private void trackBar1_Scroll(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Trackbar_Scroll");
        //    if (_daterange is null) return;

        //    lblValue.Text = _daterange.Values[trackBar1.Value].ToString();
        //    this.Scroll?.Invoke(this, e);

        //}
        private void TrackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Trackbar_ValueChanged");
            if (_daterange is null) return;

            lblValue.Text = _daterange.Values[trackBar1.Value].ToString();
            this.ValueChanged?.Invoke(this, e);
        }
        private void TimeStampTrackBar_Resize(object sender, EventArgs e)
        {
            lblValue.Top = lblStart.Top;
            lblValue.Left = (lblEnd.Left + lblStart.Left) / 2;
        }
    }
}
