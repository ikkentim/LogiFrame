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

using System;

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
            _components.ComponentAdded += (sender, args) =>
            {
                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (args.Component.ParentComponent != null)
                    throw new ArgumentException("The Component has already been bound to a Container.");

                args.Component.Changed += Container_Changed;
                args.Component.LocationChanged += Container_Changed;
                args.Component.ParentComponent = this;

                OnChanged(EventArgs.Empty);
            };
            _components.ComponentRemoved += (sender, args) =>
            {
                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                args.Component.Changed -= Container_Changed;
                args.Component.LocationChanged -= Container_Changed;
                args.Component.ParentComponent = null;

                OnChanged(EventArgs.Empty);
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of LogiFrame.Components.Component instances that will be rendered
        /// within this LogiFrame.Components.Container.
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
                {
                    component.OnChanged(EventArgs.Empty);
                }
            IsRendering = false;

            base.Refresh(forceRefresh);
        }

        protected override Bytemap Render()
        {
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

        /// <summary>
        /// Listener for Component.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_Changed(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        #endregion
    }
}