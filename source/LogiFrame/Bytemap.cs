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
    ///     Represents a single-color image in a byte array.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public class Bytemap
    {
        private int _height;
        private Size _size;
        private int _width;

        /// <summary>
        ///     Represents an empty Bytemap.
        /// </summary>
        public static readonly Bytemap Empty = null;

        /// <summary>
        ///     Transform a System.Drawing.Bitmap into a LogiFrame.Bytemap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to transform.</param>
        /// <param name="conversionMethod">The LogiFrame.ConversionMethod to use during the transformation.</param>
        public static Bytemap FromBitmap(Bitmap bitmap, ConversionMethod conversionMethod)
        {
            return FromBitmap(bitmap, conversionMethod.MaxRed, conversionMethod.MaxGreen, conversionMethod.MaxGreen,
                conversionMethod.MinAlpha);
        }

        /// <summary>
        ///     Transform a System.Drawing.Bitmap into a LogiFrame.Bytemap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to transform.</param>
        /// <param name="maxR">The maximum red color value for a pixel to be filled.</param>
        /// <param name="maxG">The maximum green color value for a pixel to be filled.</param>
        /// <param name="maxB">The maximum blue color value for a pixel to be filled.</param>
        /// <param name="minA">The minimum alpha value for a pixel to be filled.</param>
        /// <returns>The new LogiFrame.Bytemap that this method creates. </returns>
        public static Bytemap FromBitmap(Bitmap bitmap, byte maxR = 0, byte maxG = 0, byte maxB = 0,
            byte minA = 255)
        {
            if (bitmap == null)
                return null;
            lock (bitmap)
            {
                var result = new Bytemap(bitmap.Size);

                for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color px = bitmap.GetPixel(x, y);
                        result.Data[result._width*y + x] =
                            (byte) (px.R <= maxR && px.G <= maxG && px.B <= maxB && minA <= px.A ? 0xff : 0x00);
                    }
                return result;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Bytemap class.
        /// </summary>
        /// <param name="width">Initial width of the bytemap.</param>
        /// <param name="height">Initial height of the bytemap.</param>
        public Bytemap(int width, int height)
        {
            Size = new Size(width, height);
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Bytemap class.
        /// </summary>
        /// <param name="size">Initial size of the bytemap.</param>
        public Bytemap(Size size)
        {
            Size = size;
        }

        /// <summary>
        ///     Gets the byte[] array container all the date of the canvas.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        ///     Gets or sets whether the non-filled pixels should draw the lower-located
        ///     pixels when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool Transparent { get; set; }

        /// <summary>
        ///     Gets or sets whether pixels around the filled pixels should always be non-filled
        ///     when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool TopEffect { get; set; }


        /// <summary>
        ///     Gets or sets the LogiFrame.Size of this LogiFrame.Bytemap.
        /// </summary>
        public Size Size
        {
            get { return _size; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Bytemap.Size cannot be set to null.");

                if (_size == null)
                {
                    _width = value.Width;
                    _height = value.Height;
                    Data = new byte[_width*_height];
                }
                else
                {
                    _size.Changed -= size_Changed;
                }

                _size = value;
                _size.Changed += size_Changed;

                Resize();
            }
        }

        /// <summary>
        ///     Creates a copy of LogiFrame.Bytemap instance.
        /// </summary>
        /// <returns>The new LogiFrame.Bytemap that this method creates.</returns>
        public Bytemap Clone()
        {
            var result = new Bytemap(Size);
            Array.Copy(Data, result.Data, Data.Length);

            return result;
        }

        /// <summary>
        ///     Sets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="location">The location of the pixel to set.</param>
        /// <param name="fill">Whether the pixel should be filled.</param>
        public void SetPixel(Location location, bool fill)
        {
            SetPixel(location.X, location.Y, fill);
        }

        /// <summary>
        ///     Sets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="fill">Whether the pixel should be filled.</param>
        public void SetPixel(int x, int y, bool fill)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                throw new ArgumentOutOfRangeException("The given position is not within the boundaries of the Bytemap.");

            Data[x + y*_width] = fill ? (byte) 0xff : (byte) 0x00;
        }

        /// <summary>
        ///     Gets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="location">The location of the pixel to get.</param>
        /// <returns>Whether the pixel is filled</returns>
        public bool GetPixel(Location location)
        {
            return GetPixel(location.X, location.Y);
        }

        /// <summary>
        ///     Gets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to get.</param>
        /// <param name="y">The y-coordinate of the pixel to get.</param>
        /// <returns>Whether the pixel is filled</returns>
        public bool GetPixel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                throw new ArgumentOutOfRangeException("The given position is not within the boundaries of the Bytemap.");

            return Data[x + y*_width] == 0xff;
        }

        /// <summary>
        ///     Merges the given LogiFrame.Bytemap into this LogiFrame.Bytemap at the specified LogiFrame.Location.
        /// </summary>
        /// <param name="bytemap">The LogiFrame.Bytemap to merge into this LogiFrame.Bytemap.</param>
        /// <param name="location">The LogiFrame.Location to merge the LogiFrame.Bytemap at.</param>
        public void Merge(Bytemap bytemap, Location location)
        {
            if (bytemap == Empty)
                return;

            if (location == null)
                throw new ArgumentNullException("location cannot be null.");

            //Out of range test
            if (location.X + bytemap._width < 0 ||
                location.Y + bytemap._height < 0 ||
                location.X >= _width ||
                location.Y >= _height)
                return;

            for (int x = Math.Max(location.X, 0); x < Math.Min(_width, location.X + bytemap._width); x++)
                for (int y = Math.Max(location.Y, 0); y < Math.Min(_height, location.Y + bytemap._height); y++)
                {
                    int sx = x - location.X;
                    int sy = y - location.Y;
                    if (bytemap.Transparent && bytemap.TopEffect)
                    {
                        if (!bytemap.GetPixel(sx, sy)) continue;

                        SetPixel(x, y, true);

                        if (x > 0 && (sx == 0 || !bytemap.GetPixel(sx - 1, sy))) SetPixel(x - 1, y, false);
                        if (y > 0 && (sy == 0 || !bytemap.GetPixel(sx, sy - 1))) SetPixel(x, y - 1, false);
                        if (x < _width - 1 && (sx == bytemap._width - 1 || !bytemap.GetPixel(sx + 1, sy)))
                            SetPixel(x + 1, y, false);
                        if (y < _height - 1 && (sy == bytemap._height - 1 || !bytemap.GetPixel(sx, sy + 1)))
                            SetPixel(x, y + 1, false);
                    }
                    else if (!bytemap.Transparent && bytemap.TopEffect)
                    {
                        SetPixel(x, y, bytemap.GetPixel(sx, sy));

                        if (sx == 0 && x > 0) SetPixel(x - 1, y, false);
                        if (sy == 0 && y > 0) SetPixel(x, y - 1, false);
                        if (sx == bytemap._width - 1 && x < _width - 1) SetPixel(x + 1, y, false);
                        if (sy == bytemap._height - 1 && y < _height - 1) SetPixel(x, y + 1, false);
                    }
                    else if (bytemap.Transparent && !bytemap.TopEffect)
                        SetPixel(x, y, bytemap.GetPixel(sx, sy) || GetPixel(x, y));
                    else
                        SetPixel(x, y, bytemap.GetPixel(sx, sy));
                }
        }

        /// <summary>
        ///     Converts the specified <see cref="Bytemap"/> instance to a System.Drawing.Bitmap instance.
        /// </summary>
        /// <param name="bytemap">The <see cref="Bytemap"/> to be converted.</param>
        /// <returns>The <see cref="Bitmap"/> that results from the conversion.</returns>
        public static implicit operator Bitmap(Bytemap bytemap)
        {
            if (bytemap == null)
                return null;

            var result = new Bitmap(bytemap._width, bytemap._height);
            Graphics.FromImage(result).Clear(Color.White);
            for (int y = 0; y < bytemap._height; y++)
                for (int x = 0; x < bytemap._width; x++)
                    if (bytemap.Data[x + y*bytemap._width] == 255)
                        result.SetPixel(x, y, Color.Black);

            return result;
        }

        private void Resize()
        {
            if (_width == Size.Width && _height == Size.Height) return;
            var newData = new byte[Size.Width*Size.Height];

            for (int y = 0; y < Math.Min(Size.Height, _height); y++)
                for (int x = 0; x < Math.Min(Size.Width, _width); x++)
                    newData[x + y*Size.Width] = Data[x + y*_width];

            Data = newData;

            _width = Size.Width;
            _height = Size.Height;
        }

        private void size_Changed(object sender, EventArgs e)
        {
            Resize();
        }
    }
}