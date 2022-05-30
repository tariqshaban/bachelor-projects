using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    class Gradiant:Panel
    {
        public Color Fst { get; set; }
        public Color Lst { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush rgb = new LinearGradientBrush(this.ClientRectangle, this.Fst, this.Lst, 90F);
            Graphics temp = e.Graphics;
            temp.FillRectangle(rgb,this.ClientRectangle);
            base.OnPaint(e);
        }
    }
}
