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

using LogiFrame.Drawing;

namespace LogiFrame
{
    public class ContainerFrameControl : FrameControl
    {
        public ContainerFrameControl()
        {
            Controls = new FrameControlCollection(this);
        }

        public FrameControlCollection Controls { get; }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            if (Controls == null) return;

            foreach (var control in Controls)
            {
                control.PerformLayout();
                e.Bitmap.Merge(control.Bitmap, control.Location, control.MergeMethod ?? MergeMethods.Override);
            }
            base.OnPaint(e);
        }

        #endregion
    }
}