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

using System;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a Component that is capable of holding multiple other LogiFrame.Components.Component instances.
    /// </summary>
    public class Container : Component
    {
        #region Fields

        private readonly ComponentCollection<Component> _components = new ComponentCollection<Component>();

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.Container class.
        /// </summary>
        public Container()
        {
            _components.ComponentAdded += (sender, args) =>
            {
                if (IsDisposed) return;

                if (args.Component.ParentComponent != null)
                    throw new ArgumentException("The Component has already been bound to a Container.");

                args.Component.Changed += Container_Changed;
                args.Component.LocationChanged += Container_Changed;
                args.Component.ParentComponent = this;

                OnChanged(EventArgs.Empty);
            };
            _components.ComponentRemoved += (sender, args) =>
            {
                if (IsDisposed) return;

                args.Component.Changed -= Container_Changed;
                args.Component.LocationChanged -= Container_Changed;
                args.Component.ParentComponent = null;

                OnChanged(EventArgs.Empty);
            };
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a collection of LogiFrame.Components.Component instances that will be rendered
        ///     within this LogiFrame.Components.Container.
        /// </summary>
        public ComponentCollection<Component> Components
        {
            get { return _components; }
        }

        #endregion

        #region Methods

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
        ///     Listener for Component.Changed.
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