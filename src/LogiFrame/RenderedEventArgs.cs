using System;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class RenderedEventArgs : EventArgs
    {
        public RenderedEventArgs(MonochromeBitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public MonochromeBitmap Bitmap { get; }
    }
}