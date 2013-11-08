// Location.cs
// 
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

using System;
using System.ComponentModel;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a location of a component.
    /// </summary>
    [TypeConverter(typeof (LocationTypeConverter))]
    public class Location
    {
        #region Fields

        private int _x;
        private int _y;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="parent">An instance of LogiFrame.Location to copy the coordinates from.</param>
        public Location(Location parent)
        {
            _x = parent.X;
            _y = parent.Y;
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="x">The initial x-coordinate.</param>
        /// <param name="y">The initial x-coordinate.</param>
        public Location(int x, int y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        public Location()
        {
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the location has been changed.
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x-coordinate of the LogiFrame.Location.
        /// </summary>
        public int X
        {
            get { return _x; }
            set { Set(value, _y); }
        }

        /// <summary>
        ///     Gets or sets the y-coordinate of the LogiFrame.Location.
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { Set(_x, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds a certain value to this LogiFrame.Location instance.
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
        ///     Adds a certain value to this LogiFrame.Location instance.
        /// </summary>
        /// <param name="other">Value to add to the x- and y-coordinate.</param>
        public void Add(Location other)
        {
            bool changed = other.X != 0 || other.Y != 0;
            _x += other.X;
            _y += other.Y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Set a certain value to this LogiFrame.Location instance.
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
        ///     Set a certain value to this LogiFrame.Location instance.
        /// </summary>
        /// <param name="other">An instance of LogiFrame.Location to copy the coordinates from.</param>
        public void Set(Location other)
        {
            bool changed = _x != other._x || _y != other._y;
            _x = other.X;
            _y = other.Y;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Translates a LogiFrame.Location by a given LogiFrame.Location.
        /// </summary>
        /// <param name="loc1">The LogiFrame.Location to translate.</param>
        /// <param name="loc2">
        ///     A LogiFrame.Location that specifies the pair of numbers to subtract from
        ///     the coordinates of loc1.
        /// </param>
        /// <returns>
        ///     A LogiFrame.Location instance that is translated by the negative of the
        ///     other given LogiFrame.Location instance.
        /// </returns>
        public static Location operator -(Location loc1, Location loc2)
        {
            return new Location(loc1.X - loc2.X, loc1.Y - loc2.Y);
        }

        /// <summary>
        ///     Translates a LogiFrame.Location by a given LogiFrame.Location.
        /// </summary>
        /// <param name="loc1">The LogiFrame.Location to translate.</param>
        /// <param name="loc2">
        ///     A LogiFrame.Location that specifies the pair of numbers to add to
        ///     the coordinates of loc1.
        /// </param>
        /// <returns>
        ///     A LogiFrame.Location instance that is translated by the other
        ///     given LogiFrame.Location instance.
        /// </returns>
        public static Location operator +(Location loc1, Location loc2)
        {
            return new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);
        }

        /// <summary>
        ///     Translates a LogiFrame.Location by the dimentions of a LogiFrame.Size.
        /// </summary>
        /// <param name="loc">The LogiFrame.Location to translate.</param>
        /// <param name="size">
        ///     The LogiFrame.Size that specifies the pair of numers to add to
        ///     the coordinates of loc.
        /// </param>
        /// <returns>
        ///     A LogiFrame.Location instance that is translated by the given
        ///     LogiFrame.Size instance.
        /// </returns>
        public static Location operator +(Location loc, Size size)
        {
            return new Location(loc.X + size.Width, loc.Y + size.Height);
        }

        /// <summary>
        ///     Converts the specified LogiFrame.Location instance to a System.Drawing.Point struct.
        /// </summary>
        /// <param name="loc">The LogiFrame.Location to be converted.</param>
        /// <returns>The System.Drawing.Point that results from the conversion.</returns>
        public static implicit operator System.Drawing.Point(Location loc)
        {
            return new System.Drawing.Point(loc.X, loc.Y);
        }

        /// <summary>
        ///     Converts the specified System.Drawing.Point struct to a LogiFrame.Location instance.
        /// </summary>
        /// <param name="point">The System.Drawing.Point to be converted.</param>
        /// <returns>The LogiFrame.Location that results from the conversion.</returns>
        public static implicit operator Location(System.Drawing.Point point)
        {
            return new Location(point.X, point.Y);
        }

        /// <summary>
        ///     Determines whether the specified System.Object is equal to this LogiFrame.Location.
        /// </summary>
        /// <param name="obj">The System.Object to compare with this LogiFrame.Location.</param>
        /// <returns>
        ///     true if the specified System.Object is equal to this LogiFrame.Location;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Location == false)
                return false;

            Location other = obj as Location;
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        ///     Returns a hash code for this LogiFrame.Location.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this LogiFrame.Location.</returns>
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
        ///     Converts this LogiFrame.Location to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this LogiFrame.Location.</returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        #endregion
    }
}