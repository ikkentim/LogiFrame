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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable progress bar.
    /// </summary>
    public class ProgressBar : Container
    {
        private readonly Square _borderSquare = new Square();
        private readonly Square _innerSquare = new Square();
        private bool _isHorizontal = true;
        private bool _isInverted;
        private float _maximumValue = 100;
        private float _minimumValue;
        private ProgressBarStyle _progressBarStyle = ProgressBarStyle.NoBorder;
        private float _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProgressBar" /> class.
        /// </summary>
        public ProgressBar()
        {
            Components.Add(_borderSquare);
            Components.Add(_innerSquare);
            _innerSquare.IsFilled = true;
        }

        /// <summary>
        ///     Gets or sets whether the progress bar is progressing horizontally.
        /// </summary>
        public bool IsHorizontal
        {
            get { return _isHorizontal; }
            set { SwapProperty(ref _isHorizontal, value); }
        }

        /// <summary>
        ///     Gets or sets whether the progress bar is progressing inverted.
        /// </summary>
        public bool IsInverted
        {
            get { return _isInverted; }
            set { SwapProperty(ref _isInverted, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum value.
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
        ///     Gets or sets the minimum value.
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
        ///     Gets or sets the value.
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
        ///     Gets or sets the progress bar style.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get { return _progressBarStyle; }
            set { SwapProperty(ref _progressBarStyle, value); }
        }

        /// <summary>
        /// Renders all graphics of this <see cref="Container" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
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
                    _borderSquare.IsVisible = true;
                    break;
                case ProgressBarStyle.NoBorder:
                    borderOffset = 0;
                    _borderSquare.IsVisible = false;
                    break;
                case ProgressBarStyle.WhiteSpacedBorder:
                    borderOffset = 2;
                    _borderSquare.IsVisible = true;
                    break;
            }

            if (_isHorizontal)
            {
                _innerSquare.Size = new Size((int) ((Size.Width - borderOffset*2)*progress),
                    Size.Height - borderOffset*2);
                _innerSquare.Location = new Location(
                    _isInverted ? Size.Width - _innerSquare.Size.Width - borderOffset : borderOffset, borderOffset);
            }
            else
            {
                _innerSquare.Size = new Size(Size.Width - borderOffset*2,
                    (int) ((Size.Height - borderOffset*2)*progress));
                _innerSquare.Location = new Location(borderOffset,
                    _isInverted ? Size.Height - _innerSquare.Size.Height - borderOffset : borderOffset);
            }

            return base.Render();
        }
    }
}