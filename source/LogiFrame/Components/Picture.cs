//     Picture.cs
// 
//     LogiFrame rendering library.
//     Copyright (C) 2013  Tim Potze
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>. 

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable picture.
    /// </summary>
    public class Picture : Component
    {
        private bool _autoSize;
        private ConversionMethod _conversionMethod = ConversionMethod.Normal;
        private System.Drawing.Image _image = null;

        /// <summary>
        ///     The image to be drawn.
        /// </summary>
        public virtual System.Drawing.Image Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                    return;

                _image = value;
                if (AutoSize)
                    MeasureImage(true);
                HasChanged = true;
            }
        }

        /// <summary>
        ///     The conversion method to use during the rendering.
        /// </summary>
        public virtual ConversionMethod ConversionMethod
        {
            get { return _conversionMethod; }
            set
            {
                if (_conversionMethod == value)
                    return;

                _conversionMethod = value;
                if (AutoSize)
                    MeasureImage(true);
                HasChanged = true;
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Components.Picture should automatically
        ///     resize when the image has changed.
        /// </summary>
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize == value)
                    return;

                _autoSize = value;
                if (value)
                    MeasureImage(true);
                HasChanged = true;
            }
        }

        /// <summary>
        ///     The LogiFrame.Size of this LogiFrame.Components.Label.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (!AutoSize)
                    base.Size = value;
            }
        }

        protected override Bytemap Render()
        {
            Bytemap render = new Bytemap(Size);
            render.Merge(Bytemap.FromBitmap(Image as System.Drawing.Bitmap, ConversionMethod), new Location());

            return render;
        }

        private void MeasureImage(bool silent)
        {
            if (silent)
                IsRendering = true;

            if (Image != null)
                base.Size.Set(Image.Width, Image.Height);
            if (silent)
                IsRendering = false;
        }
    }
}