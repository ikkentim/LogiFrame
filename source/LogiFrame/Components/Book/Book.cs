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
using System.Linq;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a browsable Component which can contain multiple
    ///     instances of the <see cref="Page" />.
    /// </summary>
    public sealed class Book : Component
    {
        private Page _activePage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Book" /> class.
        /// </summary>
        /// <param name="frame">The <see cref="Frame" /> the <see cref="Book" /> should render in.</param>
        public Book(Frame frame)
        {
            base.Size = new Size((int) LgLcd.LgLcdBitmapWidth, (int) LgLcd.LgLcdBitmapHeight);
            Pages = new WatchableCollection<Page>();
            Pages.ItemAdded += (sender, args) =>
            {
                if (args.Item.ParentComponent != null)
                    throw new ArgumentException("The Page has already been bound to a Container.");
                if (args.Item != null) args.Item.ParentComponent = this;
            };
            Pages.ItemRemoved +=
                (sender, args) => { if (args.Item != null) args.Item.ParentComponent = null; };

            frame.Components.Add(this);

            frame.ButtonDown += (sender, args) =>
            {
                if (args.Button == MenuButton && !(ActivePage is BookMenu))
                {
                    ShowMenu();
                    return;
                }
                if (_activePage != null) _activePage.OnButtonPressed(args);
            };
            frame.ButtonUp += (sender, args) =>
            {
                if (args.Button == MenuButton) return;
                if (_activePage != null) _activePage.OnButtonReleased(args);
            };

            MenuButton = -1;
            BookMenu = new BookMenu(this);
        }

        /// <summary>
        ///     Gets or sets the active <see cref="Page" /> of this <see cref="Book" />.
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
                    _activePage.OnHide(EventArgs.Empty);
                }


                if (value != null)
                {
                    value.Changed += page_Changed;
                    value.OnShow(EventArgs.Empty);
                }

                _activePage = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets a Collection of <see cref="Page">pages</see> available in this <see cref="Book" />.
        /// </summary>
        public WatchableCollection<Page> Pages { get; private set; }

        /// <summary>
        ///     Gets or sets the <see cref="Location" /> this <see cref="Component" /> should be rendered at within the parent
        ///     <see cref="Container" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Location of a LogiFrame.Components.Book.Book cannot be changed.</exception>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Book cannot be changed."); }
        }


        /// <summary>
        ///     Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Size of a LogiFrame.Components.Book.Book cannot be changed.</exception>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Book cannot be changed."); }
        }

        /// <summary>
        ///     Gets or sets the <see cref="BookMenu" /> this <see cref="Book" /> should use.
        /// </summary>
        public BookMenu BookMenu { get; set; }

        /// <summary>
        ///     Gets or sets the menu button which opens the <see cref="BookMenu" /> of this <see cref="Book" />.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int MenuButton { get; set; }

        /// <summary>
        ///     Switches to the specified <paramref name="page" />.
        /// </summary>
        /// <param name="page">The <see cref="Page" /> to switch to.</param>
        public void SwitchTo(Page page)
        {
            ActivePage = page;
        }

        /// <summary>
        ///     Switches to the first occurrence of the given type in <see cref="Pages" />.
        /// </summary>
        /// <param name="pageType">The type of the <see cref="Page" /> to switch to.</param>
        public void SwitchTo(Type pageType)
        {
            foreach (Page page in Pages.Where(page => page.GetType() == pageType))
            {
                SwitchTo(page);
                OnChanged(EventArgs.Empty);
                break;
            }
        }

        /// <summary>
        ///     Switches to the first occurrence of the given type in <see cref="Pages" />.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Page" /> to switch to.</typeparam>
        public void SwitchTo<T>() where T : Page
        {
            SwitchTo(typeof (T));
        }

        /// <summary>
        ///     Switches to the menu.
        /// </summary>
        public void ShowMenu()
        {
            BookMenu.Pages =
                Pages.Where(p => p.IsBrowsable).ToList();
            BookMenu.SelectedPage =
                BookMenu.InitialPage = ActivePage;

            ActivePage = BookMenu;
        }

        protected override Bytemap Render()
        {
            return _activePage == null ? null : _activePage.Bytemap;
        }

        private void page_Changed(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}