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
using System.Drawing;
using LogiFrame.Drawing;

namespace LogiFrame
{
    /// <summary>
    ///     Defines the base class for LCD controls, which are components with visual representation.
    /// </summary>
    public class LCDControl : IDisposable
    {
        private int _height;
        private bool _isInvalidated;
        private bool _isLayoutInit;
        private bool _isPerformingLayout;
        private int _layoutSuspendCount;
        private int _left;
        private IMergeMethod _mergeMethod = MergeMethods.Override;
        private int _top;
        private bool _visible = true;
        private int _width;

        /// <summary>
        ///     Gets the parent container of the control.
        /// </summary>
        public LCDControl Parent { get; private set; }

        /// <summary>
        ///     Gets the last created bitmap.
        /// </summary>
        public MonochromeBitmap Bitmap { get; private set; }

        /// <summary>
        ///     Gets or sets the merge method used on the control.
        /// </summary>
        public virtual IMergeMethod MergeMethod
        {
            get { return _mergeMethod ?? MergeMethods.Override; }
            set
            {
                _mergeMethod = value;
                Parent?.Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the location of the control.
        /// </summary>
        public virtual Point Location
        {
            get { return new Point(_left, _top); }
            set
            {
                _left = value.X;
                _top = value.Y;
                Parent?.Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the distance, in pixels, between the left edge of the control and the left edge of its container.
        /// </summary>
        public int Left
        {
            get { return _left; }
            set
            {
                _left = value;
                Parent?.Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the distance, in pixels, between the top edge of the control and the top edge of its container.
        /// </summary>
        public int Top
        {
            get { return _top; }
            set
            {
                _top = value;
                Parent?.Invalidate();
            }
        }

        /// <summary>
        ///     Gets or sets the size of the control.
        /// </summary>
        public virtual Size Size
        {
            get { return new Size(Width, Height); }
            set { SetBounds(_left, _top, value.Width, value.Height); }
        }

        /// <summary>
        ///     Gets or sets the width of the control.
        /// </summary>
        public virtual int Width
        {
            get { return _width; }
            set { SetBounds(_left, _top, value, _height); }
        }

        /// <summary>
        ///     Gets or sets the height of the control.
        /// </summary>
        public virtual int Height
        {
            get { return _height; }
            set { SetBounds(_left, _top, _width, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="LCDControl" /> is visible.
        /// </summary>
        public virtual bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible == value) return;
                _visible = value;
                OnVisibleChanged();
                Invalidate();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the control has been disposed of.
        /// </summary>
        /// <returns>
        ///     true if the control has been disposed of; otherwise, false.
        /// </returns>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the base <see cref="T:LogiFrame.LCDControl" /> class is in the process of
        ///     disposing.
        /// </summary>
        /// <returns>
        ///     true if the base <see cref="T:LogiFrame.LCDControl" /> class is in the process of disposing; otherwise, false.
        /// </returns>
        public bool Disposing { get; private set; }

        #region Implementation of IDisposable

        /// <summary>
        ///     Releases all resources used by the <see cref="T:LogiFrame.LCDControl" />.
        /// </summary>
        public void Dispose()
        {
            ThrowIfDisposed();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Occurs when the <see cref="P:LogiFrame.LCDControl.Visible" /> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        ///     Occurs when the control is redrawn.
        /// </summary>
        public event EventHandler<LCDPaintEventArgs> Paint;

        /// <summary>
        ///     Occurs when a button is pressed.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonDown;

        /// <summary>
        ///     Occurs when a button is released
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonUp;

        /// <summary>
        ///     Occurs when the component is disposed by a call to the <see cref="M:LogiFrame.LCDControl.Dispose" /> method.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///     Sets the bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new <see cref="P:LogiFrame.LCDControl.Left" /> property value of the control. </param>
        /// <param name="y">The new <see cref="P:LogiFrame.LCDControl.Top" /> property value of the control. </param>
        /// <param name="width">The new <see cref="P:LogiFrame.LCDControl.Width" /> property value of the control. </param>
        /// <param name="height">The new <see cref="P:LogiFrame.LCDControl.Height" /> property value of the control. </param>
        protected void SetBounds(int x, int y, int width, int height)
        {
            SetBounds(x, y, width, height, false);
        }

        /// <summary>
        ///     Sets the bounds of the control to the specified location and size.
        /// </summary>
        /// <param name="x">The new <see cref="P:LogiFrame.LCDControl.Left" /> property value of the control.</param>
        /// <param name="y">The new <see cref="P:LogiFrame.LCDControl.Top" /> property value of the control.</param>
        /// <param name="width">The new <see cref="P:LogiFrame.LCDControl.Width" /> property value of the control.</param>
        /// <param name="height">The new <see cref="P:LogiFrame.LCDControl.Height" /> property value of the control.</param>
        /// <param name="preventInvalidation">if set to <c>true</c> invalidation is prevented.</param>
        protected virtual void SetBounds(int x, int y, int width, int height, bool preventInvalidation)
        {
            ThrowIfDisposed();

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (_left == x && _top == y && _width == width && _height == height) return;

            _left = x;
            _top = y;
            _width = width;
            _height = height;

            Bitmap = new MonochromeBitmap(Width, Height);

            if (!preventInvalidation)
                Invalidate();
        }

        /// <summary>
        ///     Assigns the <see cref="P:LogiFrame.LCDControl.Parent" /> of the control.
        /// </summary>
        /// <param name="value">The parent control.</param>
        public virtual void AssignParent(LCDControl value)
        {
            ThrowIfDisposed();

            Parent = value;
            InitLayout();
        }

        /// <summary>
        ///     Invalidates the entire surface of the control and causes the control to be redrawn.
        /// </summary>
        public virtual void Invalidate()
        {
            ThrowIfDisposed();

            _isInvalidated = true;
            if (!_isPerformingLayout && _layoutSuspendCount == 0)
                Parent?.Invalidate();
        }

        /// <summary>
        ///     Suspends the usual layout logic.
        /// </summary>
        public virtual void SuspendLayout()
        {
            ThrowIfDisposed();

            _layoutSuspendCount++;
        }

        /// <summary>
        ///     Resumes the usual layout logic.
        /// </summary>
        /// <param name="performLayout">true to execute pending layout requests; otherwise, false.</param>
        public virtual void ResumeLayout(bool performLayout)
        {
            ThrowIfDisposed();

            if (_layoutSuspendCount > 0)
            {
                _layoutSuspendCount--;

                if (_isInvalidated && performLayout)
                    Invalidate();
            }
        }

        /// <summary>
        ///     Resumes the usual layout logic.
        /// </summary>
        public virtual void ResumeLayout()
        {
            ThrowIfDisposed();

            ResumeLayout(true);
        }

        /// <summary>
        ///     Performs the control's layout logic.
        /// </summary>
        public virtual void PerformLayout()
        {
            ThrowIfDisposed();

            if (_isInvalidated && _isLayoutInit && _layoutSuspendCount == 0)
            {
                _isPerformingLayout = true;
                Bitmap.Reset();

                if (Visible)
                    OnPaint(new LCDPaintEventArgs(Bitmap));

                _isInvalidated = false;
                _isPerformingLayout = false;
            }
        }

        /// <summary>
        ///     Called after the control has been added to another container.
        /// </summary>
        protected virtual void InitLayout()
        {
            if (_isLayoutInit)
            {
                Invalidate();
                return;
            }

            _isLayoutInit = true;

            if (_width == 0 || _height == 0)
                SetBounds(0, 0, 1, 1);
            else
                Bitmap = new MonochromeBitmap(Width, Height);

            Invalidate();
        }

        /// <summary>
        ///     Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected virtual void OnPaint(LCDPaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:ButtonDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.ButtonEventArgs" /> instance containing the event data.</param>
        protected virtual void OnButtonDown(ButtonEventArgs e)
        {
            ButtonDown?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:ButtonUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.ButtonEventArgs" /> instance containing the event data.</param>
        protected virtual void OnButtonUp(ButtonEventArgs e)
        {
            ButtonUp?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:VisibleChanged" /> event.
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            VisibleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Displays the control to the user.
        /// </summary>
        public void Show()
        {
            ThrowIfDisposed();

            Visible = true;
        }

        /// <summary>
        ///     Hides the control from the user.
        /// </summary>
        public void Hide()
        {
            ThrowIfDisposed();

            Visible = false;
        }

        /// <summary>
        ///     Handles a button press.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>true if the call was handled; otherwise false.</returns>
        public bool HandleButtonDown(int button)
        {
            ThrowIfDisposed();

            var args = new ButtonEventArgs(button);
            if (!args.PreventPropagation)
                OnButtonDown(args);
            return args.PreventPropagation;
        }

        /// <summary>
        ///     Handles a button release.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>true if the call was handled; otherwise false.</returns>
        public bool HandleButtonUp(int button)
        {
            ThrowIfDisposed();

            var args = new ButtonEventArgs(button);
            if (!args.PreventPropagation)
                OnButtonUp(args);
            return args.PreventPropagation;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="LCDControl" /> class.
        /// </summary>
        ~LCDControl()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:LogiFrame.LCDControl" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            lock (this)
            {
                (Parent as ContainerLCDControl)?.Controls?.Remove(this);
                Disposing = true;
                Disposed?.Invoke(this, EventArgs.Empty);
                Disposing = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        ///     Throws an <see cref="ObjectDisposedException" /> if the control has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}