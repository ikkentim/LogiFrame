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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a style of drawing a <see cref="ProgressBar" />.
    /// </summary>
    public enum ProgressBarStyle
    {
        /// <summary>
        ///     A drawing type where there is no border around the ProgressBar.
        /// </summary>
        NoBorder,

        /// <summary>
        ///     A drawing type where there is a border around the ProgressBar.
        /// </summary>
        Border,

        /// <summary>
        ///     A drawing type where there is a border around the ProgressBar,
        ///     and a single pixel of whitespace between the border in the inner bar.
        /// </summary>
        WhiteSpacedBorder
    }
}