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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable picture.
    /// </summary>
    public class Picture : Component
    {
        private bool _isAutoSize;
        private ConversionMethod _conversionMethod = ConversionMethod.Normal;
        private Image _image;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                if (!SwapProperty(ref _image, value)) return;
                if (IsAutoSize) MeasureImage();
            }
        }

        /// <summary>
        /// Gets or sets the conversion method.
        /// </summary>
        public virtual ConversionMethod ConversionMethod
        {
            get { return _conversionMethod; }
            set
            {
                if (!SwapProperty(ref _conversionMethod, value)) return;
                if (IsAutoSize) MeasureImage();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic size.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is automatic size; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoSize
        {
            get { return _isAutoSize; }
            set
            {
                if (!SwapProperty(ref _isAutoSize, value)) return;
                if (IsAutoSize) MeasureImage();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (!IsAutoSize)
                    base.Size = value;
            }
        }

        protected override Bytemap Render()
        {
            var render = new Bytemap(Size);
            render.Merge(Bytemap.FromBitmap(Image as Bitmap, ConversionMethod), new Location());

            return render;
        }

        private void MeasureImage()
        {
            if (Image == null) return;
            IsRendering = true;
            base.Size.Set(Image.Width, Image.Height);
            IsRendering = false;
        }
    }
}