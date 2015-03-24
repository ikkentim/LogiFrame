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

using System;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a Component that is capable of holding multiple other <see cref="Component" /> instances.
    /// </summary>
    public class Container : Component
    {
        private readonly WatchableCollection<Component> _components = new WatchableCollection<Component>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
        {
            _components.ItemAdded += (sender, args) =>
            {
                if (IsDisposed) return;

                if (args.Item.ParentComponent != null)
                    throw new ArgumentException("The Component has already been bound to a Container.");

                args.Item.Changed += Container_Changed;
                args.Item.LocationChanged += Container_Changed;
                args.Item.ParentComponent = this;

                OnChanged(EventArgs.Empty);
            };
            _components.ItemRemoved += (sender, args) =>
            {
                if (IsDisposed) return;

                args.Item.Changed -= Container_Changed;
                args.Item.LocationChanged -= Container_Changed;
                args.Item.ParentComponent = null;

                OnChanged(EventArgs.Empty);
            };
        }

        /// <summary>
        ///     Gets the components.
        /// </summary>
        public WatchableCollection<Component> Components
        {
            get { return _components; }
        }

        /// <summary>
        ///     Refreshes the <see cref="Bytemap" /> and renders it if necessary.
        /// </summary>
        /// <param name="forceRefresh">Forces the <see cref="Bytemap" /> being rerendered even if it hasn't changed when True.</param>
        /// <exception cref="System.ObjectDisposedException">Resource was disposed.</exception>
        public override void Refresh(bool forceRefresh)
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Resource was disposed.");

            IsRendering = true;
            if (forceRefresh)
                foreach (Component component in Components)
                    component.OnChanged(EventArgs.Empty);
            IsRendering = false;

            base.Refresh(forceRefresh);
        }

        /// <summary>
        ///     Renders all graphics of this <see cref="Container" />.
        /// </summary>
        /// <returns>The rendered <see cref="Bytemap" />.</returns>
        protected override Bytemap Render()
        {
            var result = new Bytemap(Size);

            foreach (Component c in Components.ToList())
            {
                Bytemap bytemap = c.Bytemap;
                result.Merge(bytemap, c.RenderLocation);
            }

            return result;
        }
        
        #region Overrides of Component

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Component c in Components)
                {
                    c.Changed -= Container_Changed;
                    c.LocationChanged -= Container_Changed;
                    c.Dispose();
                }
                Components.Clear();
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Container_Changed(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}