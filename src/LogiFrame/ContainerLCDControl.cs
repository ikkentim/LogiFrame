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

using System.Linq;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class ContainerLCDControl : LCDControl
    {
        public ContainerLCDControl()
        {
            Controls = new LCDControlCollection(this);
        }

        public LCDControlCollection Controls { get; }

        #region Overrides of LCDControl

        protected override void OnPaint(LCDPaintEventArgs e)
        {
            if (Controls == null) return;

            foreach (var control in Controls)
            {
                control.PerformLayout();
                e.Bitmap.Merge(control.Bitmap, control.Location, control.MergeMethod ?? MergeMethods.Override);
            }
            base.OnPaint(e);
        }

        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (Controls.Any(control => control.HandleButtonDown(e.Button)))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonDown(e);
        }

        protected override void OnButtonUp(ButtonEventArgs e)
        {
            if (Controls.Any(control => control.HandleButtonUp(e.Button)))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonUp(e);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:LogiFrame.LCDControl" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            Controls.Clear();
            base.Dispose(disposing);
        }

        #endregion
    }
}