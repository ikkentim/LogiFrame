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

using System;
using System.Drawing;
using LogiFrame.Drawing;

namespace LogiFrame
{
    /// <summary>
    /// Represents a line.
    /// </summary>
    public class LCDLine : LCDControl
    {
        private Point _end;
        private Point _start;

        /// <summary>
        /// Initializes a new instance of the <see cref="LCDLine"/> class.
        /// </summary>
        public LCDLine()
        {
            MergeMethod = MergeMethods.Transparent;
        }

        /// <summary>
        /// Gets or sets the starting point of the lind.
        /// </summary>
        public virtual Point Start
        {
            get { return _start; }
            set
            {
                SuspendLayout();

                _start = value;
                base.Location = new Point(Math.Min(_start.X, _end.X), Math.Min(_start.Y, _end.Y));
                base.Size = new Size(Math.Max(_start.X, _end.X) - Location.X + 1,
                    Math.Max(_start.Y, _end.Y) - Location.Y + 1);

                ResumeLayout();
            }
        }

        /// <summary>
        /// Gets or sets the ending pont of the line.
        /// </summary>
        public virtual Point End
        {
            get { return _end; }
            set
            {
                SuspendLayout();

                _end = value;
                base.Location = new Point(Math.Min(_start.X, _end.X), Math.Min(_start.Y, _end.Y));
                base.Size = new Size(Math.Max(_start.X, _end.X) - Location.X + 1,
                    Math.Max(_start.Y, _end.Y) - Location.Y + 1);

                ResumeLayout();
            }
        }

        #region Overrides of LCDControl

        /// <summary>
        ///     Gets or sets the location of the control.
        /// </summary>
        /// <exception cref="System.NotImplementedException">use <see cref="Start"/> and <see cref="End"/> instead</exception>
        public override Point Location
        {
            get { return base.Location; }
            set { throw new NotImplementedException("use LCDLine.Start and LCDLine.End instead"); }
        }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <exception cref="System.NotImplementedException">use <see cref="Start"/> and <see cref="End"/> instead</exception>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new NotImplementedException("use LCDLine.Start and LCDLine.End instead"); }
        }

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            if (Width == 1)
            {
                for (var y = 0; y < Height; y++)
                    e.Bitmap[0, y] = true;
            }
            else
            {
                var top = Start.X < End.X ? (Start.Y < End.Y) : (End.Y < Start.Y);

                for (var x = 0; x < Width; x++)
                {
                    var ry = (int) (((float) Height/Width)*x);
                    var nry = Math.Max(ry, (int) (((float) Height/Width)*(x + 1)) - 1);
                    var sy = top ? ry : Height - ry - 1;
                    var ey = top ? nry : Height - nry - 1;
                    for (var y = sy; y <= ey; y++)
                        e.Bitmap[x, y] = true;
                }
            }
            base.OnPaint(e);
        }

        #endregion
    }
}