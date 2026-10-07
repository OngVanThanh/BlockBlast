using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockBlast.Module
{
    internal class BufferedPanel : Panel
    {

        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                           ControlStyles.UserPaint |
                           ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
