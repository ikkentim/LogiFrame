﻿// LogiFrame
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
    ///     Provides data for the <see cref="LogiFrame.Frame.Pushing" /> event.
    /// </summary>
    public class PushingEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PushingEventArgs" /> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public PushingEventArgs(Snapshot frame)
        {
            Frame = frame;
        }

        /// <summary>
        ///     Gets or sets whether the frame should be prevented from being
        ///     pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        /// <summary>
        ///     Gets the frame that is about to be pushed.
        /// </summary>
        public Snapshot Frame { get; private set; }
    }
}