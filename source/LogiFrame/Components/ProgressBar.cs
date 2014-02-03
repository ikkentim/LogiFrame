// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable progress bar.
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
        /// Initializes a new instance of the LogiFrame.Components.ProgressBar class.
        /// </summary>
        public ProgressBar()
        {
            Components.Add(_borderSquare);
            Components.Add(_innerSquare);
            _innerSquare.Fill = true;
        }

        /// <summary>
        /// Gets or sets whether the progress bar is progressing horizontally.
        /// </summary>
        public bool Horizontal
        {
            get { return _horizontal; }
            set
            {
                if (SwapProperty(ref _horizontal, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether the progress bar is progressing invertedly.
        /// </summary>
        public bool Inverted
        {
            get { return _inverted; }
            set
            {
                if (SwapProperty(ref _inverted, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum Value this LogiFrame.Components.ProgressBar can have.
        /// </summary>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (SwapProperty(ref _maximumValue, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the minimum Value this LogiFrame.Components.ProgressBar can have.
        /// </summary>
        public float MinimumValue
        {
            get { return _minimumValue; }
            set
            {
                if (SwapProperty(ref _minimumValue, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Value of this LogiFrame.Components.ProgressBar.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                if (SwapProperty(ref _value, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Components.ProgressBarStyle of this LogiFrame.Components.ProgressBar.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get { return _progressBarStyle; }
            set
            {
                if (SwapProperty(ref _progressBarStyle, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            float progress = _maximumValue > _minimumValue ? (_value - _minimumValue)/(_maximumValue - _minimumValue) : 0;

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