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
    public sealed class Book : Component
    {
        private Page _activePage;

        private Book()
        {
            base.Size = new Size((int) LgLcd.LglcdBmpWidth, (int) LgLcd.LglcdBmpHeight);
            Pages = new PageCollection<Page>();
        }

        public Book(Frame frame) : this()
        {
            frame.Components.Add(this);

            frame.ButtonDown += frame_ButtonDown;
            frame.ButtonUp += frame_ButtonUp;
        }

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

        public PageCollection<Page> Pages { get; private set; }

        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Book cannot be changed."); }
        }

        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Book cannot be changed."); }
        }

        private void page_Changed(object sender, EventArgs e)
        {
            HasChanged = HasChanged || _activePage.HasChanged;
        }

        private void frame_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (_activePage != null)
                _activePage.ButtonReleased(e.Button);
        }

        private void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (_activePage != null)
                _activePage.ButtonPressed(e.Button);
        }

        public void SwitchTo(Page page)
        {
            ActivePage = page;
        }

        public void SwitchTo(Type pageType)
        {
            foreach (var page in Pages.Where(page => page.GetType() == pageType))
            {
                SwitchTo(page);
                HasChanged = true;
                break;
            }
        }

        public void ShowMenu()
        {
            BookMenu menu = new BookMenu(this) {Pages = Pages, SelectedPage = ActivePage, InitialPage = ActivePage};

            ActivePage = menu;
        }

        protected override void DisposeComponent()
        {
            Pages.Dispose();
        }

        protected override Bytemap Render()
        {
            return _activePage == null ? null : _activePage.Bytemap;
        }
    }
}