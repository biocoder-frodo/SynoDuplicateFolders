using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SynoDuplicateFolders.Controls
{
    public sealed partial class TimeStampTrackBar : UserControl
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

                trackBar.Minimum = 0;
                trackBar.Maximum = _daterange.Count - 1;

                trackBar.SmallChange = 1;
                trackBar.LargeChange = _daterange.Count / 20;
                trackBar.TickFrequency = _daterange.Count / 20;
                lblStart.Text = _daterange.First().Value.ToString();
                lblEnd.Text = _daterange.Last().Value.ToString();
                trackBar.Value = trackBar.Maximum;
            }
        }
        public DateTime Value
        {
            get
            {
                if (_daterange != null)
                {
                    return _daterange.Values[trackBar.Value];
                }
                else
                {
                    return default;
                }
            }
            set
            {
                if (_daterange != null)
                {
                    var idx = _daterange.IndexOfKey(value);
                    if (idx != -1) trackBar.Value = idx;
                }
            }
        }

        private void TrackBar_ValueChanged(object sender, System.EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Trackbar_ValueChanged");
            if (_daterange is null) return;

            lblValue.Text = _daterange.Values[trackBar.Value].ToString();

            this.ValueChanged?.Invoke(this, e);
        }
        private void TimeStampTrackBar_Resize(object sender, EventArgs e)
        {
            lblValue.Top = lblStart.Top;
            lblValue.Left = (lblEnd.Left + lblStart.Left) / 2;
        }
    }
}
