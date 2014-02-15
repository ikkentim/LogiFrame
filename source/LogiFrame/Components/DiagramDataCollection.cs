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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace LogiFrame.Components
{
    public class DiagramDataCollection<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        private readonly object _sync = new object();

        public DiagramDataCollection()
        {
        }

        public DiagramDataCollection(int capacity) : base(capacity)
        {
        }

        public DiagramDataCollection(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public DiagramDataCollection(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public DiagramDataCollection(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        public DiagramDataCollection(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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

        public new void Clear()
        {
            lock (_sync)
            {
                base.Clear();
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }
}