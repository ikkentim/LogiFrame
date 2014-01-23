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
    /// Represents a drawable circle.
    /// </summary>
    public class Circle : Component
    {
        private bool _fill;

        /// <summary>
        /// Gets or sets whether the LogiFrame.Components.Square should be filled.
        /// </summary>
        public bool Fill
        {
            get { return _fill; }
            set
            {
                if (SwapProperty(ref _fill, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            Bytemap result = new Bytemap(Size);

            double hradius = (double) Size.Width/2;
            double vradius = (double) Size.Height/2;

            for (double i = 0.0; i < 360.0; i += 0.1)
            {
                double angle = i*(Math.PI/180);
                int x = (int) Math.Floor(hradius + (hradius - 1)*Math.Cos(angle));
                int y = (int) Math.Floor(vradius + (vradius - 1)*Math.Sin(angle));

                result.SetPixel(x, y, true);
            }

            return result;
        }
    }
}