using System;
using System.Diagnostics;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a Component that is capable of holding multiple other LogiFrame.Components.Component instances.
    /// </summary>
    public class Container : Component
    {

        #region Fields

        private readonly ComponentCollection<Component> _components = new ComponentCollection<Component>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Container class.
        /// </summary>
        public Container()
        {
            _components.ComponentAdded += components_ComponentAdded;
            _components.ComponentRemoved += components_ComponentRemoved;
        }


        #endregion

        #region Properties
 
        /// <summary>
        /// A collection of LogiFrame.Components.Component instances that are rendered within the current LogiFrame.Components.Container.
        /// </summary>
        public ComponentCollection<Component> Components
        {
            get { return _components; }
        }

        #endregion

        #region Methods

        public override void Refresh(bool forceRefresh)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            IsRendering = true;
            if (forceRefresh)
                foreach (Component component in Components)
                    component.HasChanged = true;
            IsRendering = false;

            base.Refresh(forceRefresh);
        }

        protected override Bytemap Render()
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            Bytemap result = new Bytemap(Size);

            foreach (Component c in Components)
            {
                Bytemap bytemap = c.Bytemap;
                result.Merge(bytemap, c.RenderLocation);
            }

            return result;
        }

        protected override void DisposeComponent()
        {
            foreach (Component c in Components)
            {
                c.Changed -= Container_Changed;
                c.LocationChanged -= Container_Changed;
                c.Dispose();
            }
            Components.Clear();
        }

        #endregion

        #region Private methods

        //Calbacks
        private void components_ComponentRemoved(object sender, ComponentChangedEventArgs e)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            Debug.WriteLine(e.Component + " was removed from container.");

            e.Component.Changed -= Container_Changed;
            e.Component.LocationChanged -= Container_Changed;

            //Notify
            HasChanged = true;
        }

        private void components_ComponentAdded(object sender, ComponentChangedEventArgs e)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            Debug.WriteLine(e.Component + " was added to container.");

            e.Component.Changed += new EventHandler(Container_Changed);
            e.Component.LocationChanged += new EventHandler(Container_Changed);

            //Notify
            HasChanged = true;
        }

        private void Container_Changed(object sender, EventArgs e)
        {
            Debug.WriteLine(sender + " reportedly changed in container.");
            HasChanged = true;
        }

        #endregion
    }
}
