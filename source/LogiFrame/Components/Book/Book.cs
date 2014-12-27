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
using System.Linq;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a browsable Component which can contain multiple
    ///     instances of the LogiFrame.Components.Book.Page class.
    /// </summary>
    public sealed class Book : Component
    {
        #region Fields

        private Page _activePage;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.Book.Book class.
        /// </summary>
        /// <param name="frame">The LogiFrame.Frame this LogiFrame.Components.Book.Book should render in.</param>
        public Book(Frame frame)
        {
            base.Size = new Size((int) LgLcd.LglcdBmpWidth, (int) LgLcd.LglcdBmpHeight);
            Pages = new PageCollection<Page>();
            Pages.PageAdded += (sender, args) =>
            {
                if (args.Component.ParentComponent != null)
                    throw new ArgumentException("The Page has already been bound to a Container.");
                if (args.Component != null) args.Component.ParentComponent = this;
            };
            Pages.PageRemoved +=
                (sender, args) => { if (args.Component != null) args.Component.ParentComponent = null; };

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

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the currently active page of this LogiFrame.Components.Book.Book.
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
        ///     Gets a Collection to add browsable LogiFrame.Components.Book.Page instances to.
        /// </summary>
        public PageCollection<Page> Pages { get; private set; }

        /// <summary>
        ///     Gets the LogiFrame.Location this LogiFrame.Components.Book.Book should
        ///     be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Book cannot be changed."); }
        }


        /// <summary>
        ///     Gets the LogiFrame.Size of this LogiFrame.Components.Book.Book.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Book cannot be changed."); }
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Components.Book.BookMenu this LogiFrame.Components.Book.Book should use.
        /// </summary>
        public BookMenu BookMenu { get; set; }

        /// <summary>
        ///     Gets or sets the menu button which opens the BookMenu of this LogiFrame.Components.Book.Book.
        ///     Set to -1 to disable this functionality.
        /// </summary>
        public int MenuButton { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Switches to the given LogiFrame.Components.Book.Page.
        /// </summary>
        /// <param name="page">The LogiFrame.Components.Book.Page to switch to.</param>
        public void SwitchTo(Page page)
        {
            ActivePage = page;
        }

        /// <summary>
        ///     Switches to the first instance of the given type in Pages.
        /// </summary>
        /// <param name="pageType">The type of the LogiFrame.Components.Book.Page to switch to.</param>
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
        ///     Switches to the first instance of the given type in Pages.
        /// </summary>
        /// <typeparam name="T">The type of the LogiFrame.Components.Book.Page to switch to.</typeparam>
        public void SwitchTo<T>() where T : Page
        {
            SwitchTo(typeof (T));
        }

        /// <summary>
        ///     Shows the BookMenu.
        /// </summary>
        public void ShowMenu()
        {
            BookMenu.Pages =
                Pages.Where(p => p.IsBrowsable()).ToList();
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
            OnChanged(EventArgs.Empty);
        }

        #endregion
    }
}