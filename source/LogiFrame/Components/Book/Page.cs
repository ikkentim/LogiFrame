// ComponentCollection.cs
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

using System;

namespace LogiFrame.Components.Book
{
    public abstract class Page : Container
    {
        protected Page()
        {
            base.Size = new Size((int)LgLcd.LglcdBmpWidth, (int)LgLcd.LglcdBmpHeight);
        }

        public override Location Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                throw new ArgumentException("The Location of a LogiFrame.Components.Book.Page cannot be changed.");
            }
        }

        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                throw new ArgumentException("The Size of a LogiFrame.Components.Book.Page cannot be changed.");
            }
        }

        public virtual void ButtonPressed(int button)
        {
            
        }

        public virtual void ButtonReleased(int button)
        {
            
        }
    }
}
