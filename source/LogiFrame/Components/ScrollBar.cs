// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable scroll bar.
    /// </summary>
    public class ScrollBar : Container //Incomplete component
    {
        private readonly Square _backgroundSquare;
        private readonly Square _square;
        private bool _horizontal = true;
        private float _maximumValue = 100;
        private float _value;

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.ScrollBar class.
        /// </summary>
        public ScrollBar()
        {
            Components.Add(_backgroundSquare = new Square
            {
                Fill = true
            });
            Components.Add(_square = new Square
            {
                Fill = true,
                Transparent = true,
                TopEffect = true
            });
        }

        /// <summary>
        ///     Gets or sets whether the scroll bar is progressing horizontally.
        /// </summary>
        public bool Horizontal
        {
            get { return _horizontal; }
            set { SwapProperty(ref _horizontal, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum Value this LogiFrame.Components.ScrollBar can have.
        /// </summary>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (value < 0) value = 0;
                if (_value > value) _value = value;
                SwapProperty(ref _maximumValue, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Value of this LogiFrame.Components.ScrollBar.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                if (value < 0) value = 0;
                if (value > _maximumValue) value = _maximumValue;
                SwapProperty(ref _value, value);
            }
        }

        protected override Bytemap Render()
        {
            var sbsize = (int) Math.Ceiling(((Horizontal ? Size.Width : Size.Height)/_maximumValue));
            float spos = ((Horizontal ? Size.Width : Size.Height) - sbsize)*
                         (_maximumValue <= 0 ? 0 : _value/_maximumValue);
            int bwidth = (Horizontal ? Size.Height : Size.Width)/5;

            if (_horizontal)
            {
                _backgroundSquare.Location.Set(0, Size.Height/2 - (bwidth/2));
                _backgroundSquare.Size.Set(Size.Width, bwidth);

                _square.Location.Set((int) Math.Round(spos), 0);
                _square.Size.Set(sbsize, Size.Height);
            }
            else
            {
                _backgroundSquare.Location.Set(Size.Width/2 - (bwidth/2), 0);
                _backgroundSquare.Size.Set(bwidth, Size.Height);

                _square.Location.Set(0, (int) Math.Round(spos));
                _square.Size.Set(Size.Width, sbsize);
            }

            return base.Render();
        }
    }
}