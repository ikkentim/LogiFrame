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

        #endregion

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

        private void AdjustSize()
        {
            if (!AutoSize) return;

            using (var bitmap = new Bitmap(1, 1))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var measure = graphics.MeasureString(Text, Font);
                var size = new Size((int) Math.Ceiling(measure.Width), (int) Math.Ceiling(measure.Height));

                if (Size.Equals(size))
                    return;

                base.Size = size;
            }
        }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            if (Font != null && Text != null)
                using (var bitmap = new Bitmap(Width, Height))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    graphics.DrawString(Text, Font, Brushes.Black, new Point(0, 0));
                    e.Bitmap.Merge(new MonochromeBitmap(bitmap), new Point(), MergeMethods.Override);
                }
        }

        #endregion
    }
}