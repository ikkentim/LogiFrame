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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a dynamic collection of LogiFrame.Components.Book.Page.
    /// </summary>
    /// <typeparam name="T">An instance of LogiFrame.Components.Book.Page.</typeparam>
    public sealed class PageCollection<T> : ObservableCollection<T>, IDisposable
        where T : Page
    {
        /// <summary>
        ///     Represents the method that handles a LogiFrame.Components.ComponentCollection.ComponentAdded and
        ///     LogiFrame.Components.ComponentCollection.ComponentRemoved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ComponentChangedEventArgs that contains the event data.</param>
        public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);

        private List<Page> _pages = new List<Page>(); //Dirty solution for Reset not parsing OldItems.

        public void Dispose()
        {
            foreach (T page in this)
            {
                page.Dispose();
            }
            Clear();
        }

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Book.Page has been added to this collection.
        /// </summary>
        public event ComponentChangedEventHandler PageAdded;

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Book.Page has been removed from this collection.
        /// </summary>
        public event ComponentChangedEventHandler PageRemoved;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset && PageRemoved != null)
                foreach (Page obj in _pages)
                    PageRemoved(this, new ComponentChangedEventArgs(obj));

            if (e.OldItems != null && PageRemoved != null)
                foreach (object obj in e.OldItems)
                    PageRemoved(this, new ComponentChangedEventArgs(obj as Component));

            if (e.NewItems != null && PageAdded != null)
                foreach (object obj in e.NewItems)
                    PageAdded(this, new ComponentChangedEventArgs(obj as Component));

            _pages = new List<Page>(this);
            base.OnCollectionChanged(e);
        }

        public static implicit operator PageCollection<T>(List<T> list)
        {
            var col = new PageCollection<T>();
            foreach (T o in list)
                col.Add(o);
            return col;
        }
    }
}