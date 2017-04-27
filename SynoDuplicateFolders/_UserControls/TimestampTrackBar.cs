using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsControlSamples
{
    public partial class TimestampTrackBar : UserControl
    {
        private IList<DateTime> _daterange = null;
        public TimestampTrackBar()
        {
            InitializeComponent();
        }

        public IList<DateTime> DateRange
        {
            set
            {
                _daterange = value;
                trackBar1.Minimum = 0;
                trackBar1.Maximum = _daterange.Count - 1;
                trackBar1.SmallChange = 1;
                trackBar1.LargeChange = _daterange.Count / 20;
                trackBar1.TickFrequency = _daterange.Count / 20;
                lblStart.Text = _daterange.First().ToString();
                lblEnd.Text = _daterange.Last().ToString();
            }
        }
        public DateTime Value
        {
            get { return _daterange[trackBar1.Value]; }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(_daterange[trackBar1.Value]);
            lblValue.Text = _daterange[trackBar1.Value].ToString();
        }

        private void TimestampTrackBar_Load(object sender, EventArgs e)
        {

        }
    }
}
