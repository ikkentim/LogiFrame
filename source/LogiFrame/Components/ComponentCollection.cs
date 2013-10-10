using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LogiFrame.Components
{
    /// <summary>
    ///  Represents a dynamic collection of LogiFrame.Components.Component.
    /// </summary>
    /// <typeparam name="T">An instance of LogiFrame.Components.Component</typeparam>
    public sealed class ComponentCollection<T> : ObservableCollection<T>
    where T : Component
    {
        public ComponentCollection()
        {
            CollectionChanged += ComponentCollection_CollectionChanged;
        }

        /// <summary>
        /// Represents the method that handles a LogiFrame.Components.ComponentCollection.ComponentAdded and
        /// LogiFrame.Components.ComponentCollection.ComponentRemoved.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ComponentChangedEventArgs that contains the event data.</param>
        public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);
        
        /// <summary>
        /// Occurs when a LogiFrame.Components.Component has been added to the current collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentAdded;

        /// <summary>
        /// Occurs when a LogiFrame.Components.Component has been removed from the current collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentRemoved;

        private void ComponentCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null || (e.NewItems == null && e.OldItems == null))
                return;

            if (e.OldItems != null && ComponentRemoved != null)
                foreach (object obj in e.OldItems)
                    ComponentRemoved(this, new ComponentChangedEventArgs(obj as Component));
                
            if (e.NewItems != null && ComponentAdded != null)
                foreach (object obj in e.NewItems)
                    ComponentAdded(this, new ComponentChangedEventArgs(obj as Component));
        }
    }
}
