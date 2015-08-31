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
using System.Threading;
using System.Threading.Tasks;
using LogiFrame.Drawing;
using LogiFrame.Internal;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a GamePanel app.
    /// </summary>
    public class LCDApp : ContainerLCDControl
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly LgLcd.ConnectContext _connection;
        private readonly int _device;
        // Must keep the open context to prevent the button change delegate from being GCed.
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly LgLcd.OpenContext _openContext;
        private int _oldButtons;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LCDApp" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="canAutoStart">if set to <c>true</c> the app can automatic start.</param>
        /// <param name="isPersistent">if set to <c>true</c> the app is persistent.</param>
        /// <param name="allowConfiguration">
        ///     if set to <c>true</c> the user is allowed to access the configuration menu from the
        ///     GamePanel software.
        /// </param>
        /// <exception cref="ConnectionException"></exception>
        public LCDApp(string name, bool canAutoStart, bool isPersistent, bool allowConfiguration)
        {
            UpdatePriority = UpdatePriority.Normal;

            _connection.AppFriendlyName = name;
            _connection.IsAutostartable = canAutoStart;
            _connection.IsPersistent = isPersistent;

            if (allowConfiguration)
                _connection.OnConfigure.ConfigCallback = (connection, pContext) =>
                {
                    OnConfigure();
                    return 1;
                };

            UnmanagedLibrariesLoader.Load();
            LgLcd.Init();

            var connectionResponse = LgLcd.Connect(ref _connection);

            if (connectionResponse != 0)
                throw new ConnectionException(connectionResponse);

            _openContext = new LgLcd.OpenContext
            {
                Connection = _connection.Connection,
                Index = 0,
                OnSoftButtonsChanged =
                {
                    Callback = (device, buttons, context) =>
                    {
                        for (var button = 0; button < 4; button++)
                        {
                            var buttonIdentifier = 1 << button;
                            if ((buttons & buttonIdentifier) > (_oldButtons & buttonIdentifier))
                                HandleButtonDown(button);
                            else if ((buttons & buttonIdentifier) < (_oldButtons & buttonIdentifier))
                                HandleButtonUp(button);
                        }
                        _oldButtons = buttons;
                        return 1;
                    }
                }
            };

            LgLcd.Open(ref _openContext);
            _device = _openContext.Device;

            SetBounds(0, 0, DefaultSize.Width, DefaultSize.Height);
            InitLayout();
        }

        /// <summary>
        ///     Gets the size of an LCD screen.
        /// </summary>
        public static Size DefaultSize { get; } = new Size((int) LgLcd.BitmapWidth, (int) LgLcd.BitmapHeight);

        /// <summary>
        ///     Gets or sets the update priority of future updates.
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }

        /// <summary>
        ///     Occurs when the configuration option is selected for app in the GamePanel software.
        /// </summary>
        public event EventHandler Configure;

        /// <summary>
        ///     Occurs when a new frame was rendered.
        /// </summary>
        public event EventHandler<RenderedEventArgs> Rendered;

        /// <summary>
        ///     Pushes the app to the foreground.
        /// </summary>
        public void PushToForeground()
        {
            ThrowIfDisposed();

            LgLcd.SetAsLCDForegroundApp(_device, 1);
        }

        /// <summary>
        ///     Pushes the app to the background.
        /// </summary>
        public void PushToBackground()
        {
            ThrowIfDisposed();

            LgLcd.SetAsLCDForegroundApp(_device, 0);
        }

        #region Overrides of LCDControl

        /// <summary>
        ///     Invalidates the entire surface of the control and causes the control to be redrawn.
        /// </summary>
        public override void Invalidate()
        {
            base.Invalidate();
            PerformLayout();
        }

        #endregion

        #region Overrides of ContainerLCDControl

        /// <summary>
        ///     Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            base.OnPaint(e);
            Push(e.Bitmap);
        }

        #endregion

        #region Overrides of LCDControl

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:LogiFrame.LCDControl" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            LgLcd.Close(_device);
            base.Dispose(disposing);
            _cancellationTokenSource.Cancel();
        }

        #endregion

        /// <summary>
        ///     Waits for the app to close asynchronously.
        /// </summary>
        /// <returns>A task which waits for the app to close.</returns>
        public async Task WaitForCloseAsync()
        {
            ThrowIfDisposed();

            while (!IsDisposed)
            {
                try
                {
                    await Task.Delay(60000, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        /// <summary>
        ///     Waits for the app to close.
        /// </summary>
        public void WaitForClose()
        {
            WaitForCloseAsync().Wait();
        }

        /// <summary>
        ///     Raises the <see cref="E:Configure" /> event.
        /// </summary>
        protected virtual void OnConfigure()
        {
            Configure?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Pushes the specified bitmap to the LCD.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        private void Push(MonochromeBitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            var render = new MonochromeBitmap(bitmap, (int) LgLcd.BitmapWidth, (int) LgLcd.BitmapHeight);
            var lgBitmap = new LgLcd.Bitmap160X43X1
            {
                Header = {Format = LgLcd.BitmapFormat160X43X1},
                Pixels = render.Pixels
            };

            LgLcd.UpdateBitmap(_device, ref lgBitmap, (uint) UpdatePriority);
            OnRendered(new RenderedEventArgs(render));
        }

        /// <summary>
        ///     Raises the <see cref="E:Rendered" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.RenderedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnRendered(RenderedEventArgs e)
        {
            Rendered?.Invoke(this, e);
        }

        /// <summary>
        ///     Determines whether the specified button is pressed.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>true if pressed; otherwise false.</returns>
        public virtual bool IsButtonDown(int button)
        {
            ThrowIfDisposed();

            return (_oldButtons & (1 << button)) != 0;
        }
    }
}