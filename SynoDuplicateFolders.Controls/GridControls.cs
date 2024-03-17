using System.Collections.Generic;
using System.Windows.Forms;

namespace SynoDuplicateFolders.Controls
{

#if DESIGNER_WORKAROUND
    public
#else
    internal 
#endif
        class GridControls<T> : List<T> where T : Control
    {
        private readonly List<T> controls = new List<T>();
        protected MouseEventHandler mouseEvent;
        public GridControls(MouseEventHandler mouseEventHandler)
        {
            mouseEvent = mouseEventHandler;
        }
        public new void Add(T control)
        {
            control.MouseClick += mouseEvent;            
            base.Add(control);
        }
        public new void Clear()
        {
            foreach (T c in this)
            {
                c.MouseClick -= mouseEvent;
            }
            base.Clear();
        }
    }
}
