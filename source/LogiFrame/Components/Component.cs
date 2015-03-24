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
using System.ComponentModel;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Provides functionality for a drawable component.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public abstract class Component : IDisposable
    {
        private Bytemap _bytemap;
        private bool _hasChanged;
        private Location _location = new Location();
        private Location _renderOffset = new Location();
        private Size _size = new Size();

        private bool _isTopEffectEnabled;
        private bool _isTransparent;
        private bool _isVisible = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Component" /> class.
        /// </summary>
        protected Component()
        {
            _location.Changed += location_Changed;
            _size.Changed += size_Changed;
        }

        /// <summary>
        ///     Gets or sets the <see cref="Location" /> this <see cref="Component" /> should be rendered at within the parent
        ///     <see cref="Container" />.
        /// </summary>
        public virtual Location Location
        {
            get { return _location; }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _location.Changed -= location_Changed;

                if (SwapProperty(ref _location, value))
                    OnLocationChanged(EventArgs.Empty);

                _location.Changed += location_Changed;
            }
        }

        /// <summary>
        ///     Gets the exact <see cref="Location" /> this <see cref="Component" /> should be rendered at within the parent
        ///     <see cref="Container" />.
        /// </summary>
        public Location RenderLocation
        {
            get { return _location + _renderOffset; }
        }

        /// <summary>
        ///     Gets or sets the offset from the actual <see cref="Location" /> this <see cref="Component" /> should be rendered at
        ///     within the parent <see cref="Container" />.
        /// </summary>
        protected Location RenderOffset
        {
            get { return _renderOffset; }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _renderOffset.Changed -= location_Changed;

                if (SwapProperty(ref _renderOffset, value))
                    OnLocationChanged(EventArgs.Empty);

                _renderOffset.Changed += location_Changed;
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        public virtual Size Size
        {
            get { return _size; }
            set
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("Resource was disposed.");

                _size.Changed -= size_Changed;
                SwapProperty(ref _size, value);
                _size.Changed += size_Changed;
            }
        }

        /// <summary>
        ///     Gets or sets whether this <see cref="Component" /> should have Bytemap.IsTopEffectEnabled enabled when rendered.
        /// </summary>
        public bool IsTopEffectEnabled
        {
            get { return _isTopEffectEnabled; }
            set { SwapProperty(ref _isTopEffectEnabled, value); }
        }

        /// <summary>
        ///     Gets or sets whether this <see cref="Component" /> should have Bytemap.IsTransparent enabled when rendered.
        /// </summary>
        public bool IsTransparent
        {
            get { return _isTransparent; }
            set { SwapProperty(ref _isTransparent, value); }
        }

        /// <summary>
        ///     Gets or sets whether this <see cref="Component" /> should be visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SwapProperty(ref _isVisible, value); }
        }

        /// <summary>
        ///     Gets or sets(protected) whether this <see cref="Component" /> is in the process of rendering itself.
        ///     When IsRendering is True, the component won't be calling listeners of Changed when a refresh is
        ///     requested.
        /// </summary>
        public bool IsRendering { get; protected set; }

        /// <summary>
        ///     Gets or sets the object that contains data about this <see cref="Component" />.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     Gets whether this <see cref="Component" /> has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Gets the rendered <see cref="LogiFrame.Bytemap" /> of this <see cref="Component" />.
        /// </summary>
        public Bytemap Bytemap
        {
            get
            {
                if (!_hasChanged)
                    return IsVisible ? _bytemap : Bytemap.Empty;

                Refresh(false);
                _hasChanged = false;
                return IsVisible ? _bytemap : Bytemap.Empty;
            }
        }

        /// <summary>
        ///     Gets or sets the parent <see cref="Component" />.
        /// </summary>
        public Component ParentComponent { get; set; }

        /// <summary>
        ///     Releases all resources used by <see cref="Component" />.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            DisposeComponent();
            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Component" /> class.
        /// </summary>
        ~Component()
        {
            Dispose();
        }

        /// <summary>
        ///     Occurs when a property has changed or the <see cref="Component" /> needs to be refreshed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        ///     Occurs when the location of this <see cref="Component" /> has changed.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        ///     Occurs when this <see cref="Component" /> has been disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///     Refreshes the <see cref="Bytemap" /> and renders it if necessary.
        /// </summary>
        /// <param name="forceRefresh">
        ///     Forces the <see cref="Bytemap" /> being rerendered even if it hasn't changed when True.
        /// </param>
        public virtual void Refresh(bool forceRefresh)
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Resource was disposed.");

            if (!forceRefresh && !_hasChanged)
                return;

            IsRendering = true;

            //Debug.WriteLine("[LogiFrame] Rendering " + this + " : parent=" + ParentComponent);

            _bytemap = Size.Width == 0 || Size.Height == 0 ? new Bytemap(1, 1) : (Render() ?? new Bytemap(1, 1));
            _bytemap.Transparent = IsTransparent;
            _bytemap.TopEffect = IsTopEffectEnabled;

            IsRendering = false;
        }

        /// <summary>
        ///     Refreshes the Bytemap and renders it if nececcary.
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        ///     Swaps property with given value.
        /// </summary>
        /// <param name="field">The value of the field.</param>
        /// <param name="value">The value to swap it with.</param>
        /// <returns>Whether the field's value has changed.</returns>
        protected bool SwapProperty<T>(ref T field, T value)
        {
            return SwapProperty(ref field, value, false, true);
        }

        /// <summary>
        ///     Swaps property with given value.
        /// </summary>
        /// <param name="field">The value of the field.</param>
        /// <param name="value">The value to swap it with.</param>
        /// <param name="allowNull">Whether null values are allowed.</param>
        /// <returns>Whether the field's value has changed.</returns>
        protected bool SwapProperty<T>(ref T field, T value, bool allowNull)
        {
            return SwapProperty(ref field, value, allowNull, true);
        }

        /// <summary>
        ///     Swaps property with given <paramref name="value" />.
        /// </summary>
        /// <param name="field">The value of the field.</param>
        /// <param name="value">The value to swap it with.</param>
        /// <param name="allowNull">Whether null values are allowed.</param>
        /// <param name="reportChange">
        ///     Whether <see cref="OnChanged" /> should be called if the property has been swapped with the
        ///     value.
        /// </param>
        /// <returns>Whether the field's value has changed.</returns>
        protected bool SwapProperty<T>(ref T field, T value, bool allowNull, bool reportChange)
        {
            if (value == null && !allowNull)
                throw new NullReferenceException("value");

            if (field != null && field.Equals(value))
                return false;

            field = value;

            if (reportChange)
                OnChanged(EventArgs.Empty);

            return true;
        }

        /// <summary>
        ///     Finds the parent <see cref="Component" /> of the given type.
        /// </summary>
        /// <typeparam name="T">Type to find.</typeparam>
        /// <returns>The partent <see cref="Component" /> of the given type. Returns null if not found.</returns>
        public T GetParentComponentOfType<T>() where T : Component
        {
            Component c = this;
            while ((c = c.ParentComponent) != null)
                if (c is T)
                    return c as T;
            return null;
        }

        /// <summary>
        ///     Raises the <see cref="LocationChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public virtual void OnLocationChanged(EventArgs e)
        {
            if (LocationChanged != null)
                LocationChanged(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Changed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public virtual void OnChanged(EventArgs e)
        {
            _hasChanged = true;

            if (!IsRendering && Changed != null)
                Changed(this, e);
        }

        /// <summary>
        ///     Stub for child components. This overridable method can be used to dispose resources.
        /// </summary>
        protected virtual void DisposeComponent()
        {
            //Stub
        }

        /// <summary>
        ///     Renders all graphics of this <see cref="Component" />.
        /// </summary>
        /// <returns>The rendered <see cref="Bytemap" />.</returns>
        protected abstract Bytemap Render();

        private void size_Changed(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }

        private void location_Changed(object sender, EventArgs e)
        {
            if (!IsDisposed && LocationChanged != null)
                LocationChanged(sender, e);
        }
    }
}