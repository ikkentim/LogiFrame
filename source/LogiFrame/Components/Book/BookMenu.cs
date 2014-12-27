// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System;
using System.Drawing;
using System.Linq;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a drawable Menu for a LogiFrame.Components.Book.Book.
    /// </summary>
    public class BookMenu : Page
    {
        #region Fields

        protected Book Book;
        protected Line Line;
        protected Label PageTitle;
        protected Square SelectionSquare;
        private PageCollection<Page> _pages;
        private Page _selectedPage;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.Book.BookMenu class.
        /// </summary>
        /// <param name="book">The LogiFrame.Components.Book.Book this menu is working for.</param>
        public BookMenu(Book book)
        {
            Book = book;
            ParentComponent = book;

            PageTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                Location = new Location(1, 1),
                UseCache = true
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

        #endregion

        #region Properites

        /// <summary>
        ///     Gets or sets a Colection of LogiFrame.Components.Book.Page instances the user can browse trough.
        /// </summary>
        public PageCollection<Page> Pages
        {
            get { return _pages; }
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

                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the currently selected LogiFrame.Components.Book.Page.
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
        ///     Gets or sets the LogiFrame.Components.Book.Page to return to when the return button has been pressed.
        /// </summary>
        public Page InitialPage { get; set; }

        /// <summary>
        ///     Gets or sets the button which will select the previous LogiFrame.Components.Book.Page.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonPrevious { get; set; }

        /// <summary>
        ///     Gets or sets the button which will select the next LogiFrame.Components.Book.Page.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonNext { get; set; }

        /// <summary>
        ///     Gets or sets the button which will switch to the currently selected LogiFrame.Components.Book.Page.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonSelect { get; set; }

        /// <summary>
        ///     Gets or sets the button which will return to the initial LogiFrame.Components.Book.Page.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int ButtonReturn { get; set; }

        #endregion

        #region Methods

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

        protected override PageIcon GetPageIcon()
        {
            return new PageIcon();
        }

        public override string GetName()
        {
            return base.ToString();
        }

        protected override Bytemap Render()
        {
            if (Pages == null || Pages.Count == 0)
                return null;

            int selectedIndex = Pages.IndexOf(SelectedPage);

            if (selectedIndex == -1)
            {
                SelectedPage = Pages[0];
                selectedIndex = 0;
            }

            PageTitle.Text = SelectedPage.GetName();

            for (int i = 0; i < Pages.Count(); i++)
            {
                PageIcon icon = Pages[i].PageIcon;
                icon.Location.Set(72 + 20*(i - selectedIndex), 24);

                if (!Components.Contains(icon))
                    Components.Add(icon);
            }

            return base.Render();
        }

        private void pages_PageRemoved(object sender, ComponentChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void pages_PageAdded(object sender, ComponentChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        #endregion
    }
}