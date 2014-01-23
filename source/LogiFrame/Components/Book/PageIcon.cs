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

namespace LogiFrame.Components.Book
{
    /// <summary>
    /// Represents a drawable icon of a LogiFrame.Components.Book.Page.
    /// </summary>
    public class PageIcon : Container
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Book.PageIcon class.
        /// </summary>
        public PageIcon()
        {
            base.Size = new Size(16, 16);
        }

        /// <summary>
        /// Gets the LogiFrame.Location this LogiFrame.Components.Book.PageIcon should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set
            {
                throw new ArgumentException("The Location of a LogiFrame.Components.Book.PageIcon cannot be changed.");
            }
        }


        /// <summary>
        /// Gets the LogiFrame.Size of this LogiFrame.Components.Book.PageIcon.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }
    }
}