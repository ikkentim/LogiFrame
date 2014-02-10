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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LogiFrame.Components.Book
{
    /// <summary>
    /// Represents a dynamic collection of LogiFrame.Components.Book.Page.
    /// </summary>
    /// <typeparam name="T">An instance of LogiFrame.Components.Book.Page.</typeparam>
    public sealed class PageCollection<T> : ObservableCollection<T>, IDisposable
        where T : Page
    {
        /// <summary>
        /// Represents the method that handles a LogiFrame.Components.ComponentCollection.ComponentAdded and
        /// LogiFrame.Components.ComponentCollection.ComponentRemoved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ComponentChangedEventArgs that contains the event data.</param>
        public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);

        private List<Page> _pages = new List<Page>(); //Dirty solution for Reset not parsing OldItems.

        public PageCollection()
        {
            CollectionChanged += ComponentCollection_CollectionChanged;
        }

        public void Dispose()
        {
            foreach (var page in this)
            {
                page.Dispose();
            }
            Clear();
        }

        /// <summary>
        /// Occurs when a LogiFrame.Components.Book.Page has been added to this collection.
        /// </summary>
        public event ComponentChangedEventHandler PageAdded;

        /// <summary>
        /// Occurs when a LogiFrame.Components.Book.Page has been removed from this collection.
        /// </summary>
        public event ComponentChangedEventHandler PageRemoved;

        /// <summary>
        /// Listener for ObservableCollection.CollectionChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComponentCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (PageRemoved != null)
                    foreach (Page obj in _pages)
                        PageRemoved(this, new ComponentChangedEventArgs(obj));

                //Reset backup list
                _pages = new List<Page>();
                return;
            }

            if (e.OldItems != null && PageRemoved != null)
            {
                foreach (object obj in e.OldItems)
                    PageRemoved(this, new ComponentChangedEventArgs(obj as Component));

                _pages = new List<Page>(this);
            }

            if (e.NewItems != null && PageAdded != null)
            {
                foreach (object obj in e.NewItems)
                    PageAdded(this, new ComponentChangedEventArgs(obj as Component));

                _pages = new List<Page>(this);
            }
        }

        public static implicit operator PageCollection<T>(List<T> list)
        {
            var col = new PageCollection<T>();
            foreach (var o in list)
                col.Add(o);
            return col;
        }
    }
}