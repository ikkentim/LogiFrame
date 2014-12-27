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

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Component has been added to this collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentAdded;

        /// <summary>
        ///     Occurs when a LogiFrame.Components.Component has been removed from this collection.
        /// </summary>
        public event ComponentChangedEventHandler ComponentRemoved;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset && ComponentRemoved != null)
                foreach (Component obj in _components)
                    ComponentRemoved(this, new ComponentChangedEventArgs(obj));

            if (e.OldItems != null && ComponentRemoved != null)
                foreach (object obj in e.OldItems)
                    ComponentRemoved(this, new ComponentChangedEventArgs(obj as Component));

            if (e.NewItems != null && ComponentAdded != null)
                foreach (object obj in e.NewItems)
                    ComponentAdded(this, new ComponentChangedEventArgs(obj as Component));

            _components = new List<Component>(this);
            base.OnCollectionChanged(e);
        }
    }
}