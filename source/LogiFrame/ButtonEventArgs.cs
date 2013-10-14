//     ButtonEventArgs.cs
// 
//     LogiFrame rendering library.
//     Copyright (C) 2013  Tim Potze
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;

namespace LogiFrame
{
    /// <summary>
    ///     Provides data for the LogiFrame.Frame.ButtonDown and LogiFrame.Frame.ButtonUp events.
    /// </summary>
    public class ButtonEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the LogiFrame.ButtonEventArgs class.
        /// </summary>
        /// <param name="button">0-based number of the button being pressed.</param>
        public ButtonEventArgs(int button)
        {
            Button = button;
        }

        /// <summary>
        ///     Represents the 0-based number of the button being pressed.
        /// </summary>
        public int Button { get; set; }
    }
}