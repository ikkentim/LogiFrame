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

namespace LogiFrame
{
    /// <summary>
    ///     Represents the priority of LCD updates.
    /// </summary>
    public enum UpdatePriority : uint
    {
        /// <summary>
        ///     Lowest priority, disable displaying. Use this priority when you don't have
        ///     anything to show.
        /// </summary>
        IdleNoShow = 0,

        /// <summary>
        ///     Priority used for low priority items.
        /// </summary>
        Background = 64,

        /// <summary>
        ///     Normal priority, to be used by most applications most of the time.
        /// </summary>
        Normal = 128,

        /// <summary>
        ///     Highest priority. To be used only for critical screens.
        /// </summary>
        Alert = 255
    }
}