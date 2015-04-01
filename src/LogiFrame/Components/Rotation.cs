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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents modifications which can be set to the Rotation property of <see cref="Rotator" />.
    /// </summary>
    [Flags]
    public enum Rotation
    {
        /// <summary>
        ///     Rotates the container 0 degrees.
        /// </summary>
        Rotate0Degrees = 0,

        /// <summary>
        ///     Rotates the container 90 degrees.
        /// </summary>
        Rotate90Degrees = 1,

        /// <summary>
        ///     Rotates the container 180 degrees.
        /// </summary>
        Rotate180Degrees = 2,

        /// <summary>
        ///     Rotates the container 270 degrees.
        /// </summary>
        Rotate270Degrees = 4,

        /// <summary>
        ///     Flips the container horizontally.
        /// </summary>
        FlipHorizontal = 8,

        /// <summary>
        ///     Flips the container vertically.
        /// </summary>
        FlipVertical = 16
    }
}