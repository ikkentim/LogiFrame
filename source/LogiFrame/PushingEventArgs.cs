// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;

namespace LogiFrame
{
    /// <summary>
    /// Provides data for the LogiFrame.Frame.Pushing event.
    /// </summary>
    public class PushingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.PushingEventArgs class.
        /// </summary>
        /// <param name="frame">The frame that is about to be pushed.</param>
        public PushingEventArgs(Bytemap frame)
        {
            Frame = frame;
        }

        /// <summary>
        /// Gets or sets whether this frame should be prevented from being
        /// pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        /// <summary>
        /// Gets the frame that is about to be
        /// </summary>
        public Bytemap Frame { get; private set; }
    }
}