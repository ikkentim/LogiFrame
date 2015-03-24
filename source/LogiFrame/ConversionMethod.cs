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

using System.Drawing;

namespace LogiFrame
{
    /// <summary>
    ///     Represents the technique used for converting a <see cref="Bitmap"/>
    ///     into a <see cref="Bytemap"/>.
    /// </summary>
    public struct ConversionMethod
    {
        /// <summary>
        ///     Represents the default conversion type.
        /// </summary>
        public static readonly ConversionMethod Normal = new ConversionMethod(0, 0, 0, 255);

        /// <summary>
        ///     Represents a conversion where pixels with RGR values of 0-64 and A value of 255 are filled.
        /// </summary>
        public static readonly ConversionMethod QuarterByte = new ConversionMethod(64, 64, 64, 255);

        /// <summary>
        ///     Represents a conversion where pixels with RGR values of 0-254 and A value of 255 are filled.
        /// </summary>
        public static readonly ConversionMethod NonWhite = new ConversionMethod(254, 254, 254, 255);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionMethod"/> struct.
        /// </summary>
        /// <param name="maxRed">The maximum red color value for a pixel to be filled.</param>
        /// <param name="maxGreen">The maximum green color value for a pixel to be filled.</param>
        /// <param name="maxBlue">The maximum blue color value for a pixel to be filled.</param>
        /// <param name="minAlpha">The minimum alpha color value for a pixel to be filled.</param>
        public ConversionMethod(byte maxRed, byte maxGreen, byte maxBlue, byte minAlpha)
            : this()
        {
            MaxRed = maxRed;
            MaxGreen = maxGreen;
            MaxBlue = maxBlue;
            MinAlpha = minAlpha;
        }

        /// <summary>
        ///     Gets or sets the maximum red color value for a pixel to be filled.
        /// </summary>
        public byte MaxRed { get; set; }

        /// <summary>
        ///     Gets or sets the maximum green color value for a pixel to be filled.
        /// </summary>
        public byte MaxGreen { get; set; }

        /// <summary>
        ///     Gets or sets the maximum blue color value for a pixel to be filled.
        /// </summary>
        public byte MaxBlue { get; set; }

        /// <summary>
        ///     Gets or sets the minimum alpha color value for a pixel to be filled.
        /// </summary>
        public byte MinAlpha { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj is ConversionMethod == false)
                return false;

            var other = (ConversionMethod) obj;

            return other.MaxBlue == MaxBlue &&
                   other.MaxGreen == MaxGreen &&
                   other.MaxRed == MaxRed &&
                   other.MinAlpha == MinAlpha;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ConversionMethod left, ConversionMethod right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ConversionMethod left, ConversionMethod right)
        {
            return left.Equals(right) == false;
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
                result += MaxRed;
                result *= 397;
                result += MaxGreen;
                result *= 397;
                result += MaxBlue;
                result *= 397;
                result += MinAlpha;
                return result;
            }
        }
    }
}