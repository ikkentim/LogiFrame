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
    /// Represents a page which can be used with a LogiFrame.Components.Book.Book.
    /// </summary>
    public abstract class Page : Container
    {
        private PageIcon _pageIcon;

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Book.Page class.
        /// </summary>
        protected Page()
        {
            base.Size = new Size((int) LgLcd.LglcdBmpWidth, (int) LgLcd.LglcdBmpHeight);
        }

        /// <summary>
        /// Gets the LogiFrame.Location this LogiFrame.Components.Book.Page should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Page cannot be changed."); }
        }


        /// <summary>
        /// Gets the LogiFrame.Size of this LogiFrame.Components.Book.Page.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Page cannot be changed."); }
        }

        /// <summary>
        /// Gets the LogiFrame.Components.Book.PageIcon of this LogiFrame.Components.Book.Page.
        /// </summary>
        public PageIcon PageIcon
        {
            get { return _pageIcon = _pageIcon ?? GetPageIcon(); }
        }

        /// <summary>
        /// Is called when a button has been pressed.
        /// </summary>
        /// <param name="e">Contains information about the button pressed</param>
        public virtual void OnButtonPressed(ButtonEventArgs e)
        {
        }

        /// <summary>
        /// Is called when a button has been released.
        /// </summary>
        /// <param name="e">Contains information bout the button pressed.</param>
        public virtual void OnButtonReleased(ButtonEventArgs e)
        {
        }

        /// <summary>
        /// Is called when the page is being showed.
        /// </summary>
        /// <param name="e">Contains information bout the event.</param>
        public virtual void OnShow(EventArgs e)
        {
        }

        /// <summary>
        /// Is called when the page is being hidden.
        /// </summary>
        /// <param name="e">Contains information bout the event.</param>
        public virtual void OnHide(EventArgs e)
        {
        }

        /// <summary>
        /// Returns the LogiFrame.Components.Book.PageIcon of this LogiFrame.Components.Book.Page.
        /// </summary>
        /// <returns>LogiFrame.Components.Book.PageIcon of this LogiFrame.Components.Book.Page.</returns>
        protected abstract PageIcon GetPageIcon();


        /// <summary>
        /// Return the name of this LogiFrame.Components.Book.Page.
        /// </summary>
        /// <returns>The name of this LogiFrame.Components.Book.Page.</returns>
        public abstract string GetName();


        /// <summary>
        /// Return whether this LogiFrame.Components.Book.Page should be visible in a BookMenu.
        /// </summary>
        /// <returns>The name of this LogiFrame.Components.Book.Page.</returns>
        public virtual bool IsBrowsable()
        {
            return true;
        }
    }
}