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
using LogiFrame.Tools;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Provides functionality for a drawable component.
    /// </summary>
    [TypeConverter(typeof (SimpleExpandableObjectConverter))]
    public abstract class Component : Disposable
    {
        private Bytemap _bytemap;
        private bool _hasChanged;

        private bool _isTopEffectEnabled;
        private bool _isTransparent;
        private bool _isVisible = true;
        private Location _location = new Location();
        private Location _renderOffset = new Location();
        private Size _size = new Size();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Component" /> class.
        /// </summary>
        protected Component()
        {
            _location.Changed += location_Changed;
            _size.Changed += size_Changed;
        }

        /// <summary>
        ///     Gets or sets the location.
        /// </summary>
        public virtual Location Location
        {
            get { return _location; }
            set
            {
                AssertNotDisposed();

                _location.Changed -= location_Changed;

                if (SwapProperty(ref _location, value))
                    OnLocationChanged(EventArgs.Empty);

                _location.Changed += location_Changed;
            }
        }

        /// <summary>
        ///     Gets the render location.
        /// </summary>
        public Location RenderLocation
        {
            get { return _location + _renderOffset; }
        }

        /// <summary>
        ///     Gets or sets the render offset.
        /// </summary>
        protected Location RenderOffset
        {
            get { return _renderOffset; }
            set
            {
                AssertNotDisposed();

                _renderOffset.Changed -= location_Changed;

                if (SwapProperty(ref _renderOffset, value))
                    OnLocationChanged(EventArgs.Empty);

                _renderOffset.Changed += location_Changed;
            }
        }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        public virtual Size Size
        {
            get { return _size; }
            set
            {
                AssertNotDisposed();

                _size.Changed -= size_Changed;
                SwapProperty(ref _size, value);
                _size.Changed += size_Changed;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is top effect enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is top effect enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsTopEffectEnabled
        {
            get { return _isTopEffectEnabled; }
            set { SwapProperty(ref _isTopEffectEnabled, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is transparent.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is transparent; otherwise, <c>false</c>.
        /// </value>
        public bool IsTransparent
        {
            get { return _isTransparent; }
            set { SwapProperty(ref _isTransparent, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SwapProperty(ref _isVisible, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is rendering.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is rendering; otherwise, <c>false</c>.
        /// </value>
        public bool IsRendering { get; protected set; }

        /// <summary>
        ///     Gets or sets the object that contains data about this <see cref="Component" />.
        /// </summary>
        public object Tag { get; set; }

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

        #region Overrides of Disposable

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            //...
        }

        #endregion

        /// <summary>
        ///     Occurs when a property of this instance has changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        ///     Occurs when the location of this instance has changed.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        ///     Refreshes this instance.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c>, this instance will be refreshed even if it hasn't changed.</param>
        public virtual void Refresh(bool forceRefresh)
        {
            AssertNotDisposed();

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
        ///     Refreshes this instance.
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
        ///     Renders this instance to a <see cref="Bytemap"/>.
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