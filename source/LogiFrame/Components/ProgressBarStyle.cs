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

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a Style of drawing a LogiFrame.Components.ProgressBar.
    /// </summary>
    public enum ProgressBarStyle
    {
        /// <summary>
        /// A drawing type where there is no border around the ProgressBar.
        /// </summary>
        NoBorder,

        /// <summary>
        /// A drawing type where there is a border around the ProgressBar.
        /// </summary>
        Border,

        /// <summary>
        /// A drawing type where there is a border around the ProgressBar,
        /// and a single pixel of whitespace between the border in the inner bar.
        /// </summary>
        WhiteSpacedBorder
    }
}