using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ListPoker.View
{
    class RotatedLabel : Label
    {
        protected override void OnPaint (PaintEventArgs e)
        {
            e.Graphics.RotateTransform(90);
        }
    }
}
