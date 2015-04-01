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

using System;
using System.Drawing;
using System.Linq;
using LogiFrame.Collections;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a drawable menu for a <see cref="Book" />.
    /// </summary>
    public class BookMenu : Page
    {
        protected Book Book;
        protected Line Line;
        protected Label PageTitle;
        protected Square SelectionSquare;
        private WatchableCollection<Page> _pages;
        private Page _selectedPage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BookMenu" /> class.
        /// </summary>
        /// <param name="book">The book the menu is working for.</param>
        public BookMenu(Book book)
        {
            Book = book;
            ParentComponent = book;

            PageTitle = new Label
            {
                IsAutoSize = true,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                Location = new Location(1, 1),
                IsUseCache = true
            };

            SelectionSquare = new Square
            {
                Location = new Location(70, 22),
                Size = new Size(20, 20)
            };

            Line = new Line
            {
                Start = new Location(0, 20),
                End = new Location(160, 20)
            };

            Components.Add(PageTitle);
            Components.Add(SelectionSquare);
            Components.Add(Line);

            ButtonPrevious = 0;
            ButtonNext = 1;
            ButtonSelect = 2;
            ButtonReturn = 3;
        }

        /// <summary>
        ///     Gets or sets a collection of <see cref="Page" /> instances the user can browse trough.
        /// </summary>
        public WatchableCollection<Page> Pages
        {
            get { return _pages; }
            set
            {
                if (_pages != null)
                {
                    _pages.ItemAdded -= pages_PageAdded;
                    _pages.ItemRemoved -= pages_PageRemoved;
                }

                _pages = value;

                if (_pages == null) return;

                _pages.ItemAdded += pages_PageAdded;
                _pages.ItemRemoved += pages_PageRemoved;

                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the currently selected <see cref="Page" />.
        /// </summary>
        public Page SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (_selectedPage == value)
                    return;

                _selectedPage = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Page" /> to return to when the return button has been pressed.
        /// </summary>
        public Page InitialPage { get; set; }

        /// <summary>
        ///     Gets or sets the button which will select the previous <see cref="Page" />.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonPrevious { get; set; }

        /// <summary>
        ///     Gets or sets the button which will select the next <see cref="Page" />.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonNext { get; set; }

        /// <summary>
        ///     Gets or sets the button which will switch to the currently selected <see cref="Page" />.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonSelect { get; set; }

        /// <summary>
        ///     Gets or sets the button which will return to the initial <see cref="Page" />.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonReturn { get; set; }

        /// <summary>
        ///     Gets the icon.
        /// </summary>
        public override PageIcon Icon
        {
            get { return null; }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public override string Name
        {
            get { return ToString(); }
        }

        public override void OnButtonPressed(ButtonEventArgs e)
        {
            if (Pages.Count == 0)
                return;

            if (e.Button == ButtonPrevious)
            {
                int prevIndex = Pages.IndexOf(SelectedPage) - 1;

                if (prevIndex < 0)
                    prevIndex = Pages.Count - 1;

                SelectedPage = Pages[prevIndex];
            }
            else if (e.Button == ButtonNext)
            {
                int nextIndex = Pages.IndexOf(SelectedPage) + 1;

                if (nextIndex >= Pages.Count)
                    nextIndex = 0;

                SelectedPage = Pages[nextIndex];
            }
            else if (e.Button == ButtonSelect)
            {
                Book.SwitchTo(SelectedPage);
            }
            else if (e.Button == ButtonReturn)
            {
                Book.SwitchTo(InitialPage);
            }
        }

        /// <summary>
        ///     Renders all graphics of this <see cref="BookMenu" />.
        /// </summary>
        /// <returns>
        ///     The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            if (Pages == null || Pages.Count == 0)
                return Snapshot.Empty;

            int selectedIndex = Pages.IndexOf(SelectedPage);

            if (selectedIndex == -1)
            {
                SelectedPage = Pages[0];
                selectedIndex = 0;
            }

            PageTitle.Text = SelectedPage.Name;

            for (int i = 0; i < Pages.Count(); i++)
            {
                PageIcon icon = Pages[i].Icon;
                icon.Location = new Location(72 + 20*(i - selectedIndex), 24);

                if (!Components.Contains(icon))
                    Components.Add(icon);
            }

            return base.Render();
        }

        private void pages_PageRemoved(object sender, ItemEventArgs<Page> e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void pages_PageAdded(object sender, ItemEventArgs<Page> e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}