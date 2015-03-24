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