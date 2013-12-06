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
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace LogiFrame.Components.Book
{
    public sealed class Book : Component
    {
        private Page _activePage;

        private Book()
        {
            Debug.WriteLine("Book initialized");
            base.Size = new Size((int)LgLcd.LglcdBmpWidth, (int)LgLcd.LglcdBmpHeight);
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
            get
            {
                return _activePage;
            }
            set
            {
                if (_activePage != null)
                {
                    _activePage.Changed -= page_Changed;
                }

                value.Changed += page_Changed;

                if (value == _activePage)
                    return;

                _activePage = value;
                HasChanged = true;
            }
        }

        private void page_Changed(object sender, EventArgs e)
        {
            HasChanged = HasChanged || _activePage.HasChanged;
        }

        private void frame_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (_activePage != null)
            {
            }
        }

        private void frame_ButtonDown(object sender, ButtonEventArgs e)
        {

        }

        public PageCollection<Page> Pages { get; private set; }

        public override Location Location
        {
            get
            {
                return base.Location;
            }
            set
            {
               throw new ArgumentException("The Location of a LogiFrame.Components.Book.Book cannot be changed."); 
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
                throw new ArgumentException("The Size of a LogiFrame.Components.Book.Book cannot be changed.");
            }
        }

        public void SwitchTo(Page page)
        {
            _activePage = page;
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

        protected override void DisposeComponent()
        {
            foreach (var page in Pages)
            {
                page.Dispose();
            }
            Pages.Clear();
        }

        protected override Bytemap Render()
        {
            return _activePage == null ? null : _activePage.Bytemap;
        }
    }
}
