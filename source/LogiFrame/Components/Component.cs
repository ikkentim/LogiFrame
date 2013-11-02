//     Component.cs
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

using System;
using System.Diagnostics;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a renderable component for LogiFrame.Frame
    /// </summary>
    public abstract class Component : IDisposable
    {
        #region Fields

        private Bytemap _bytemap;
        private bool _hasChanged = true;
        private bool _isRendering;
        private Location _location = new Location();
        private Location _renderOffset = new Location();
        private Size _size = new Size();

        private bool _topEffect;
        private bool _transparent;
        private bool _visible = true;

        #endregion

        #region Constructor/Deconstructor

        /// <summary>
        ///     Initializes a new instance of the abstract LogiFrame.Components.Component class.
        /// </summary>
        internal Component()
        {
            _location.Changed += new EventHandler(location_LocationChanged);
            _size.Changed += new EventHandler(size_SizeChanged);
        }

        /// <summary>
        ///     Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        ~Component()
        {
            Dispose();
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when a property has changed or the LogiFrame.Components.Component needs to be refreshed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        ///     Occurs when the location of the LogiFrame.Components.Component has changed.
        /// </summary>
        public event EventHandler LocationChanged;

        #endregion

        #region Properties

        /// <summary>
        ///     The LogiFrame.Location this LogiFrame.Components.Comonent should
        ///     be rendered at in the parrent LogiFrame.Components.Container.
        /// </summary>
        public virtual Location Location
        {
            get { return _location; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Components.Component.Location cannot be set to null.");

                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (_location == value)
                    return;

                _location.Changed -= location_LocationChanged;
                value.Changed += location_LocationChanged;

                _location = value;

                if (LocationChanged != null)
                    LocationChanged(value, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Get the exact LogiFrame.Location this LogiFrame.Components.Component should
        ///     be rendered at in the parrent LogiFrame.Components.Container.
        /// </summary>
        public Location RenderLocation
        {
            get { return _location + _renderOffset; }
        }

        protected Location RenderOffset
        {
            get { return _renderOffset; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Components.Component.RenderOffset cannot be set to null.");

                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (_renderOffset == value)
                    return;

                _renderOffset.Changed -= location_LocationChanged;
                value.Changed += location_LocationChanged;

                _renderOffset = value;

                if (LocationChanged != null)
                    LocationChanged(value, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     The LogiFrame.Size of this LogiFrame.Components.Component.
        /// </summary>
        public virtual Size Size
        {
            get { return _size; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LogiFrame.Components.Component.Size cannot be set to null.");

                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                if (_size == value)
                    return;

                _size.Changed -= size_SizeChanged;
                value.Changed += size_SizeChanged;

                _size = value;
                HasChanged = true;
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Components.Component should have Bytemap.TopEffect enabled.
        /// </summary>
        public bool TopEffect
        {
            get { return _topEffect; }
            set
            {
                if (_topEffect == value)
                    return;

                _topEffect = value;
                HasChanged = true;
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Components.Component should have Bytemap.Transparent enabled.
        /// </summary>
        public bool Transparent
        {
            get { return _transparent; }
            set
            {
                if (_transparent == value)
                    return;

                _transparent = value;
                HasChanged = true;
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Components.Component should be visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible == value)
                    return;

                _visible = value;
                HasChanged = true;
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Component has been changed since the last render.
        /// </summary>
        public bool HasChanged
        {
            get { return _hasChanged; }
            set
            {
                if (Disposed || IsRendering)
                    return;

                _hasChanged = value;

                if (Changed != null && value)
                    Changed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Whether this LogiFrame.Component is in the process of rendering itself.
        /// </summary>
        public bool IsRendering
        {
            get { return _isRendering; }
            protected set { _isRendering = value; }
        }

        /// <summary>
        ///     Gets or sets the object that contains data about the Component.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     Whether this LogiFrame.Components.Component has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        ///     Gets the rendered LogiFrame.Bytemap of the current LogiFrame.Components.Component.
        /// </summary>
        public Bytemap Bytemap
        {
            get
            {
                if (!_hasChanged)
                    return Visible ? _bytemap : Bytemap.Empty;

                Refresh(false);
                _hasChanged = false;
                return Visible ? _bytemap : Bytemap.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            DisposeComponent();
            Disposed = true;
        }

        /// <summary>
        ///     Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary.
        /// </summary>
        /// <param name="forceRefresh">Force the LogiFrame.Components.Component.Bytemap being rerendered even if it hasn't changed</param>
        public virtual void Refresh(bool forceRefresh)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            if (!forceRefresh && !HasChanged)
                return;

            System.Diagnostics.Debug.WriteLine("[DEBUG] Rendering " + ToString() + " @ " + Location + " of " + Size);

            _isRendering = true;
            _bytemap = Render() ?? new Bytemap(1, 1);
            _bytemap.Transparent = Transparent;
            _bytemap.TopEffect = TopEffect;
            _isRendering = false;
        }

        /// <summary>
        ///     Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary.
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        ///     Stub for child components. This overridable method can be used to dispose resources.
        /// </summary>
        protected virtual void DisposeComponent()
        {
            //Stub
        }

        /// <summary>
        ///     Renders all grahpics of the current LogiFrame.Components.Component
        /// </summary>
        /// <returns>The rendered LogiFrame.Bytemap</returns>
        protected abstract Bytemap Render();

        #endregion

        #region Private methods

        //Callbacks
        private void size_SizeChanged(object sender, EventArgs e)
        {
            HasChanged = true;
        }

        private void location_LocationChanged(object sender, EventArgs e)
        {
            if (!Disposed && LocationChanged != null)
                LocationChanged(sender, e);
        }

        #endregion
    }
}