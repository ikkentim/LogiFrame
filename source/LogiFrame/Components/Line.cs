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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable line.
    /// </summary>
    public class Line : Component
    {
        private readonly Location _end = new Location();
        private readonly Location _start = new Location();

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        public Line()
        {
            _start.Changed += start_Changed;
            _end.Changed += end_Changed;
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should start
        ///     at.
        /// </summary>
        public Location Start
        {
            get { return _start; }
            set
            {
                _start.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - End.X) + 1, Math.Abs(value.Y - End.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should end at.
        /// </summary>
        public Location End
        {
            get { return _end; }
            set
            {
                _end.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - Start.X) + 1, Math.Abs(value.Y - Start.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            //TODO: More efficient rendering
            var bitmap = new Bitmap(Size.Width, Size.Height);
            var start = new Location(0, Start.Y <= End.Y ? 0 : Size.Height - 1);
            var end = new Location(Size.Width - 1, Start.Y > End.Y ? 0 : Size.Height - 1);
            Graphics.FromImage(bitmap)
                .DrawLine(new Pen(Brushes.Black), start, end);
            return Bytemap.FromBitmap(bitmap);
        }

        private void end_Changed(object sender, EventArgs e)
        {
            End = _end;
        }

        private void start_Changed(object sender, EventArgs e)
        {
            Start = _start;
        }
    }
}