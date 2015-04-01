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

using System.ComponentModel;
using System.Drawing;
using LogiFrame.Tools;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a location of a component.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public struct Location
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Location" /> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public Location(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Get the x-coordinate.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        ///     Gets the y-coordinate.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="loc1">The loc1.</param>
        /// <param name="loc2">The loc2.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Location operator -(Location loc1, Location loc2)
        {
            return new Location(loc1.X - loc2.X, loc1.Y - loc2.Y);
        }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Location operator -(Location loc, Size size)
        {
            return new Location(loc.X - size.Width, loc.Y - size.Height);
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="loc1">The loc1.</param>
        /// <param name="loc2">The loc2.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Location operator +(Location loc1, Location loc2)
        {
            return new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Location operator +(Location loc, Size size)
        {
            return new Location(loc.X + size.Width, loc.Y + size.Height);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Location" /> to <see cref="Point" />.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Point(Location loc)
        {
            return new Point(loc.X, loc.Y);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Point" /> to <see cref="Location" />.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Location(Point point)
        {
            return new Location(point.X, point.Y);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37;
                result *= 397;
                result += X;
                result *= 397;
                result += Y;
                return result;
            }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}