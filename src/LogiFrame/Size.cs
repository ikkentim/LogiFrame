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

namespace LogiFrame
{
    /// <summary>
    ///     Represents the size of a <see cref="Component" />.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public struct Size
    {
        public Size(int width, int height) : this()
        {
            if (width < 0 || height < 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be at least 0.");
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Gets the width.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the height.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="size1">The size1.</param>
        /// <param name="size2">The size2.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Size operator -(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="size1">The size1.</param>
        /// <param name="size2">The size2.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static Size operator +(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Size" /> to <see cref="System.Drawing.Size" />.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Drawing.Size" /> to <see cref="Size" />.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
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
                result += Width;
                result *= 397;
                result += Height;
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
            return "(" + Width + ", " + Height + ")";
        }
    }
}