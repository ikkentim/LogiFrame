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
    ///     Represents a <see cref="Container" /> which can be rotated.
    /// </summary>
    public class Rotator : Container
    {
        private Rotation _rotation;

        /// <summary>
        ///     Gets or sets the rotation.
        /// </summary>
        public Rotation Rotation
        {
            get { return _rotation; }
            set { SwapProperty(ref _rotation, value); }
        }

        /// <summary>
        /// Renders all graphics of this <see cref="Container" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            //Sum rotation
            int rotation = 0;
            if (Rotation.HasFlag(Rotation.Rotate90Degrees)) rotation += 90;
            if (Rotation.HasFlag(Rotation.Rotate180Degrees)) rotation += 180;
            if (Rotation.HasFlag(Rotation.Rotate270Degrees)) rotation += 270;
            rotation = rotation%360;

            //Render original result
            Snapshot result = base.Render();

            //Calculate goal canvas
            var endresult = new Snapshot(rotation == 90 || rotation == 270 ? Size.Height : Size.Width,
                rotation == 90 || rotation == 270 ? Size.Width : Size.Height);

            //Rotation algorithm
            switch (rotation)
            {
                case 0:
                    endresult = result;
                    break;
                case 90:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[x*Size.Height + (Size.Height - 1 - y)] = result.Data[x + Size.Width*y];
                    break;
                case 180:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x) + Size.Width*(Size.Height - 1 - y)] =
                                result.Data[x + Size.Width*y];
                    break;
                case 270:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x)*Size.Height + (y)] = result.Data[x + Size.Width*y];
                    break;
            }

            //IsHorizontal flip
            if (Rotation.HasFlag(Rotation.FlipHorizontal))
            {
                Snapshot hflipsrc = endresult;
                endresult = new Snapshot(endresult.Size);
                for (int x = 0; x < endresult.Size.Width; x++)
                    for (int y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[endresult.Size.Width - 1 - x + y*endresult.Size.Width] =
                            hflipsrc.Data[x + y*endresult.Size.Width];
            }

            //Vertical flip
            if (Rotation.HasFlag(Rotation.FlipVertical))
            {
                Snapshot vflipsrc = endresult;
                endresult = new Snapshot(endresult.Size);
                for (int x = 0; x < endresult.Size.Width; x++)
                    for (int y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[x + (endresult.Size.Height - 1 - y)*endresult.Size.Width] =
                            vflipsrc.Data[x + y*endresult.Size.Width];
            }
            return endresult;
        }
    }
}