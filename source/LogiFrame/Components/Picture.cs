// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System.Drawing;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable picture.
    /// </summary>
    public class Picture : Component
    {
        private bool _autoSize;
        private ConversionMethod _conversionMethod = ConversionMethod.Normal;
        private Image _image;

        /// <summary>
        ///     Gets or sets the image to be drawn.
        /// </summary>
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                if (!SwapProperty(ref _image, value)) return;
                if (AutoSize) MeasureImage();
            }
        }

        /// <summary>
        ///     Gets or sets the conversion method to use during the rendering.
        /// </summary>
        public virtual ConversionMethod ConversionMethod
        {
            get { return _conversionMethod; }
            set
            {
                if (!SwapProperty(ref _conversionMethod, value)) return;
                if (AutoSize) MeasureImage();
            }
        }

        /// <summary>
        ///     Gets or sets whether this LogiFrame.Components.Picture should automatically
        ///     resize when the image has changed.
        /// </summary>
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (!SwapProperty(ref _autoSize, value)) return;
                if (AutoSize) MeasureImage();
            }
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Size of this LogiFrame.Components.Label.
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