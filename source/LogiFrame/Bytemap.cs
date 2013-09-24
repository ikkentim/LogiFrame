using System;

namespace LogiFrame
{
    /// <summary>
    /// Represents a single-color image in a bytearray
    /// </summary>
    public class Bytemap
    {
        public static Bytemap Empty;

        #region Properties
        private byte[] data;

        /// <summary>
        /// The byte[] array container all the date of the canvas.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Whether the non-filled pixels should draw the lower-located
        /// pixels when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool Transparent { get; set; }

        /// <summary>
        /// Whether pixels around the filled pixels should always be non-filled
        /// when using the LogiFrame.Bytemap.Merge method.
        /// </summary>
        public bool TopEffect { get; set; }

        private int width;
        private int height;
        private Size size;

        /// <summary>
        /// The LogiFrame.Size of this LogiFrame.Bytemap.
        /// </summary>
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("LogiFrame.Bytemap.Size cannot be set to null.");

                if (size == null)
                {
                    width = value.Width;
                    height = value.Height;
                    data = new byte[width * height];
                }
                else
                {
                    size.Changed -= new EventHandler(size_SizeChanged);
                }

                size = value;
                size.Changed += new EventHandler(size_SizeChanged);

                resize();
            }
        }
        #endregion

        #region Constructor/Deconstructor
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

        #region Methods
        /// <summary>
        /// Creates a copy of LogiFrame.Bytemap instance.
        /// </summary>
        /// <returns>The new LogiFrame.Bytemap that this method creates.</returns>
        public Bytemap Clone()
        {
            Bytemap result = new Bytemap(Size);
            Array.Copy(data, result.data, data.Length);

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
            if (x < 0 || y < 0 || x >= width || y >= height)
                throw new ArgumentOutOfRangeException("The given position is not within the boundaries of the Bytemap.");

            data[x + y * width] = fill ? (byte)0xff : (byte)0x00;
        }

        /// <summary>
        /// Get the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="location">The location of the pixel to get.</param>
        /// <returns>Whether the pixel is filled</returns>
        public bool GetPixel(Location location)
        {
            return GetPixel(location.X, location.Y);
        }

        /// <summary>
        /// Get the content of the specified pixel in this LogiFrame.Bytemap.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to get.</param>
        /// <param name="y">The y-coordinate of the pixel to get.</param>
        /// <returns>Whether the pixel is filled</returns>
        public bool GetPixel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                throw new ArgumentOutOfRangeException("The given position is not within the boundaries of the Bytemap.");

            return data[x + y * width] == (byte)0xff;
        }

        /// <summary>
        /// Merge the given LogiFrame.Bytemap into the current LogiFrame.Bytemap at the specified location.
        /// </summary>
        /// <param name="bytemap">The LogiFrame.Bytemap to merge into the current LogiFrame.Bytemap.</param>
        /// <param name="location">The LogiFrame.Location to merge the LogiFrame.Bytemap at.</param>
        public void Merge(Bytemap bytemap, Location location)
        {
            if (bytemap == Empty)
                return;

            if(location == null)
                throw new ArgumentNullException("location cannot be null.");

            //Out of range test
            if (location.X + bytemap.width < 0 ||
                location.Y + bytemap.height < 0 ||
                location.X >= width ||
                location.Y >= height)
                return;

            //TODO: Transparency and TopMergeEffect
            for(int x=Math.Max(location.X, 0);x<Math.Min(width,location.X+bytemap.width);x++)
                for (int y = Math.Max(location.Y, 0); y < Math.Min(height, location.Y + bytemap.height); y++)
                {
                    data[x + y * width] = bytemap.data[x - location.X + (y - location.Y) * bytemap.width];
                }
        }

        /// <summary>
        /// Transform a System.Drawing.Bitmap into a LogiFrame.Bytemap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to transform.</param>
        /// <returns>The new LogiFrame.Bytemap that this method creates. </returns>
        public static Bytemap FromBitmap(System.Drawing.Bitmap bitmap)
        {
            //Everything totally black is black; everything else is white
            return FromBitmap(bitmap, 0, 0, 0, 255);
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
        public static Bytemap FromBitmap(System.Drawing.Bitmap bitmap, byte maxR, byte maxG, byte maxB, byte minA)
        {
            if(bitmap == null)
                return null;

            Bytemap result = new Bytemap((Size)bitmap.Size);

            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    System.Drawing.Color px = bitmap.GetPixel(x,y);
                    result.data[result.width * y + x] = (byte)(px.R <= maxR && px.G <= maxG && px.B <= maxB && minA <= px.A ? 0xff : 0x00);
                }
            return result;
        }

        /// <summary>
        /// Converts the specified LogiFrame.Bytemap instance to a System.Drawing.Bitmap instance.
        /// </summary>
        /// <param name="loc">The LogiFrame.Bytemap to be converted.</param>
        /// <returns>The System.Drawing.Bitmap that results from the conversion.</returns>
        public static implicit operator System.Drawing.Bitmap(Bytemap bytemap)
        {
            if (bytemap == null)
                return null;

            System.Drawing.Bitmap result = new System.Drawing.Bitmap(bytemap.width, bytemap.height);

            for (int y = 0; y < bytemap.height; y++)
                for (int x = 0; x < bytemap.width; x++)
                    if (bytemap.Data[x + y * bytemap.width] == 255)
                        result.SetPixel(x, y, System.Drawing.Color.Black);

            return result;
        }

        private void resize()
        {
            if (width != Size.Width || height != Size.Height)
            {
                byte[] newData = new byte[Size.Width * Size.Height];

                for (int y = 0; y < Math.Min(Size.Height, height); y++)
                    for (int x = 0; x < Math.Min(Size.Width, width); x++)
                    {
                        newData[x + y * Size.Width] = Data[x + y * width];
                    }


                data = newData;

                width = Size.Width;
                height = Size.Height;
            }
        }

        //Callbacks
        private void size_SizeChanged(object sender, EventArgs e)
        {
            resize();
        }
        #endregion
    }
}
