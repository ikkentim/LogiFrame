// UpdatePriority.cs
// 
// LogiFrame rendering library.
// Copyright (C) 2013 Tim Potze
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

namespace LogiFrame
{
    /// <summary>
    /// Represents the priority of the forthcoming LCD updates.
    /// </summary>
    public enum UpdatePriority : uint
    {
        /// <summary>
        /// Lowest priority, disable displaying. Use this priority when you don't have
        /// anything to show.
        /// </summary>
        IdleNoShow = 0,

        /// <summary>
        /// Priority used for low priority items.
        /// </summary>
        Background = 64,

        /// <summary>
        /// Normal priority, to be used by most applications most of the time.
        /// </summary>
        Normal = 128,

        /// <summary>
        /// Highest priority. To be used only for critical screens, such as 'your CPU
        /// temperature is too high'
        /// </summary>
        Alert = 255
    }
}