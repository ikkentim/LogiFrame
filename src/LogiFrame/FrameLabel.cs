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
using System.Drawing;
using System.Drawing.Text;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FrameLabel : FrameControl
    {
        private bool _autoSize;
        private Font _font = new Font(PixelFonts.SmallFamily, 6);
        private string _text;
        private ContentAlignment _textAlign = ContentAlignment.TopLeft;

        public virtual Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                SuspendLayout();
                AdjustSize();
                Invalidate();
                ResumeLayout();
            }
        }

        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                AdjustSize();
            }
        }

        public virtual ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value; 
                Invalidate();
            }
        }

        public virtual string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                SuspendLayout();
                AdjustSize();
                Invalidate();
                ResumeLayout();
            }
        }

        private Size MeasureText()
        {
            if (Font == null || string.IsNullOrEmpty(Text))
                return new Size(1, 1);

            using (var bitmap = new Bitmap(1, 1))
            using (var graphics = Graphics.FromImage(bitmap))
                return Size.Round(graphics.MeasureString(Text, Font));
        }

        private void AdjustSize()
        {
            if (!AutoSize) return;

            var size = MeasureText();

            if (Size.Equals(size))
                return;

            base.Size = size;
        }

        #region Overrides of FrameControl

        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (AutoSize) return;
                base.Size = value;
            }
        }
        
        protected override void OnPaint(FramePaintEventArgs e)
        {
            if (Font != null && Text != null)
                using (var bitmap = new Bitmap(Width, Height))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

                    if (AutoSize)
                        graphics.DrawString(Text, Font, Brushes.Black, new Point(0, 0));
                    else
                    {
                        Size size;
                        switch (TextAlign)
                        {
                            default:
                            case ContentAlignment.TopLeft:
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(0, 0));
                                break;
                            case ContentAlignment.TopCenter:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point((Width-size.Width)/2, 0));
                                break;
                            case ContentAlignment.TopRight:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(Width - size.Width, 0));
                                break;
                            case ContentAlignment.MiddleLeft:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(0, (Height-size.Height)/2));
                                break;
                            case ContentAlignment.MiddleCenter:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point((Width - size.Width) / 2, (Height - size.Height) / 2));
                                break;
                            case ContentAlignment.MiddleRight:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(Width - size.Width, (Height - size.Height) / 2));
                                break;
                            case ContentAlignment.BottomLeft:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(0, Height - size.Height));
                                break;
                            case ContentAlignment.BottomCenter:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point((Width -size.Width)/2, Height - size.Height));
                                break;
                            case ContentAlignment.BottomRight:
                                size = MeasureText();
                                graphics.DrawString(Text, Font, Brushes.Black, new Point(Width-size.Width, Height - size.Height));
                                break;
                        }
                    }

                    e.Bitmap.Merge(new MonochromeBitmap(bitmap), new Point(), MergeMethods.Override);
                }
        }

        #endregion
    }
}