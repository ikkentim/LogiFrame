//     ComponentCollection.cs
// 
//     LogiFrame rendering library.
//     Copyright (C) 2013  Tim Potze
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a dynamic collection of LogiFrame.Components.Component.
    /// </summary>
    /// <typeparam name="T">An instance of LogiFrame.Components.Component</typeparam>
    public sealed class ComponentCollection<T> : ObservableCollection<T>
        where T : Component
    {
        /// <summary>
        ///     Represents the method that handles a LogiFrame.Components.ComponentCollection.ComponentAdded and
        ///     LogiFrame.Components.ComponentCollection.ComponentRemoved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ComponentChangedEventArgs that contains the event data.</param>
        public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);

        private List<Component> _components = new List<Component>(); //Dirty solution for Reset not parsing OldItems.

        public ComponentCollection()
        {
            CollectionChanged += ComponentCollection_CollectionChanged;
        }

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Component has been added to this collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentAdded;

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Component has been removed from this collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentRemoved;

        /// <summary>
        ///     Listener for ObservableCollection.CollectionChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComponentCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
                return;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (ComponentRemoved != null)
                    foreach (Component obj in _components)
                        ComponentRemoved(this, new ComponentChangedEventArgs(obj as Component));

                //Reset backup list
                _components = new List<Component>();
                return;
            }

            if (e.OldItems != null && ComponentRemoved != null)
            {
                foreach (object obj in e.OldItems)
                    ComponentRemoved(this, new ComponentChangedEventArgs(obj as Component));

                _components = new List<Component>(this);
            }

            if (e.NewItems != null && ComponentAdded != null)
            {
                foreach (object obj in e.NewItems)
                    ComponentAdded(this, new ComponentChangedEventArgs(obj as Component));
                _components = new List<Component>(this);
            }
        }
    }
}