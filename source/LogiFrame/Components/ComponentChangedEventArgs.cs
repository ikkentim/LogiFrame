using System;

namespace LogiFrame.Components
{
    /// <summary>
    /// Provides data for the LogiFrame.Components.ComponentCollection.ComponentAdded and
    /// LogiFrame.Components.ComponentCollection.ComponentRemoved event.
    /// </summary>
    public class ComponentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The component that has been changed.
        /// </summary>
        public Component Component { get; set; }

        /// <summary>
        /// Initilizes a new instance of the LogiFrame.Components.ComponentChangedEventArgs
        /// </summary>
        /// <param name="component">The component that has been changed</param>
        public ComponentChangedEventArgs(Component component)
        {
            Component = component;
        }
    }
}
