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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a collection of data.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class DiagramDataCollection<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        private readonly object _sync = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        public DiagramDataCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public DiagramDataCollection(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DiagramDataCollection(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DiagramDataCollection(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public DiagramDataCollection(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDataCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="comparer">The comparer.</param>
        public DiagramDataCollection(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public new TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                TValue oldValue;
                bool exist = TryGetValue(key, out oldValue);
                var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                lock (_sync)
                {
                    base[key] = value;
                }
                var newItem = new KeyValuePair<TKey, TValue>(key, value);
                OnCollectionChanged(exist
                    ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem,
                        Keys.ToList().IndexOf(key))
                    : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem,
                        Keys.ToList().IndexOf(key)));
            }
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        public new void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) return;
            var item = new KeyValuePair<TKey, TValue>(key, value);
            lock (_sync)
            {
                base.Add(key, value);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item,
                Keys.ToList().IndexOf(key)));
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2" />.
        /// </returns>
        public new bool Remove(TKey key)
        {
            TValue value;
            if (!TryGetValue(key, out value)) return false;
            var item = new KeyValuePair<TKey, TValue>(key, base[key]);
            bool result;
            lock (_sync)
            {
                result = base.Remove(key);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item,
                Keys.ToList().IndexOf(key)));
            return result;
        }

        /// <summary>
        /// Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2" />.
        /// </summary>
        public new void Clear()
        {
            lock (_sync)
            {
                base.Clear();
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }
}