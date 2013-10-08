using System;
using LogiFrame;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable text label.
    /// </summary>
    public class Label : Component
    {
        private System.Drawing.Font font = new System.Drawing.Font("Arial", 7);
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
            }
        }

        public System.Drawing.Font Font
        {
            get { return font; }
            set
            {
                font = value;
            }
        }

        protected override Bytemap Render()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Size.Width, Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString("test123", font, System.Drawing.Brushes.Black, (System.Drawing.Point)Location);
            return Bytemap.FromBitmap(bmp);
        }
    }
}
