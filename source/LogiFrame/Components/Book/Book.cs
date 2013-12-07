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
using System.Linq;

namespace LogiFrame.Components.Book
{
    /// <summary>
    /// Represents a browsable Component which can contain multiple
    /// instances of the LogiFrame.Components.Book.Page class.
    /// </summary>
    public sealed class Book : Component
    {

        #region Fields

        private Page _activePage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Book.Book class.
        /// </summary>
        /// <param name="frame">The LogiFrame.Frame this LogiFrame.Components.Book.Book should render in.</param>
        public Book(Frame frame)
        {
            base.Size = new Size((int) LgLcd.LglcdBmpWidth, (int) LgLcd.LglcdBmpHeight);
            Pages = new PageCollection<Page>();

            frame.Components.Add(this);

            frame.ButtonDown += frame_ButtonDown;
            frame.ButtonUp += frame_ButtonUp;

            MenuButton = -1;
            BookMenu = new BookMenu(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently active page of this LogiFrame.Components.Book.Book.
        /// </summary>
        public Page ActivePage
        {
            get { return _activePage; }
            set
            {
                if (value == _activePage)
                    return;

                if (_activePage != null)
                {
                    _activePage.Changed -= page_Changed;
                }

        
                if (value != null)
                    value.Changed += page_Changed;

                _activePage = value;
                HasChanged = true;
            }
        }

        /// <summary>
        /// Gets a Collection to add browsable LogiFrame.Components.Book.Page instances to.
        /// </summary>
        public PageCollection<Page> Pages { get; private set; }

        /// <summary>
        /// Gets the LogiFrame.Location this LogiFrame.Components.Book.Book should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Book cannot be changed."); }
        }


        /// <summary>
        /// Gets the LogiFrame.Size of this LogiFrame.Components.Book.Book.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Book cannot be changed."); }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Components.Book.BookMenu this LogiFrame.Components.Book.Book should use.
        /// </summary>
        public BookMenu BookMenu { get; set; }

        /// <summary>
        /// Gets or sets the menu button which opens the BookMenu of this LogiFrame.Components.Book.Book.
        /// Set to -1 to disable this functionality.
        /// </summary>
        public int MenuButton { get; set; }

        #endregion 

        #region Methods

        /// <summary>
        /// Switches to the given LogiFrame.Components.Book.Page.
        /// </summary>
        /// <param name="page">The LogiFrame.Components.Book.Page to switch to.</param>
        public void SwitchTo(Page page)
        {
            ActivePage = page;
        }

        /// <summary>
        /// Switches to the first instance of the given type in the Pages.
        /// </summary>
        /// <param name="pageType">The type of the LogiFrame.Components.Book.Page to switch to.</param>
        public void SwitchTo(Type pageType)
        {
            foreach (var page in Pages.Where(page => page.GetType() == pageType))
            {
                SwitchTo(page);
                HasChanged = true;
                break;
            }
        }

        /// <summary>
        /// Shows the BookMenu.
        /// </summary>
        public void ShowMenu()
        {
            BookMenu.Pages = Pages;
            BookMenu.SelectedPage =
                BookMenu.InitialPage = ActivePage;

            ActivePage = BookMenu;
        }

        protected override void DisposeComponent()
        {
            Pages.Dispose();
        }

        protected override Bytemap Render()
        {
            return _activePage == null ? null : _activePage.Bytemap;
        }

        private void page_Changed(object sender, EventArgs e)
        {
            HasChanged = HasChanged || _activePage.HasChanged;
        }

        private void frame_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (e.Button == MenuButton)
                return;

            if (_activePage != null)
                _activePage.ButtonReleased(e.Button);
        }

        private void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == MenuButton)
            {
                ShowMenu();
                return;
            }

            if (_activePage != null)
                _activePage.ButtonPressed(e.Button);
        }

        #endregion
    }
}