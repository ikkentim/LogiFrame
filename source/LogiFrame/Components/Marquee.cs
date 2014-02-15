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
using System.Drawing;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a scrolling text.
    /// </summary>
    public class Marquee : Container
    {
        #region Fields

        private readonly Label _label;
        private readonly Timer _timer;
        private int _stepSize = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Component.Marquee class.
        /// </summary>
        public Marquee()
        {
            _timer = new Timer();
            _timer.Tick += (sender, args) =>
            {
                if (!Vertical)
                {
                    var newx = _label.Location.X - StepSize;
                    if (newx <= -_label.Size.Width)
                        newx = Size.Width;
                    _label.Location.Set(newx, 0);
                }
                else
                {
                    var newy = _label.Location.Y - StepSize;
                    if (newy <= -_label.Size.Height)
                        newy = Size.Height;
                    _label.Location.Set(0, newy);
                }
            };
            _label = new Label {AutoSize = true};

            Components.Add(_label);
            Components.Add(_timer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets thetext this LogiFrame.Component.Marquee should draw.
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        /// <summary>
        /// Gets or sets the System.Drawing.Font this LogiFrame.Component.Marquee should draw with.
        /// </summary>
        public Font Font
        {
            get { return _label.Font; }
            set { _label.Font = value; }
        }

        /// <summary>
        /// Gets or sets whether the label should cache all rendered texts.
        /// </summary>
        public bool UseCache
        {
            get { return _label.UseCache; }
            set { _label.UseCache = value; }
        }

        /// <summary>
        /// Gets or sets the time in miliseconds each frame lasts.
        /// </summary>
        public int Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Gets or sets whether the text should shift around.
        /// </summary>
        public bool Run
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        public bool Vertical { get; set; }

        /// <summary>
        /// Gets or sets the number of pixels shifted each interval.
        /// </summary>
        public int StepSize
        {
            get { return _stepSize; }
            set
            {
                if (value <= 0)
                    throw new IndexOutOfRangeException("Marquee.StepSize must at least contain a value of 1 or higher.");

                _stepSize = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all items from the Label's cache.
        /// </summary>
        public void ClearCache()
        {
            _label.ClearCache();
        }

        #endregion
    }
}