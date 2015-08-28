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
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FramePicture : FrameControl
    {
        private MonochromeBitmap _bitmap;
        private Image _image;

        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                _bitmap = value == null
                    ? null
                    : new MonochromeBitmap(value is Bitmap ? (Bitmap) value : new Bitmap(value));
                Invalidate();
            }
        }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            if (_bitmap != null)
                e.Bitmap.Merge(_bitmap, new Point(0, 0), MergeMethods.Override);

            base.OnPaint(e);
        }

        #endregion
    }
}