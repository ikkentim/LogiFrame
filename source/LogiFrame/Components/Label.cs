using System;
using LogiFrame;

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

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;

                if (AutoSize)
                    measureText();
            }
        }

        public System.Drawing.Font Font
        {
            get { return _font; }
            set
            {
                _font = value;

                if(AutoSize)
                    measureText();
            }
        }

        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
            }
        }

        public override Size Size
        {
            get {  return base.Size; }
            set
            {
                if(AutoSize)
                    base.Size = value;
            }
        }

        protected override Bytemap Render()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Size.Width, Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            g.DrawString("test123", _font, System.Drawing.Brushes.Black, (System.Drawing.Point)Location);
            return Bytemap.FromBitmap(bmp);
        }

        private void measureText()
        {
            System.Drawing.SizeF strSize = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)).MeasureString(Text, Font);
            base.Size = new Size((int)Math.Ceiling(strSize.Width), (int)Math.Ceiling(strSize.Height));
        }
    }
}
