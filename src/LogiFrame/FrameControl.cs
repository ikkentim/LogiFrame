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
using System.Drawing;
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FrameControl : IDisposable
    {
        private int _height;
        private bool _isInvalidated;
        private bool _isLayoutInit;
        private bool _isPerformingLayout;
        private int _layoutSuspendCount;
        private IMergeMethod _mergeMethod = MergeMethods.Override;
        private bool _visible = true;
        private int _width;
        private int _x;
        private int _y;
        public FrameControl Parent { get; private set; }
        public MonochromeBitmap Bitmap { get; private set; }

        public virtual IMergeMethod MergeMethod
        {
            get { return _mergeMethod ?? MergeMethods.Override; }
            set
            {
                _mergeMethod = value;
                Parent?.Invalidate();
            }
        }

        public virtual Point Location
        {
            get { return new Point(_x, _y); }
            set
            {
                _x = value.X;
                _y = value.Y;
                Parent?.Invalidate();
            }
        }

        public virtual Size Size
        {
            get { return new Size(Width, Height); }
            set { SetBounds(_x, _y, value.Width, value.Height); }
        }

        public virtual int Width
        {
            get { return _width; }
            set { SetBounds(_x, _y, value, _height); }
        }

        public virtual int Height
        {
            get { return _height; }
            set { SetBounds(_x, _y, _width, value); }
        }

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

        public event EventHandler VisibleChanged;
        public event EventHandler<FramePaintEventArgs> Paint;
        public event EventHandler<ButtonEventArgs> ButtonDown;
        public event EventHandler<ButtonEventArgs> ButtonUp;

        protected void SetBounds(int x, int y, int width, int height)
        {
            SetBounds(x, y, width, height, false);
        }

        protected virtual void SetBounds(int x, int y, int width, int height, bool preventInvalidation)
        {
            ThrowIfDisposed();

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (_x == x && _y == y && _width == width && _height == height) return;

            _x = x;
            _y = y;
            _width = width;
            _height = height;

            Bitmap = new MonochromeBitmap(Width, Height);

            if (!preventInvalidation)
                Invalidate();
        }

        public virtual void AssignParent(FrameControl value)
        {
            ThrowIfDisposed();

            Parent = value;
            InitLayout();
        }

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

        public virtual void PerformLayout()
        {
            ThrowIfDisposed();

            if (_isInvalidated && _isLayoutInit && _layoutSuspendCount == 0)
            {
                _isPerformingLayout = true;
                Bitmap.Reset();

                if (Visible)
                    OnPaint(new FramePaintEventArgs(Bitmap));

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

        protected virtual void OnPaint(FramePaintEventArgs e)
        {
            Paint?.Invoke(this, e);
        }

        protected virtual void OnButtonDown(ButtonEventArgs e)
        {
            ButtonDown?.Invoke(this, e);
        }

        protected virtual void OnButtonUp(ButtonEventArgs e)
        {
            ButtonUp?.Invoke(this, e);
        }

        protected virtual void OnVisibleChanged()
        {
            VisibleChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Show()
        {
            ThrowIfDisposed();

            Visible = true;
        }

        public void Hide()
        {
            ThrowIfDisposed();

            Visible = false;
        }

        public bool HandleButtonDown(int button)
        {
            ThrowIfDisposed();

            var args = new ButtonEventArgs(button);
            if (!args.PreventPropagation)
                OnButtonDown(args);
            return args.PreventPropagation;
        }

        public bool HandleButtonUp(int button)
        {
            ThrowIfDisposed();

            var args = new ButtonEventArgs(button);
            if (!args.PreventPropagation)
                OnButtonUp(args);
            return args.PreventPropagation;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Releases all resources used by the <see cref="T:LogiFrame.FrameControl"/>.
        /// </summary>
        public void Dispose()
        {
            ThrowIfDisposed();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~FrameControl()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:LogiFrame.FrameControl"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            lock (this)
            {
                (Parent as ContainerFrameControl)?.Controls?.Remove(this);
                Disposing = true;
                Disposed?.Invoke(this, EventArgs.Empty);
                Disposing = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the control has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Gets a value indicating whether the control has been disposed of.
        /// </summary>
        /// 
        /// <returns>
        /// true if the control has been disposed of; otherwise, false.
        /// </returns>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the base <see cref="T:LogiFrame.FrameControl"/> class is in the process of disposing.
        /// </summary>
        /// 
        /// <returns>
        /// true if the base <see cref="T:LogiFrame.FrameControl"/> class is in the process of disposing; otherwise, false.
        /// </returns>
        public bool Disposing { get; private set; }

        /// <summary>
        /// Occurs when the component is disposed by a call to the <see cref="M:LogiFrame.FrameControl.Dispose"/> method.
        /// </summary>
        public event EventHandler Disposed;
    }
}