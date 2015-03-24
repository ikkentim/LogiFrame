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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LogiFrame.Components;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a watchable collection of the specified type.
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    public sealed class WatchableCollection<T> : ObservableCollection<T>
    {
        private List<T> _components = new List<T>();

        /// <summary>
        ///     Occurs when an item has been added.
        /// </summary>
        public event EventHandler<ItemEventArgs<T>> ItemAdded;

        /// <summary>
        ///     Occurs when an item has been removed.
        /// </summary>
        public event EventHandler<ItemEventArgs<T>> ItemRemoved;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset && ItemRemoved != null)
                foreach (T obj in _components)
                    ItemRemoved(this, new ItemEventArgs<T>(obj));

            if (e.OldItems != null && ItemRemoved != null)
                foreach (object obj in e.OldItems)
                    ItemRemoved(this, new ItemEventArgs<T>((T) obj));

            if (e.NewItems != null && ItemAdded != null)
                foreach (object obj in e.NewItems)
                    ItemAdded(this, new ItemEventArgs<T>((T) obj));

            _components = new List<T>(this);
            base.OnCollectionChanged(e);
        }


        /// <summary>
        /// Performs an implicit conversion from <see cref="List{T}"/> to <see cref="WatchableCollection{T}"/>.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator WatchableCollection<T>(List<T> list)
        {
            var col = new WatchableCollection<T>();
            foreach (T o in list)
                col.Add(o);
            return col;
        }
    }
}