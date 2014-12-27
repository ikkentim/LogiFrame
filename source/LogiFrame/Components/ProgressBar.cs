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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable progress bar.
    /// </summary>
    public class ProgressBar : Container
    {
        private readonly Square _borderSquare = new Square();
        private readonly Square _innerSquare = new Square();
        private bool _horizontal = true;
        private bool _inverted;
        private float _maximumValue = 100;
        private float _minimumValue;
        private ProgressBarStyle _progressBarStyle = ProgressBarStyle.NoBorder;
        private float _value;

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.ProgressBar class.
        /// </summary>
        public ProgressBar()
        {
            Components.Add(_borderSquare);
            Components.Add(_innerSquare);
            _innerSquare.Fill = true;
        }

        /// <summary>
        ///     Gets or sets whether the progress bar is progressing horizontally.
        /// </summary>
        public bool Horizontal
        {
            get { return _horizontal; }
            set { SwapProperty(ref _horizontal, value); }
        }

        /// <summary>
        ///     Gets or sets whether the progress bar is progressing invertedly.
        /// </summary>
        public bool Inverted
        {
            get { return _inverted; }
            set { SwapProperty(ref _inverted, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum Value this LogiFrame.Components.ProgressBar can have.
        /// </summary>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (value < _minimumValue) _minimumValue = value;
                if (_value > value) _value = value;
                SwapProperty(ref _maximumValue, value);
            }
        }

        /// <summary>
        ///     Gets or sets the minimum Value this LogiFrame.Components.ProgressBar can have.
        /// </summary>
        public float MinimumValue
        {
            get { return _minimumValue; }
            set
            {
                if (value > _maximumValue) _maximumValue = value;
                if (_value < value) _value = value;
                SwapProperty(ref _minimumValue, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Value of this LogiFrame.Components.ProgressBar.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                if (value < _minimumValue) value = _minimumValue;
                if (value > _maximumValue) value = _maximumValue;
                SwapProperty(ref _value, value);
            }
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Components.ProgressBarStyle of this LogiFrame.Components.ProgressBar.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get { return _progressBarStyle; }
            set { SwapProperty(ref _progressBarStyle, value); }
        }

        protected override Bytemap Render()
        {
            float progress = _maximumValue > _minimumValue
                ? (_value - _minimumValue)/(_maximumValue - _minimumValue)
                : 0;

            _borderSquare.Size = Size;

            int borderOffset = 0;

            switch (_progressBarStyle)
            {
                case ProgressBarStyle.Border:
                    borderOffset = 1;
                    _borderSquare.Visible = true;
                    break;
                case ProgressBarStyle.NoBorder:
                    borderOffset = 0;
                    _borderSquare.Visible = false;
                    break;
                case ProgressBarStyle.WhiteSpacedBorder:
                    borderOffset = 2;
                    _borderSquare.Visible = true;
                    break;
            }

            if (_horizontal)
            {
                _innerSquare.Size.Set((int) ((Size.Width - borderOffset*2)*progress), Size.Height - borderOffset*2);
                _innerSquare.Location.Set(
                    _inverted ? Size.Width - _innerSquare.Size.Width - borderOffset : borderOffset, borderOffset);
            }
            else
            {
                _innerSquare.Size.Set(Size.Width - borderOffset*2, (int) ((Size.Height - borderOffset*2)*progress));
                _innerSquare.Location.Set(borderOffset,
                    _inverted ? Size.Height - _innerSquare.Size.Height - borderOffset : borderOffset);
            }

            return base.Render();
        }
    }
}