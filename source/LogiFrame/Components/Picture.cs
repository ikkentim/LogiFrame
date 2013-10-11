namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable picture.
    /// </summary>
    public class Picture : Component
    {
        private System.Drawing.Image _image = null;
        private ConversionMethod _conversionMethod = ConversionMethod.Normal;
        private bool _autoSize;

        /// <summary>
        /// The image to be drawn.
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
        /// The conversion method to use during the rendering.
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
        /// Whether this LogiFrame.Components.Picture should automatically
        /// resize when the image has changed.
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
        /// The LogiFrame.Size of this LogiFrame.Components.Label.
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

            if(Image != null)
                base.Size.Set(Image.Width, Image.Height);
            if (silent)
                IsRendering = false;
        }
    }
}
