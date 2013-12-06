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

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable square.
    /// </summary>
    public class Square : Component
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
                if (_fill == value)
                    return;

                _fill = value;
                HasChanged = true;
            }
        }

        protected override Bytemap Render()
        {
            Bytemap result = new Bytemap(Size);

            if (Fill)
            {
                for (int x = 0; x < Size.Width; x++)
                    for (int y = 0; y < Size.Height; y++)
                        result.SetPixel(x, y, true);
            }
            else
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    result.SetPixel(x, 0, true); //Top
                    result.SetPixel(x, Size.Height - 1, true); //Bottom
                }
                for (int y = 0; y < Size.Height; y++)
                {
                    result.SetPixel(0, y, true); //Left
                    result.SetPixel(Size.Width - 1, y, true); //Right
                }
            }
            return result;
        }
    }
}