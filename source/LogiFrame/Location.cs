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
using System.ComponentModel;
using System.Drawing;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a location of a component.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public class Location
    {
        private int _x;
        private int _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="parent">An instance of Location to copy the coordinates from.</param>
        public Location(Location parent)
        {
            _x = parent.X;
            _y = parent.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="x">The initial x-coordinate.</param>
        /// <param name="y">The initial x-coordinate.</param>
        public Location(int x, int y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        ///     Occurs when the location has been changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Gets or sets the x-coordinate.
        /// </summary>
        public int X
        {
            get { return _x; }
            set { Set(value, _y); }
        }

        /// <summary>
        /// Gets or sets the y-coordinate.
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { Set(_x, value); }
        }

        /// <summary>
        ///     Adds a specified <paramref name="x"/> and <paramref name="y"/> values to this instance.
        /// </summary>
        /// <param name="x">Value to add to the x-coordinate.</param>
        /// <param name="y">Value to add to the y-coordinate.</param>
        public void Add(int x, int y)
        {
            bool changed = x != 0 || y != 0;
            _x += x;
            _y += y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds the specified <paramref name="value"/> to this instance.
        /// </summary>
        /// <param name="value">Value to add to the x- and y-coordinate.</param>
        public void Add(Location value)
        {
            bool changed = value.X != 0 || value.Y != 0;
            _x += value.X;
            _y += value.Y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Set the specified <paramref name="x"/> and <paramref name="y"/> values to this instance.
        /// </summary>
        /// <param name="x">The new x-coordinate value.</param>
        /// <param name="y">The new y-coordinate value.</param>
        public void Set(int x, int y)
        {
            bool changed = _x != x || _y != y;
            _x = x;
            _y = y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Set a certain value to this Location instance.
        /// </summary>
        /// <param name="other">An instance of Location to copy the coordinates from.</param>
        public void Set(Location other)
        {
            bool changed = _x != other._x || _y != other._y;
            _x = other.X;
            _y = other.Y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="loc1">The loc1.</param>
        /// <param name="loc2">The loc2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Location operator -(Location loc1, Location loc2)
        {
            return new Location(loc1.X - loc2.X, loc1.Y - loc2.Y);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="loc1">The loc1.</param>
        /// <param name="loc2">The loc2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Location operator +(Location loc1, Location loc2)
        {
            return new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Location operator +(Location loc, Size size)
        {
            return new Location(loc.X + size.Width, loc.Y + size.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Location"/> to <see cref="Point"/>.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Point(Location loc)
        {
            return new Point(loc.X, loc.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Point"/> to <see cref="Location"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Location(Point point)
        {
            return new Location(point.X, point.Y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}