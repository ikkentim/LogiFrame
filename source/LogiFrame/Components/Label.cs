using System;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable text label.
    /// </summary>
    public class Label : Component
    {
        private System.Drawing.Font _font = new System.Drawing.Font("Arial", 7);
        private string _text;
        private bool _autoSize;

        /// <summary>
        /// The text the current LogiFrame.Components.Label should draw.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                _text = value;

                if (AutoSize) 
                    MeasureText(true);

                HasChanged = true;
            }
        }

        /// <summary>
        /// The font the current LogiFrame.Components.Label should draw with.
        /// </summary>
        public System.Drawing.Font Font
        {
            get { return _font; }
            set
            {
                if (_font == value)
                    return;

                _font = value;
                if(AutoSize)
                    MeasureText(true);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Whether this LogiFrame.Components.Label should automatically
        /// resize when the text has changed.
        /// </summary>
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize == value)
                    return;

                _autoSize = value;
                if(value)
                    MeasureText(true);
                HasChanged = true;
            }
        }

        /// <summary>
        /// The LogiFrame.Size of this LogiFrame.Components.Label.
        /// </summary>
        public override Size Size
        {
            get {  return base.Size; }
            set
            {
                if(!AutoSize)
                    base.Size = value;
            }
        }

        protected override Bytemap Render()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Size.Width, Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            g.DrawString(Text, Font, System.Drawing.Brushes.Black, new System.Drawing.Point(0, 0));
            return Bytemap.FromBitmap(bmp);
        }

        private void MeasureText(bool silent)
        {
            if (silent)
                IsRendering = true;

            System.Drawing.SizeF strSize = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)).MeasureString(Text, Font);
            base.Size.Set((int)Math.Ceiling(strSize.Width), (int)Math.Ceiling(strSize.Height));

            if (silent)
                IsRendering = false;
        }
    }
}
