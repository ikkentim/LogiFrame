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
using LogiFrame.Tools;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a single-color image.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public struct Snapshot
    {
        /// <summary>
        /// An empty snapshot.
        /// </summary>
        public static readonly Snapshot Empty = new Snapshot();

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Snapshot(int width, int height) : this()
        {
            Size = new Size(width, height);
            Data = new byte[width*height];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot"/> struct.
        /// </summary>
        /// <param name="size">The size.</param>
        public Snapshot(Size size) : this()
        {
            Size = size;
            Data = new byte[size.Width*size.Height];
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is transparent.
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is top effect.
        /// </summary>
        public bool IsTopEffect { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Data == null; }
        }

        /// <summary>
        /// Creates a <see cref="Snapshot" /> from the specified <paramref name="bitmap" />.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="conversionMethod">The conversion method.</param>
        /// <returns>The created snapshot.</returns>
        public static Snapshot FromBitmap(Bitmap bitmap, ConversionMethod conversionMethod)
        {
            return FromBitmap(bitmap, conversionMethod.MaxRed, conversionMethod.MaxGreen, conversionMethod.MaxGreen,
                conversionMethod.MinAlpha);
        }

        /// <summary>
        /// Creates a <see cref="Snapshot" /> from the specified <paramref name="bitmap" />.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="maxR">The maximum red.</param>
        /// <param name="maxG">The maximum green.</param>
        /// <param name="maxB">The maximum blue.</param>
        /// <param name="minA">The minimum alpha.</param>
        /// <returns>The created snapshot.</returns>
        public static Snapshot FromBitmap(Bitmap bitmap, byte maxR = 0, byte maxG = 0, byte maxB = 0,
            byte minA = 255)
        {
            if (bitmap == null)
                return Empty;

            lock (bitmap)
            {
                var result = new Snapshot(bitmap.Size);

                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color px = bitmap.GetPixel(x, y);
                        result.Data[result.Size.Width*y + x] =
                            (byte) (px.R <= maxR && px.G <= maxG && px.B <= maxB && minA <= px.A ? 0xff : 0x00);
                    }
                return result;
            }
        }

        /// <summary>
        /// Sets the pixel.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="fill">if set to <c>true</c> [fill].</param>
        public void SetPixel(Location location, bool fill)
        {
            SetPixel(location.X, location.Y, fill);
        }

        /// <summary>
        /// Sets the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="fill">if set to <c>true</c> fill, otherwise empty.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// x
        /// or
        /// y
        /// </exception>
        public void SetPixel(int x, int y, bool fill)
        {
            if (x < 0 || x >= Size.Width)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= Size.Height)
                throw new ArgumentOutOfRangeException("y");

            Data[x + y*Size.Width] = fill ? (byte) 0xff : (byte) 0x00;
        }

        /// <summary>
        /// Gets the pixel.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>True if the pixel is filled; False otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The given position is not within the boundaries of the Snapshot.</exception>
        public bool GetPixel(Location location)
        {
            return GetPixel(location.X, location.Y);
        }

        /// <summary>
        /// Gets the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>True if the pixel is filled; False otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The given position is not within the boundaries of the Snapshot.</exception>
        public bool GetPixel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Size.Width || y >= Size.Height)
                throw new ArgumentOutOfRangeException("The given position is not within the boundaries of the Snapshot.");

            return Data[x + y*Size.Width] == 0xff;
        }

        /// <summary>
        /// Merges the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="location">The location.</param>
        public void Merge(Snapshot snapshot, Location location)
        {
            if (snapshot.IsEmpty)
                return;

            /* Test range.
             */
            if (location.X + snapshot.Size.Width < 0 ||
                location.Y + snapshot.Size.Height < 0 ||
                location.X >= Size.Width ||
                location.Y >= Size.Height)
                return;

            for (int x = Math.Max(location.X, 0); x < Math.Min(Size.Width, location.X + snapshot.Size.Width); x++)
                for (int y = Math.Max(location.Y, 0); y < Math.Min(Size.Height, location.Y + snapshot.Size.Height); y++)
                {
                    int sx = x - location.X;
                    int sy = y - location.Y;
                    if (snapshot.IsTransparent && snapshot.IsTopEffect)
                    {
                        if (!snapshot.GetPixel(sx, sy)) continue;

                        SetPixel(x, y, true);

                        if (x > 0 && (sx == 0 || !snapshot.GetPixel(sx - 1, sy))) SetPixel(x - 1, y, false);
                        if (y > 0 && (sy == 0 || !snapshot.GetPixel(sx, sy - 1))) SetPixel(x, y - 1, false);
                        if (x < Size.Width - 1 && (sx == snapshot.Size.Width - 1 || !snapshot.GetPixel(sx + 1, sy)))
                            SetPixel(x + 1, y, false);
                        if (y < Size.Height - 1 && (sy == snapshot.Size.Height - 1 || !snapshot.GetPixel(sx, sy + 1)))
                            SetPixel(x, y + 1, false);
                    }
                    else if (!snapshot.IsTransparent && snapshot.IsTopEffect)
                    {
                        SetPixel(x, y, snapshot.GetPixel(sx, sy));

                        if (sx == 0 && x > 0) SetPixel(x - 1, y, false);
                        if (sy == 0 && y > 0) SetPixel(x, y - 1, false);
                        if (sx == snapshot.Size.Width - 1 && x < Size.Width - 1) SetPixel(x + 1, y, false);
                        if (sy == snapshot.Size.Height - 1 && y < Size.Height - 1) SetPixel(x, y + 1, false);
                    }
                    else if (snapshot.IsTransparent && !snapshot.IsTopEffect)
                        SetPixel(x, y, snapshot.GetPixel(sx, sy) || GetPixel(x, y));
                    else
                        SetPixel(x, y, snapshot.GetPixel(sx, sy));
                }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Snapshot"/> to <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Bitmap(Snapshot snapshot)
        {
            if (snapshot.IsEmpty)
                return null;

            var result = new Bitmap(snapshot.Size.Width, snapshot.Size.Height);
            Graphics.FromImage(result).Clear(Color.White);
            for (int y = 0; y < snapshot.Size.Height; y++)
                for (int x = 0; x < snapshot.Size.Width; x++)
                    if (snapshot.Data[x + y*snapshot.Size.Width] == 255)
                        result.SetPixel(x, y, Color.Black);

            return result;
        }
    }
}