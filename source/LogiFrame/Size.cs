﻿using System;

namespace LogiFrame
{
    /// <summary>
    /// Represents the size of a component.
    /// </summary>
    public class Size
    {

        /// <summary>
        /// Represents the method that handles a LogiFrame.Size.SizeChanged.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.SizeChangedEventArgs that contains the event data.</param>
        public delegate void SizeChangedEventHandler(object sender, SizeChangedEventArgs e);

        /// <summary>
        /// Occurs when the Size has been changed.
        /// </summary>
        public event SizeChangedEventHandler SizeChanged;

        #region Properties
        private int width;
        /// <summary>
        /// The width of the LogiFrame.Size.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("The width of a Size must be higher than 0.");

                width = value;
                if (SizeChanged != null)
                    SizeChanged(this, new SizeChangedEventArgs());
            }
        }

        private int height;
        /// <summary>
        /// The height of the LogiFrame.Size.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("The height of a Size must be higher than 0.");

                height = value;
                if (SizeChanged != null)
                    SizeChanged(this, new SizeChangedEventArgs());
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        public Size()
        {
        }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="parent">An instance of LogiFrame.Size to copy the dimentions from.</param>
        public Size(Size parent)
        {
            width = parent.Width;
            height = parent.Height;
        }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Location class.
        /// </summary>
        /// <param name="width">The initial width.</param>
        /// <param name="height">The initial height.</param>
        public Size(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("The width and height of a Size must be higher than 0.");
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Adds a certain value to the current LogiFrame.Size instance.
        /// </summary>
        /// <param name="width">Value to add to the width.</param>
        /// <param name="height">Value to add to the height</param>
        public void Add(int width, int height)
        {
            bool changed = width != 0 || height != 0;
            this.width += width;
            this.height += height;

            if (changed && SizeChanged != null)
                SizeChanged(this, new SizeChangedEventArgs());
        }

        /// <summary>
        /// Adds a certain value to the current LogiFrame.Size instance.
        /// </summary>
        /// <param name="other">Value to add to the width and height.</param>
        public void Add(Size other)
        {
            bool changed = other.Width != 0 || other.Height != 0;
            this.width += other.Width;
            this.height += other.Height;

            if (changed && SizeChanged != null)
                SizeChanged(this, new SizeChangedEventArgs());
        }

        /// <summary>
        /// Set a certain value to the current LogiFrame.Size instance.
        /// </summary>
        /// <param name="width">The new width value.</param>
        /// <param name="height">The new height value.</param>
        public void Set(int width, int height)
        {
            bool changed = this.width != width || this.height != height;
            this.width = width;
            this.height = height;

            if (changed && SizeChanged != null)
                SizeChanged(this, new SizeChangedEventArgs());
        }

        /// <summary>
        /// Set a certain value to the current LogiFrame.Size instance.
        /// </summary>
        /// <param name="other">An instance of LogiFrame.Size to copy the dimensions from.</param>
        public void Set(Size other)
        {
            bool changed = this.Width != other.Width || this.Height != other.Height;
            this.width = other.Width;
            this.height = other.Height;

            if (changed && SizeChanged != null)
                SizeChanged(this, new SizeChangedEventArgs());
        }

        /// <summary>
        /// Translates a LogiFrame.Size by a given LogiFrame.Size.
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
        /// Translates a LogiFrame.Size by the  of a given LogiFrame.Size.
        /// </summary>
        /// <param name="loc1">The LogiFrame.Size to translate.</param>
        /// <param name="loc2">
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
        /// Converts the specified LogiFrame.Size instance to a System.Drawing.Size struct.
        /// </summary>
        /// <param name="size">The LogiFrame.Size to be converted.</param>
        /// <returns>The System.Drawing.Size that results from the conversion.</returns>
        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        /// Converts the specified System.Drawing.Size struct to a LogiFrame.Size instance.
        /// </summary>
        /// <param name="size">The System.Drawing.Size to be converted.</param>
        /// <returns>The LogiFrame.Size that results from the conversion.</returns>
        public static implicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current LogiFrame.Size.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current LogiFrame.Size.</param>
        /// <returns>
        ///     true if the specified System.Object is equal to the current LogiFrame.Size;
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
        /// Returns a hash code for this LogiFrame.Size.
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
        /// Converts this LogiFrame.Size to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this LogiFrame.Size.</returns>
        public override string ToString()
        {
            return "(" + Width + ", " + Height + ")";
        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Size.SizeChanged event.
    /// </summary>
    public class SizeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.SizeChangedEventArgs class.
        /// </summary>
        public SizeChangedEventArgs()
        {

        }
    }
}