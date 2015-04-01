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
        private Location _end;
        private Location _start;

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public Location Start
        {
            get { return _start; }
            set
            {
                _start = value;
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - End.X) + 1, Math.Abs(value.Y - End.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        public Location End
        {
            get { return _end; }
            set
            {
                _end = value;
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - Start.X) + 1, Math.Abs(value.Y - Start.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Renders this instance to a <see cref="Snapshot" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            //TODO: More efficient rendering

            using (var bitmap = new Bitmap(Size.Width, Size.Height))
            {
                var start = new Location(0, Start.Y <= End.Y ? 0 : Size.Height - 1);
                var end = new Location(Size.Width - 1, Start.Y > End.Y ? 0 : Size.Height - 1);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawLine(new Pen(Brushes.Black), start, end);
                }
                return Snapshot.FromBitmap(bitmap);
            }
        }
    }
}