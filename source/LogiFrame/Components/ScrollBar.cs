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
    /// Represents a drawable scroll bar.
    /// </summary>
    public class ScrollBar : Container //Incomplete component
    {
        private readonly Square _backgroundSquare;
        private readonly Square _square;
        private bool _horizontal = true;
        private float _maximumValue = 100;
        private float _value;

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.ScrollBar class.
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
        /// Gets or sets whether the scroll bar is progressing horizontally.
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
        /// Gets or sets the maximum Value this LogiFrame.Components.ScrollBar can have.
        /// </summary>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (value < 0) value = 0;
                if (_value > value) _value = value;

                if (SwapProperty(ref _maximumValue, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Value of this LogiFrame.Components.ScrollBar.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                if (value < 0) value = 0;
                if (value > _maximumValue) value = _maximumValue;

                if (SwapProperty(ref _value, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            var sbsize = (int) Math.Ceiling(((Horizontal ? Size.Width : Size.Height)/_maximumValue));
            var spos = ((Horizontal ? Size.Width : Size.Height) - sbsize)*
                       (_maximumValue <= 0 ? 0 : _value/_maximumValue);
            var bwidth = (Horizontal ? Size.Height : Size.Width)/5;

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