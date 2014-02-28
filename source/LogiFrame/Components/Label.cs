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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable text label.
    /// </summary>
    public class Label : Component
    {
        #region Fields

        private readonly List<CacheItem> _cache = new List<CacheItem>();
        private bool _autoSize;
        private Font _font = new Font("Arial", 7);
        private Alignment _horizontalAlignment = Alignment.Left;
        private string _text;
        private Alignment _verticalAlignment = Alignment.Top;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text this LogiFrame.Components.Label should draw.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (!SwapProperty(ref _text, value, false, false)) return;

                if (AutoSize) MeasureText(true);
                AlignText();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the System.Drawing.Font this LogiFrame.Components.Label should draw with.
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set
            {
                if (!SwapProperty(ref _font, value, false)) return;

                if (AutoSize) MeasureText(true);
                AlignText();
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
                AlignText();
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
                AlignText();
            }
        }

        /// <summary>
        /// Gets or sets the vertical LogiFrame.Component.Alignment of this LogiFrame.Components.Label.
        /// </summary>
        public Alignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (!SwapProperty(ref _verticalAlignment, value, false)) return;
                AlignText();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal LogiFrame.Component.Alignment of this LogiFrame.Components.Label.
        /// </summary>
        public Alignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                if (!SwapProperty(ref _horizontalAlignment, value, false)) return;
                AlignText();
            }
        }

        /// <summary>
        /// Gets or sets whether the label should cache all rendered texts.
        /// </summary>
        public bool UseCache { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all cache items from the Label's cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        protected override Bytemap Render()
        {
            if (UseCache)
            {
                var cacheItem = _cache.FirstOrDefault(c => c.Text == Text && c.Font.Equals(Font));
                if (cacheItem != null)
                {
                    if (AutoSize)
                        Size = cacheItem.Bytemap.Size;
                    return cacheItem.Bytemap;
                }
            }

            Bitmap bmp = new Bitmap(Size.Width, Size.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            g.DrawString(Text, Font, Brushes.Black, new Point(0, 0));

            var bymp = Bytemap.FromBitmap(bmp);
            if (UseCache)
                _cache.Add(new CacheItem {Bytemap = bymp, Font = Font.Clone() as Font, Text = Text});

            return bymp;
        }

        private void MeasureText(bool silent)
        {
            if (silent) IsRendering = true;
            SizeF strSize = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(Text, Font);
            base.Size.Set((int) Math.Ceiling(strSize.Width), (int) Math.Ceiling(strSize.Height));
            if (silent) IsRendering = false;
        }

        private void AlignText()
        {
            var x = 0;
            var y = 0;

            switch (HorizontalAlignment)
            {
                case Alignment.Left:
                    x = 0;
                    break;
                case Alignment.Middle:
                    x = -Size.Width/2;
                    break;
                case Alignment.Right:
                    x = -Size.Width;
                    break;
            }

            switch (VerticalAlignment)
            {
                case Alignment.Top:
                    y = 0;
                    break;
                case Alignment.Center:
                    y = -Size.Height/2;
                    break;
                case Alignment.Bottom:
                    y = -Size.Height;
                    break;
            }

            RenderOffset.Set(x, y);
        }

        #endregion

        #region Subclasses

        private class CacheItem
        {
            public string Text { get; set; }
            public Font Font { get; set; }
            public Bytemap Bytemap { get; set; }
        }

        #endregion
    }
}