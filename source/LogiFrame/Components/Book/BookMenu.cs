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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LogiFrame.Components.Book
{
    public class BookMenu : Page
    {
        private Book _book;
        private Label _pageTitle;
        private Square _selectionSquare;
        private Page _selectedPage;
        private PageCollection<Page> _pages;
 
        public PageCollection<Page> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                if (_pages != null)
                {
                    _pages.PageAdded -= pages_PageAdded;
                    _pages.PageRemoved -= pages_PageRemoved;
                }

                _pages = value;

                if (_pages == null) return;

                _pages.PageAdded += pages_PageAdded;
                _pages.PageRemoved += pages_PageRemoved;

                HasChanged = true;
            }
        }

        public Page SelectedPage
        {
            get
            {
                return _selectedPage;
            }
            set
            {
                if (_selectedPage == value)
                    return;

                _selectedPage = value;
                HasChanged = true;
            }
        }

        public Page InitialPage { get; set; }

        public BookMenu(Book book)
        {
            _book = book;

            _pageTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                Location = new Location(1, 1)
            };

            _selectionSquare = new Square
            {
                Location = new Location(70, 22),
                Size = new Size(20, 20)
            };

            Components.Add(_pageTitle);
            Components.Add(_selectionSquare);
        }

        protected override Bytemap Render()
        {
            if (Pages.Count == 0)
                return null;

            var selectedIndex = Pages.IndexOf(SelectedPage);

            if (selectedIndex == -1)
            {
                SelectedPage = Pages[0];
                selectedIndex = 0;
            }

            var pageInfo = (PageInfo)Attribute.GetCustomAttribute(SelectedPage.GetType(), typeof(PageInfo)) ?? new PageInfo();
            
            _pageTitle.Text = pageInfo.Name;

            for (var i = 0; i < Pages.Count(); i++)
            {
                var icon = Pages[i].PageIcon;
                icon.Location.Set(72 + 20*(i - selectedIndex), 24);

                if(!Components.Contains(icon))
                    Components.Add(icon);
            }

            return base.Render();
        }

        private void pages_PageRemoved(object sender, ComponentChangedEventArgs e)
        {
            HasChanged = true;
        }

        private void pages_PageAdded(object sender, ComponentChangedEventArgs e)
        {
            HasChanged = true;
        }

        public override void ButtonPressed(int button)
        {
            switch (button)
            {
                case 0:
                    if (Pages.Count == 0)
                        break;

                    var prevIndex = Pages.IndexOf(SelectedPage) - 1;

                    if (prevIndex < 0)
                        prevIndex = Pages.Count - 1;

                    SelectedPage = Pages[prevIndex];
                    break;
                case 1:
                    if (Pages.Count == 0)
                        break;

                    var nextIndex = Pages.IndexOf(SelectedPage) + 1;

                    if (nextIndex >= Pages.Count)
                        nextIndex = 0;

                    SelectedPage = Pages[nextIndex];
                    break;
                case 2:
                    _book.SwitchTo(SelectedPage);
                    break;
                case 3:
                    _book.SwitchTo(InitialPage);
                    break;
            }
        }
    }
}