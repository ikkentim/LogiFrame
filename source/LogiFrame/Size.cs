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
using LogiFrame.Tools;
//
namespace LogiFrame
{
    /// <summary>
    ///     Represents the size of a <see cref="Component"/>.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public class Size
    {
        #region Fields

        private int _height;
        private int _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        public Size()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        /// <param name="size">An instance of <see cref="Size"/> to copy the dimentions from.</param>
        public Size(Size size)
        {
            _width = size.Width;
            _height = size.Height;
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
            _width = width;
            _height = height;
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the <see cref="Size"/> has been changed.
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { Set(value, _height); }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>t.
        /// </value>
        public int Height
        {
            get { return _height; }
            set { Set(_width, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified <paramref name="width"/> and <paramref name="height"/> values to this instance.
        /// </summary>
        /// <param name="width">Value to add to the width.</param>
        /// <param name="height">Value to add to the height</param>
        public void Add(int width, int height)
        {
            bool changed = width != 0 || height != 0;

            if (_width + width < 0 || _height + height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");

            _width += width;
            _height += height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Adds a the specified <paramref name="value"/> to this instance.
        /// </summary>
        /// <param name="value">Value to add to the width and height.</param>
        public void Add(Size value)
        {
            bool changed = value.Width != 0 || value.Height != 0;
            _width += value.Width;
            _height += value.Height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Sets this instance to the specified <paramref name="width"/> and <paramref name="height"/> value.
        /// </summary>
        /// <param name="width">The new width value.</param>
        /// <param name="height">The new height value.</param>
        public void Set(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");

            bool changed = _width != width || _height != height;
            _width = width;
            _height = height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Sets this instance to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An instance of Size to copy the dimensions from.</param>
        public void Set(Size value)
        {
            bool changed = Width != value.Width || Height != value.Height;
            _width = value.Width;
            _height = value.Height;

            if (changed && Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="size1">The size1.</param>
        /// <param name="size2">The size2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator -(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="size1">The size1.</param>
        /// <param name="size2">The size2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Size operator +(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Size"/> to <see cref="System.Drawing.Size"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.Size"/> to <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
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
                result += Width;
                result *= 397;
                result += Height;
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
            return "(" + Width + ", " + Height + ")";
        }

        #endregion
    }
}