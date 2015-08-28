using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiFrame
{
    public class FrameRectangle : FrameControl
    {
        private RectangleStyle _style;

        public RectangleStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            switch (Style)
            {
                case RectangleStyle.Bordered:
                    for (var x = 1; x < Width - 1; x++)
                    {
                        e.Bitmap[x, 0] = true;
                        e.Bitmap[x, Height - 1] = true;
                    }
                    for (var y = 0; y < Height; y++)
                    {
                        e.Bitmap[0, y] = true;
                        e.Bitmap[Width - 1, y] = true;
                    }
                    break;
                    case RectangleStyle.Filled:
                    for (var x = 0; x < Width; x++)
                        for (var y = 0; y < Height; y++)
                            e.Bitmap[x, y] = true;
                    break;
            }
            base.OnPaint(e);
        }

        #endregion
    }
}
