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

namespace LogiFrame
{
    public class LCDRectangle : LCDControl
    {
        private RectangleStyle _style;

        public RectangleStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        #region Overrides of LCDControl

        protected override void OnPaint(LCDPaintEventArgs e)
        {
            switch (Style)
            {
                case RectangleStyle.Bordered:
                    for (var x = 1; x < Width - 1; x++)
                    {
                        e.Bitmap[x, 0] = true;
                        e.Bitmap[x, Height - 1] = true;
                    }
                    for (var y = 0; y < Height; y++)
                    {
                        e.Bitmap[0, y] = true;
                        e.Bitmap[Width - 1, y] = true;
                    }
                    break;
                case RectangleStyle.Filled:
                    for (var x = 0; x < Width; x++)
                        for (var y = 0; y < Height; y++)
                            e.Bitmap[x, y] = true;
                    break;
            }
            base.OnPaint(e);
        }

        #endregion
    }
}