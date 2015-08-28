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

using System.Drawing;
using System.Drawing.Text;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FrameLabel : FrameControl
    {
        private Font _font = new Font(PixelFonts.SmallFamily, 6);
        private string _text;

        public virtual Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                Invalidate();
            }
        }

        public virtual string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Invalidate();
            }
        }
        
        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            if (Font != null && Text != null)
            {
                var bitmap = new Bitmap(Width, Height);

                using (var g = Graphics.FromImage(bitmap))
                {
                    g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                    g.DrawString(Text, Font, Brushes.Black, new Point(0, 0));
                }

                e.Bitmap.Merge(new MonochromeBitmap(bitmap), new Point(), MergeMethods.Override);

            }
        }

        #endregion
    }
}