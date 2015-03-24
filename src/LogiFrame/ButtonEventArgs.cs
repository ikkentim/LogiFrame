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

namespace LogiFrame
{
    /// <summary>
    ///     Provides data for the <see cref="Frame.ButtonDown" /> and <see cref="Frame.ButtonUp" /> events.
    /// </summary>
    public class ButtonEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonEventArgs" /> class.
        /// </summary>
        /// <param name="button">0-based number of the button being pressed.</param>
        public ButtonEventArgs(int button)
        {
            Button = button;
        }

        /// <summary>
        ///     Gets the 0-based number of the button being pressed.
        /// </summary>
        public int Button { get; private set; }
    }
}