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

namespace LogiFrame
{
    /// <summary>
    /// Represents a single-color image in a bytearray
    /// </summary>
    public class Bytemap
    {
        #region Fields

        private int _height;
        private Size _size;
        private int _width;

        #endregion

        #region Statics

        /// <summary>
        /// Represents an empty Bytemap.
        /// </summary>
        public static readonly Bytemap Empty = null;

        #endregion

        #region Factory

        /// <summary>
        /// Transform a System.Drawing.Bitmap into a LogiFrame.Bytemap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to transform.</param>
        /// <param name="conversionMethod">The LogiFrame.ConversionMethod to use during the transformation.</param>
        public static Bytemap FromBitmap(System.Drawing.Bitmap bitmap, ConversionMethod conversionMethod)
        {
            return FromBitmap(bitmap, conversionMethod.MaxRed, conversionMethod.MaxGreen, conversionMethod.MaxGreen,
                conversionMethod.MinAlpha);
        }

        /// <summary>
        /// Transform a System.Drawing.Bitmap into a LogiFrame.Bytemap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to transform.</param>
        /// <param name="maxR">The maximum red color value for a pixel to be filled.</param>
        /// <param name="maxG">The maximum green color value for a pixel to be filled.</param>
        /// <param name="maxB">The maximum blue color value for a pixel to be filled.</param>
        /// <param name="minA">The minimum alpha value for a pixel to be filled.</param>
        /// <returns>The new LogiFrame.Bytemap that this method creates. </returns>
        public static Bytemap FromBitmap(System.Drawing.Bitmap bitmap, byte maxR = 0, byte maxG = 0, byte maxB = 0,
            byte minA = 255)
        {
            if (bitmap == null)
                return null;

            Bytemap result = new Bytemap(bitmap.Size);

            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    System.Drawing.Color px = bitmap.GetPixel(x, y);
                    result.Data[result._width*y + x] =
                        (byte) (px.R <= maxR && px.G <= maxG && px.B <= maxB && minA <= px.A ? 0xff : 0x00);
                }
            return result;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Bytemap class.
        /// </summary>
        /// <param name="width">Initial width of the bytemap.</param>
        /// <param name="height">Initial height of the bytemap.</param>
        public Bytemap(int width, int height)
        {
            Size = new Size(width, height);
        }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Bytemap class.
        /// </summary>
        /// <param name="size">Initial size of the bytemap.</param>
        public Bytemap(Size size)
        {
            Size = size;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the byte[] array container all the date of the canvas.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets or sets whether the non-filled pixels should draw the lower-located
        /// pixels when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool Transparent { get; set; }

        /// <summary>
        /// Gets or sets whether pixels around the filled pixels should always be non-filled
        /// when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool TopEffect { get; set; }


        /// <summary>
        /// Gets or sets the LogiFrame.Size of this LogiFrame.Bytemap.
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

        #endregion

        #region Methods

        /// <summary>
        /// Creates a copy of LogiFrame.Bytemap instance.
        /// </summary>
        /// <returns>The new LogiFrame.Bytemap that this method creates.</returns>
        public Bytemap Clone()
        {
            Bytemap result = new Bytemap(Size);
            Array.Copy(Data, result.Data, Data.Length);

            return result;
        }

        /// <summary>
        /// Sets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="location">The location of the pixel to set.</param>
        /// <param name="fill">Whether the pixel should be filled.</param>
        public void SetPixel(Location location, bool fill)
        {
            SetPixel(location.X, location.Y, fill);
        }

        /// <summary>
        /// Sets the content of the specified pixel in this LogiFrame.Bytemap.
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
        /// Gets the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="location">The location of the pixel to get.</param>
        /// <returns>Whether the pixel is filled</returns>
        public bool GetPixel(Location location)
        {
            return GetPixel(location.X, location.Y);
        }

        /// <summary>
        /// Gets the content of the specified pixel in this LogiFrame.Bytemap.
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
        /// Merges the given LogiFrame.Bytemap into this LogiFrame.Bytemap at the specified LogiFrame.Location.
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
                        if (bytemap.GetPixel(sx, sy))
                        {
                            SetPixel(x, y, true);

                            if (x > 0 && (sx == 0 || !bytemap.GetPixel(sx - 1, sy)))
                                SetPixel(x - 1, y, false);

                            if (x < _width - 1 && (sx == bytemap._width - 1 || !bytemap.GetPixel(sx + 1, sy)))
                                SetPixel(x + 1, y, false);

                            if (y > 0 && (sy == 0 || !bytemap.GetPixel(sx, sy - 1)))
                                SetPixel(x, y - 1, false);

                            if (y < _width - 1 && (sy == bytemap._height - 1 || !bytemap.GetPixel(sx, sy + 1)))
                                SetPixel(x, y + 1, false);
                        }
                    }
                    else if (bytemap.Transparent)
                        SetPixel(x, y, bytemap.GetPixel(sx, sy) || GetPixel(x, y));
                    else
                        SetPixel(x, y, bytemap.GetPixel(sx, sy));
                }
        }

        /// <summary>
        /// Converts the specified LogiFrame.Bytemap instance to a System.Drawing.Bitmap instance.
        /// </summary>
        /// <param name="bytemap">The LogiFrame.Bytemap to be converted.</param>
        /// <returns>The System.Drawing.Bitmap that results from the conversion.</returns>
        public static implicit operator System.Drawing.Bitmap(Bytemap bytemap)
        {
            if (bytemap == null)
                return null;

            System.Drawing.Bitmap result = new System.Drawing.Bitmap(bytemap._width, bytemap._height);
            System.Drawing.Graphics.FromImage(result).Clear(System.Drawing.Color.White);
            for (int y = 0; y < bytemap._height; y++)
                for (int x = 0; x < bytemap._width; x++)
                    if (bytemap.Data[x + y*bytemap._width] == 255)
                        result.SetPixel(x, y, System.Drawing.Color.Black);

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Resizes the byte array according to the Size.
        /// </summary>
        private void Resize()
        {
            if (_width != Size.Width || _height != Size.Height)
            {
                byte[] newData = new byte[Size.Width*Size.Height];

                for (int y = 0; y < Math.Min(Size.Height, _height); y++)
                    for (int x = 0; x < Math.Min(Size.Width, _width); x++)
                    {
                        newData[x + y*Size.Width] = Data[x + y*_width];
                    }


                Data = newData;

                _width = Size.Width;
                _height = Size.Height;
            }
        }

        /// <summary>
        /// Listener for Size.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void size_Changed(object sender, EventArgs e)
        {
            Resize();
        }

        #endregion
    }
}