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
using System.ComponentModel;
using System.Diagnostics;

namespace LogiFrame.Components
{
    /// <summary>
    /// An abstract base class that provides functionality for the LogiFrame.Frame class.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public abstract class Component : IDisposable
    {
        #region Fields

        private Bytemap _bytemap;
        private Location _location = new Location();
        private Location _renderOffset = new Location();
        private Size _size = new Size();

        private bool _topEffect;
        private bool _transparent;
        private bool _visible = true;

        #endregion

        #region Constructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the abstract LogiFrame.Components.Component class.
        /// </summary>
        internal Component()
        {
            _location.Changed += location_Changed;
            _size.Changed += size_Changed;
        }

        /// <summary>
        /// Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        ~Component()
        {
            Dispose();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property has changed or the LogiFrame.Components.Component needs to be refreshed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Occurs when the location of the LogiFrame.Components.Component has changed.
        /// </summary>
        public event EventHandler LocationChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the LogiFrame.Size of an LCD screen.
        /// </summary>
        public static Size LCDSize
        {
            get { return new Size((int) LgLcd.LglcdBmpWidth, (int) LgLcd.LglcdBmpHeight); }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Location this LogiFrame.Components.Component should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public virtual Location Location
        {
            get { return _location; }
            set
            {
                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _location.Changed -= location_Changed;

                if (SwapProperty(ref _location, value, false))
                    OnLocationChanged(EventArgs.Empty);

                _location.Changed += location_Changed;
            }
        }

        /// <summary>
        /// Gets the exact LogiFrame.Location this LogiFrame.Components.Component should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public Location RenderLocation
        {
            get { return _location + _renderOffset; }
        }

        /// <summary>
        /// Gets or sets the offset from the actual Location this LogiFrame.Components.Component 
        /// should be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        protected Location RenderOffset
        {
            get { return _renderOffset; }
            set
            {
                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _renderOffset.Changed -= location_Changed;

                if (SwapProperty(ref _renderOffset, value, false))
                    OnLocationChanged(EventArgs.Empty);

                _renderOffset.Changed += location_Changed;
            }
        }

        /// <summary>
        /// Gets or sets the LogiFrame.Size of this LogiFrame.Components.Component.
        /// </summary>
        public virtual Size Size
        {
            get { return _size; }
            set
            {
                if (Disposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _size.Changed -= size_Changed;

                if (SwapProperty(ref _size, value, false))
                    OnChanged(EventArgs.Empty);

                _size.Changed += size_Changed;
            }
        }

        /// <summary>
        /// Gets or sets whether this LogiFrame.Components.Component should have Bytemap.TopEffect enabled when rendered.
        /// </summary>
        public bool TopEffect
        {
            get { return _topEffect; }
            set
            {
                if (SwapProperty(ref _topEffect, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether this LogiFrame.Components.Component should have Bytemap.Transparent enabled when rendered.
        /// </summary>
        public bool Transparent
        {
            get { return _transparent; }
            set
            {
                if (SwapProperty(ref _transparent, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether this LogiFrame.Components.Component should be visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (SwapProperty(ref _visible, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether this LogiFrame.Component has been changed since the lastest render.
        /// </summary>
        public bool HasChanged { get; private set; }

        /// <summary>
        /// Gets or sets(protected) whether this LogiFrame.Component is in the process of rendering itself.
        /// When IsRendering is True, the component won't be calling listeners of Changed when HasChanged is set to True.
        /// </summary>
        public bool IsRendering { get; protected set; }

        /// <summary>
        /// Gets or sets the object that contains data about the Component.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets whether this LogiFrame.Components.Component has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the rendered LogiFrame.Bytemap of this LogiFrame.Components.Component.
        /// </summary>
        public Bytemap Bytemap
        {
            get
            {
                if (!HasChanged)
                    return Visible ? _bytemap : Bytemap.Empty;

                Refresh(false);
                HasChanged = false;
                return Visible ? _bytemap : Bytemap.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the parent LogiFrame.Components.Component.
        /// </summary>
        public Component ParentComponent { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Releases all resources used by LogiFrame.Components.Comonent.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            DisposeComponent();
            Disposed = true;
        }

        /// <summary>
        /// Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary.
        /// </summary>
        /// <param name="forceRefresh">Forces the LogiFrame.Components.Component.Bytemap being rerendered even if it hasn't changed when True.</param>
        public virtual void Refresh(bool forceRefresh)
        {
            if (Disposed)
                throw new ObjectDisposedException("Resource was disposed.");

            if (!forceRefresh && !HasChanged)
                return;

            IsRendering = true;

            Debug.WriteLine("[LogiFrame] Rendering " + this + " : parent=" + ParentComponent);

            _bytemap = Size.Width == 0 || Size.Height == 0 ? new Bytemap(1, 1) : (Render() ?? new Bytemap(1, 1));
            _bytemap.Transparent = Transparent;
            _bytemap.TopEffect = TopEffect;

            IsRendering = false;
        }

        /// <summary>
        /// Refreshes the LogiFrame.Components.Component.Bytemap and renders it if nececcary.
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        /// Swaps property with given value.
        /// </summary>
        /// <param name="field">The value of the field.</param>
        /// <param name="value">The value to swap it with.</param>
        /// <param name="allowNull">Whether null values are allowed.</param>
        /// <returns>Whether the field's value has changed.</returns>
        protected bool SwapProperty<T>(ref T field, T value, bool allowNull)
        {
            var fa = field;
            var va = value;

            if (va == null && !allowNull)
                throw new NullReferenceException("Property cannot be set to null.");

            if (fa != null && fa.Equals(va))
                return false;

            field = value;
            OnChanged(EventArgs.Empty);

            return true;
        }

        public T GetParentComponent<T>() where T : Component
        {
            var c = this;
            while ((c = c.ParentComponent) != null)
                if (c is T)
                    return c as T;
            return null;
        }

        /// <summary>
        /// Called when the location has changed.
        /// </summary>
        /// <param name="e">Contains information about the event.</param>
        public virtual void OnLocationChanged(EventArgs e)
        {
            if (LocationChanged != null)
                LocationChanged(this, e);
        }

        /// <summary>
        /// Called when the component has changed.
        /// </summary>
        /// <param name="e">Contains information about the event.</param>
        public virtual void OnChanged(EventArgs e)
        {
            if (IsRendering)
                return;

            HasChanged = true;

            if (Changed != null)
                Changed(this, e);
        }

        /// <summary>
        /// Stub for child components. This overridable method can be used to dispose resources.
        /// </summary>
        protected virtual void DisposeComponent()
        {
            //Stub
        }

        /// <summary>
        /// Renders all grahpics of this LogiFrame.Components.Component.
        /// </summary>
        /// <returns>The rendered LogiFrame.Bytemap.</returns>
        protected abstract Bytemap Render();

        /// <summary>
        /// Listens to Size.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void size_Changed(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Listens to Location.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void location_Changed(object sender, EventArgs e)
        {
            if (!Disposed && LocationChanged != null)
                LocationChanged(sender, e);
        }

        #endregion
    }
}