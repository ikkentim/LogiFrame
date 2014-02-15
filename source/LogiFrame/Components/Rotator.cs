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
    /// Represents a LogiFrame.Components.Container which can be rotated.
    /// </summary>
    public class Rotator : Container
    {
        private Rotation _rotation;

        /// <summary>
        /// Gets or sets the LogiFrame.Components.Rotation of this LogiFrame.Components.Rotator.
        /// </summary>
        public Rotation Rotation
        {
            get { return _rotation; }
            set
            {
                if (SwapProperty(ref _rotation, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            //Sum rotation
            var rotation = 0;
            if (Rotation.HasFlag(Rotation.Rotate90Degrees)) rotation += 90;
            if (Rotation.HasFlag(Rotation.Rotate180Degrees)) rotation += 180;
            if (Rotation.HasFlag(Rotation.Rotate270Degrees)) rotation += 270;
            rotation = rotation%360;

            //Render original result
            var result = base.Render();

            //Calculate goal canvas
            var endresult = new Bytemap(rotation == 90 || rotation == 270 ? Size.Height : Size.Width,
                rotation == 90 || rotation == 270 ? Size.Width : Size.Height);

            //Rotation algorithm
            switch (rotation)
            {
                case 0:
                    endresult = result;
                    break;
                case 90:
                    for (var x = 0; x < Size.Width; x++)
                        for (var y = 0; y < Size.Height; y++)
                            endresult.Data[x*Size.Height + (Size.Height - 1 - y)] = result.Data[x + Size.Width*y];
                    break;
                case 180:
                    for (var x = 0; x < Size.Width; x++)
                        for (var y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x) + Size.Width*(Size.Height - 1 - y)] =
                                result.Data[x + Size.Width*y];
                    break;
                case 270:
                    for (var x = 0; x < Size.Width; x++)
                        for (var y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x)*Size.Height + (y)] = result.Data[x + Size.Width*y];
                    break;
            }

            //Horizontal flip
            if (Rotation.HasFlag(Rotation.FlipHorizontal))
            {
                var hflipsrc = endresult;
                endresult = new Bytemap(endresult.Size);
                for (var x = 0; x < endresult.Size.Width; x++)
                    for (var y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[endresult.Size.Width - 1 - x + y*endresult.Size.Width] =
                            hflipsrc.Data[x + y*endresult.Size.Width];
            }

            //Vertical flip
            if (Rotation.HasFlag(Rotation.FlipVertical))
            {
                var vflipsrc = endresult;
                endresult = new Bytemap(endresult.Size);
                for (var x = 0; x < endresult.Size.Width; x++)
                    for (var y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[x + (endresult.Size.Height - 1 - y)*endresult.Size.Width] =
                            vflipsrc.Data[x + y*endresult.Size.Width];
            }
            return endresult;
        }
    }
}