using System;
using System.Collections.Generic;
using System.Linq;

namespace LogiFrame
{
    public class FrameSimpleGraph : FrameControl
    {
        private int _maximum = 100;
        private int _minimum;
        private BorderStyle _style;

        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                if (_maximum < value) _maximum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                if (_minimum > value) _minimum = value;
                Invalidate();
            }
        }
        
        public BorderStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        private readonly Queue<int> _values = new Queue<int>();

        public int MaxEntries => Math.Max(0, Width - (Style == BorderStyle.Border ? 2 : (Style == BorderStyle.BorderWithPadding ? 4 : 0)));

        public void PushValue(int value)
        {
            _values.Enqueue(value);
            if (_values.Count > MaxEntries)
                _values.Dequeue();

            Invalidate();
        }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            int graphX, graphY, graphWidth, graphHeight;
            switch (Style)
            {
                case BorderStyle.Border:
                    graphX = 1;
                    graphY = 1;
                    graphWidth = Width - 2;
                    graphHeight = Height - 2;
                    break;
                case BorderStyle.BorderWithPadding:
                    graphX = 2;
                    graphY = 2;
                    graphWidth = Width - 4;
                    graphHeight = Height - 4;
                    break;
                case BorderStyle.None:
                default:
                    graphX = 0;
                    graphY = 0;
                    graphWidth = Width;
                    graphHeight = Height;
                    break;
            }

            if (Style == BorderStyle.Border || Style == BorderStyle.BorderWithPadding)
            {
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
            }
            
            if (graphWidth <= 0 || graphHeight <= 0 || !_values.Any())
            {
                base.OnPaint(e);
                return;
            }

            var cx = graphX + MaxEntries - _values.Count;

            foreach (
                var height in
                    _values.Select(value => (float) (value - Minimum)/(Maximum - Minimum))
                        .Select(value => Math.Min(1f, Math.Max(0f, value)))
                        .Select(value => (int) (value*graphHeight)))
            {
                for (var y = 0; y < height; y++)
                    e.Bitmap[cx, graphY + graphHeight - y] = true;

                cx++;
            }

            base.OnPaint(e);
        }

        #endregion
    }
}