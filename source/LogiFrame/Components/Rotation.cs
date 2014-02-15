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
    /// Represents modifications which can be set to the Rotation property of LogiFrame.Components.Rotator.
    /// </summary>
    [Flags]
    public enum Rotation
    {
        /// <summary>
        /// Rotates the container 0 degrees.
        /// </summary>
        Rotate0Degrees = 0,

        /// <summary>
        /// Rotates the container 90 degrees.
        /// </summary>
        Rotate90Degrees = 1,

        /// <summary>
        /// Rotates the container 180 degrees.
        /// </summary>
        Rotate180Degrees = 2,

        /// <summary>
        /// Rotates the container 270 degrees.
        /// </summary>
        Rotate270Degrees = 4,

        /// <summary>
        /// Flips the container horizontally.
        /// </summary>
        FlipHorizontal = 8,

        /// <summary>
        /// Flips the container vertically.
        /// </summary>
        FlipVertical = 16
    }
}