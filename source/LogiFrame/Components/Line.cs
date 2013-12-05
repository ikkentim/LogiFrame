// Line.cs
// 
// LogiFrame rendering library.
// Copyright (C) 2013 Tim Potze
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
    /// Represents a drawable line.
    /// </summary>
    public class Line : Component
    {
        #region Fields

        private readonly Location _end = new Location();
        private readonly Location _start = new Location();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.components.Line class.
        /// </summary>
        public Line()
        {
            _start.Changed += start_Changed;
            _end.Changed += end_Changed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should start at.
        /// </summary>
        public Location Start
        {
            get { return _start; }
            set
            {
                _start.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - End.X) + 1, Math.Abs(value.Y - End.Y) + 1);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should end at.
        /// </summary>
        public Location End
        {
            get { return _end; }
            set
            {
                _end.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - Start.X) + 1, Math.Abs(value.Y - Start.Y) + 1);
                HasChanged = true;
            }
        }

        #endregion

        #region Methods

        protected override Bytemap Render()
        {
            //TODO: More efficient rendering
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
            Location start = new Location(0, Start.Y <= End.Y ? 0 : Size.Height - 1);
            Location end = new Location(Size.Width - 1, Start.Y > End.Y ? 0 : Size.Height - 1);
            System.Drawing.Graphics.FromImage(bitmap)
                .DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Black), start, end);
            return Bytemap.FromBitmap(bitmap);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Listener for Location.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void end_Changed(object sender, EventArgs e)
        {
            End = _end;
        }

        /// <summary>
        /// Listener for Location.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Changed(object sender, EventArgs e)
        {
            Start = _start;
        }

        #endregion
    }
}