using System;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FramePaintEventArgs : EventArgs
    {
        public FramePaintEventArgs(MonochromeBitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            Bitmap = bitmap;
        }

        public MonochromeBitmap Bitmap { get; }
    }
}