// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace LogiFrame
{
    /// <summary>
    /// Represents a progress bar.
    /// </summary>
    public class LCDProgressBar : LCDControl
    {
        private ProgressBarDirection _direction;
        private int _maximum = 100;
        private int _minimum;
        private BorderStyle _style;
        private int _value;

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the style of the progress bar.
        /// </summary>
        public BorderStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the direction of the progress bar.
        /// </summary>
        public ProgressBarDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                Invalidate();
            }
        }

        #region Overrides of LCDControl

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            int barX, barY, barWidth, barHeight;
            switch (Style)
            {
                case BorderStyle.Border:
                    barX = 1;
                    barY = 1;
                    barWidth = Width - 2;
                    barHeight = Height - 2;
                    break;
                case BorderStyle.BorderWithPadding:
                    barX = 2;
                    barY = 2;
                    barWidth = Width - 4;
                    barHeight = Height - 4;
                    break;
                case BorderStyle.None:
                default:
                    barX = 0;
                    barY = 0;
                    barWidth = Width;
                    barHeight = Height;
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

            var val = (float) (Value - Minimum)/(Maximum - Minimum);

            if (barWidth <= 0 || barHeight <= 0 || val <= 0)
            {
                base.OnPaint(e);
                return;
            }

            {
                int x, y, width, height;
                switch (Direction)
                {
                    case ProgressBarDirection.Down:
                        x = barX;
                        y = barY;
                        width = barWidth;
                        height = (int) (barHeight*val);
                        break;
                    case ProgressBarDirection.Up:
                        x = barX;
                        height = (int) (barHeight*val);
                        y = barY + barHeight - height;
                        width = barWidth;
                        break;
                    case ProgressBarDirection.Right:
                    default:
                        x = barX;
                        y = barY;
                        width = (int) (barWidth*val);
                        height = barHeight;
                        break;
                    case ProgressBarDirection.Left:
                        y = barY;
                        width = (int) (barWidth*val);
                        x = barX + barWidth - width;
                        height = barHeight;
                        break;
                }

                for (var cx = x; cx < x + width; cx++)
                    for (var cy = y; cy < y + height; cy++)
                        e.Bitmap[cx, cy] = true;
            }

            base.OnPaint(e);
        }

        #endregion
    }
}