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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable text label.
    /// </summary>
    public class Label : Component
    {
        private readonly List<CacheItem> _cache = new List<CacheItem>();
        private bool _isAutoSize;
        private Font _font = new Font("Arial", 7);
        private Alignment _horizontalAlignment = Alignment.Left;
        private string _text;
        private Alignment _verticalAlignment = Alignment.Top;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (!SwapProperty(ref _text, value, false, false)) return;

                if (IsAutoSize) MeasureText(true);
                AlignText();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set
            {
                if (!SwapProperty(ref _font, value, false)) return;

                if (IsAutoSize) MeasureText(true);
                AlignText();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatically resizing.
        /// </summary>
        public bool IsAutoSize
        {
            get { return _isAutoSize; }
            set
            {
                if (!SwapProperty(ref _isAutoSize, value, false)) return;

                if (IsAutoSize) MeasureText(true);
                AlignText();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (!IsAutoSize)
                    base.Size = value;
                AlignText();
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment.
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
        /// Gets or sets the horizontal alignment.
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
        ///     Gets or sets whether the label should cache all rendered texts.
        /// </summary>
        public bool IsUseCache { get; set; }

        /// <summary>
        ///     Clears all cache items from the Label's cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Renders this instance to a <see cref="Snapshot" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            if (IsUseCache)
            {
                CacheItem cacheItem = _cache.FirstOrDefault(c => c.Text == Text && c.Font.Equals(Font));
                if (cacheItem != null)
                {
                    if (IsAutoSize)
                        Size = cacheItem.Snapshot.Size;
                    return cacheItem.Snapshot;
                }
            }

            Snapshot snapshot;
            using (var bmp = new Bitmap(Size.Width, Size.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawString(Text, Font, Brushes.Black, new Point(0, 0));
                }
                
                snapshot = Snapshot.FromBitmap(bmp);
            }
            if (IsUseCache)
                _cache.Add(new CacheItem { Snapshot = snapshot, Font = Font.Clone() as Font, Text = Text });

            return snapshot;
        }

        private void MeasureText(bool silent)
        {
            if (silent) IsRendering = true;
            SizeF strSize = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(Text, Font);
            base.Size = new Size((int) Math.Ceiling(strSize.Width), (int) Math.Ceiling(strSize.Height));
            if (silent) IsRendering = false;
        }

        private void AlignText()
        {
            int x = 0;
            int y = 0;

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

            RenderOffset = new Location(x, y);
        }

        private class CacheItem
        {
            public string Text { get; set; }
            public Font Font { get; set; }
            public Snapshot Snapshot { get; set; }
        }
    }
}