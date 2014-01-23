// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
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

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable text label.
    /// </summary>
    public class Label : Component
    {
        private bool _autoSize;
        private System.Drawing.Font _font = new System.Drawing.Font("Arial", 7);
        private string _text;

        /// <summary>
        /// Gets or sets the text this LogiFrame.Components.Label should draw.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (!SwapProperty(ref _text, value, false)) return;

                if (AutoSize) MeasureText(true);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the System.Drawing.Font this LogiFrame.Components.Label should draw with.
        /// </summary>
        public System.Drawing.Font Font
        {
            get { return _font; }
            set
            {
                if (!SwapProperty(ref _font, value, false)) return;

                if (AutoSize) MeasureText(true);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether this LogiFrame.Components.Label should automatically
        /// resize to fit the Text.
        /// </summary>
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (!SwapProperty(ref _autoSize, value, false)) return;

                if (AutoSize) MeasureText(true);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Size of this LogiFrame.Components.Label.
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
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Size.Width, Size.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            g.DrawString(Text, Font, System.Drawing.Brushes.Black, new System.Drawing.Point(0, 0));
            return Bytemap.FromBitmap(bmp);
        }

        /// <summary>
        /// Measures the size of the Text when rendered with the set Font.
        /// </summary>
        /// <param name="silent">Whether the change of font should make the LogiFrame.Components.Label rerender.</param>
        private void MeasureText(bool silent)
        {
            if (silent)
                IsRendering = true;

            System.Drawing.SizeF strSize =
                System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(1, 1)).MeasureString(Text, Font);
            base.Size.Set((int) Math.Ceiling(strSize.Width), (int) Math.Ceiling(strSize.Height));

            if (silent)
                IsRendering = false;
        }
    }
}