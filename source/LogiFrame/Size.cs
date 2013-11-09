// Size.cs
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
    ///     Represents the size of a component.
    /// </summary>
    [TypeConverter(typeof (SizeTypeConverter))]
    public class Size
    {
        #region Fields

        private int height;
        private int width;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        public Size()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="parent">An instance of LogiFrame.Size to copy the dimentions from.</param>
        public Size(Size parent)
        {
            width = parent.Width;
            height = parent.Height;
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="width">The initial width.</param>
        /// <param name="height">The initial height.</param>
        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");
            this.width = width;
            this.height = height;
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the Size has been changed.
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the width of the LogiFrame.Size.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { Set(value, height); }
        }

        /// <summary>
        ///     Gets or sets the height of the LogiFrame.Size.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { Set(width, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds a certain value to this LogiFrame.Size instance.
        /// </summary>
        /// <param name="width">Value to add to the width.</param>
        /// <param name="height">Value to add to the height</param>
        public void Add(int width, int height)
        {
            bool changed = width != 0 || height != 0;

            if (this.width + width < 0 || this.height + height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");

            this.width += width;
            this.height += height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds a certain value to this LogiFrame.Size instance.
        /// </summary>
        /// <param name="other">Value to add to the width and height.</param>
        public void Add(Size other)
        {
            bool changed = other.Width != 0 || other.Height != 0;
            width += other.Width;
            height += other.Height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Set a certain value to this LogiFrame.Size instance.
        /// </summary>
        /// <param name="width">The new width value.</param>
        /// <param name="height">The new height value.</param>
        public void Set(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");

            bool changed = this.width != width || this.height != height;
            this.width = width;
            this.height = height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Set a certain value to this LogiFrame.Size instance.
        /// </summary>
        /// <param name="other">An instance of LogiFrame.Size to copy the dimensions from.</param>
        public void Set(Size other)
        {
            bool changed = Width != other.Width || Height != other.Height;
            width = other.Width;
            height = other.Height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Translates a LogiFrame.Size by a given LogiFrame.Size.
        /// </summary>
        /// <param name="size1">The LogiFrame.Size to translate.</param>
        /// <param name="size2">
        ///     A LogiFrame.Size that specifies the pair of numbers to add to
        ///     the coordinates of size1.
        /// </param>
        /// <returns>
        ///     A LogiFrame.Size instance that is translated by the other
        ///     given LogiFrame.Size instance.
        /// </returns>
        public static Size operator -(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }

        /// <summary>
        ///     Translates a LogiFrame.Size by the  of a given LogiFrame.Size.
        /// </summary>
        /// <param name="size1">The LogiFrame.Size to translate.</param>
        /// <param name="size2">
        ///     A LogiFrame.Size that specifies the pair of numbers to subtract from
        ///     the coordinates of loc1.
        /// </param>
        /// <returns>
        ///     A LogiFrame.Size instance that is translated by the negative of the
        ///     other given LogiFrame.Size instance.
        /// </returns>
        public static Size operator +(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        /// <summary>
        ///     Converts the specified LogiFrame.Size instance to a System.Drawing.Size struct.
        /// </summary>
        /// <param name="size">The LogiFrame.Size to be converted.</param>
        /// <returns>The System.Drawing.Size that results from the conversion.</returns>
        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        ///     Converts the specified System.Drawing.Size struct to a LogiFrame.Size instance.
        /// </summary>
        /// <param name="size">The System.Drawing.Size to be converted.</param>
        /// <returns>The LogiFrame.Size that results from the conversion.</returns>
        public static implicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        /// <summary>
        ///     Determines whether the specified System.Object is equal to this LogiFrame.Size.
        /// </summary>
        /// <param name="obj">The System.Object to compare with this LogiFrame.Size.</param>
        /// <returns>
        ///     true if the specified System.Object is equal to this LogiFrame.Size;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Size == false)
                return false;

            Size other = obj as Size;
            return Width == other.Width && Height == other.Height;
        }

        /// <summary>
        ///     Returns a hash code for this LogiFrame.Size.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this LogiFrame.Size.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37;
                result *= 397;
                result += Width;
                result *= 397;
                result += Height;
                return result;
            }
        }

        /// <summary>
        ///     Converts this LogiFrame.Size to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this LogiFrame.Size.</returns>
        public override string ToString()
        {
            return "(" + Width + ", " + Height + ")";
        }

        #endregion
    }
}