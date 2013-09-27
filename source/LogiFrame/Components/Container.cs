using System;
using LogiFrame;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a Component that is capable of holding multiple other LogiFrame.Components.Component instances.
    /// </summary>
    public class Container : Component
    {

        #region Fields

        private ComponentCollection<Component> components = new ComponentCollection<Component>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Container class.
        /// </summary>
        public Container()
            : base()
        {
            Components.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Components_CollectionChanged);
        }

        #endregion

        #region Properties
 
        /// <summary>
        /// A collection of LogiFrame.Components.Component instances that are rendered within the current LogiFrame.Components.Container.
        /// </summary>
        public ComponentCollection<Component> Components
        {
            get { return components; }
        }

        #endregion

        #region Methods

        protected override Bytemap Render()
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            Bytemap result = new Bytemap(Size);

            foreach (Component c in Components)
                result.Merge(c.Bytemap, c.Location);

            return result;
        }

        protected override void DisposeComponent()
        {
            foreach (Component c in Components)
            {
                c.Changed -= Container_ComponentChanged;
                c.LocationChanged -= Container_LocationChanged;
                c.Dispose();
            }
            Components.Clear();
        }

        #endregion

        #region Private methods

        private void Components_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e == null || (e.NewItems == null && e.OldItems == null))
                return;

            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            //Add events
            if (e.NewItems != null)
                foreach (object obj in e.NewItems)
                {
                    (obj as Component).Changed += new EventHandler(Container_Changed);
                    (obj as Component).LocationChanged += new EventHandler(Container_Changed);
                }
            //Remove events
            if (e.OldItems != null)
                foreach (object obj in e.OldItems)
                {
                    (obj as Component).Changed -= Container_Changed;
                    (obj as Component).LocationChanged -= Container_Changed;
                }

            //Notify
            HasChanged = true;
        }

        //Callbacks
        private void Container_Changed(object sender, EventArgs e)
        {
            HasChanged = true;
        }

        #endregion
    }
}
