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
using LogiFrame.Drawing;

namespace LogiFrame
{
    /// <summary>
    /// Provides data for the <see cref="E:LogiFrame.LCDControl.Paint"/> event.
    /// </summary>
    public class LCDPaintEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LCDPaintEventArgs"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if bitmap is null.</exception>
        public LCDPaintEventArgs(MonochromeBitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            Bitmap = bitmap;
        }

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        public MonochromeBitmap Bitmap { get; }
    }
}