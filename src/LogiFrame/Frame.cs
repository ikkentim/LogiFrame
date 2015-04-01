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
using System.Threading;
using System.Threading.Tasks;
using LogiFrame.Components;
using LogiFrame.Tools;

namespace LogiFrame
{
    /// <summary>
    ///     Represents the framework.
    /// </summary>
    public class Frame : Container
    {
        private LgLcd.Bitmap160X43X1 _bitmap;
        private int _buttonState;
        private LgLcd.ConnectContext _connection;
        private LgLcd.OpenContext _openContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Frame" /> class.
        /// </summary>
        /// <param name="name">The name of the application.</param>
        /// <param name="isCanAutoStart">Whether the application can be started by LCDMon.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        /// <param name="isAllowConfiguration">Whether the application is configurable trough LCDmon.</param>
        public Frame(string name, bool isCanAutoStart, bool isPersistent, bool isAllowConfiguration)
            : this(name, isCanAutoStart, isPersistent, isAllowConfiguration, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Frame" /> class.
        /// </summary>
        /// <param name="name">The name of the application.</param>
        /// <param name="isCanAutoStart">Whether the application can be started by LCDMon.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        /// <param name="isAllowConfiguration">Whether the application is configurable trough LCDmon.</param>
        /// <param name="isSimulationEnabled">Whether the frame should start in simulation mode.</param>
        public Frame(string name, bool isCanAutoStart, bool isPersistent, bool isAllowConfiguration,
            bool isSimulationEnabled)
        {
            _connection.AppFriendlyName = name;
            _connection.IsAutostartable = isCanAutoStart;
            _connection.IsPersistent = isPersistent;

            UpdatePriority = UpdatePriority.Normal;
            base.Size = LCDSize;

            /* Handle configuration event.
             */
            if (isAllowConfiguration)
                _connection.OnConfigure.ConfigCallback =
                    (connection, pContext) =>
                    {
                        OnConfigure(EventArgs.Empty);
                        return 1;
                    };

            /* If simulation is enabled, spawn a thread in which the simulation form is displayed.
             */
            if (isSimulationEnabled)
                new Thread(() => Simulation.Start(this)) {Name = "LogiFrame simulation thread"}.Start();

            Changed += (sender, args) =>
            {
                if (IsDisposed) return;

                var component = sender as Component;
                if (component != null) UpdateScreen(component.Snapshot);
            };

            /* Initialize the library.
             */
            UnmanagedLibrariesLoader.Load();
            LgLcd.Init();

            /* Create a connection with the device.
             */
            int connectionResponse = LgLcd.Connect(ref _connection);

            if (connectionResponse != 0 && !isSimulationEnabled)
                throw new ConnectionException(connectionResponse);

            /* Start the connection with the device.
             */
            _openContext.Connection = _connection.Connection;
            _openContext.OnSoftButtonsChanged.Callback = OnSoftButtonsChangedCallback;
            _openContext.Index = 0;

            LgLcd.Open(ref _openContext);

            /* Prepare the bitmap header.
             */
            _bitmap.hdr.Format = LgLcd.BitmapFormat160X43X1;

            /* Send an empty frame to the screen.
             */
            UpdateScreen(Snapshot.Empty);
        }

        /// <summary>
        ///     Gets the size of the LCD.
        /// </summary>
        public static Size LCDSize
        {
            get { return new Size((int) LgLcd.BitmapWidth, (int) LgLcd.BitmapHeight); }
        }

        /// <summary>
        ///     Gets or sets the update priority.
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }

        #region Overrides of Component

        /// <summary>
        ///     Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Size cannot be set</exception>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new InvalidOperationException("Size cannot be set"); }
        }

        #endregion

        private int OnSoftButtonsChangedCallback(int device, int buttons, IntPtr context)
        {
            for (int i = 0, b = 1; i < 4; i++, b *= 2)
                if ((_buttonState & b) == 0 && (buttons & b) == b) OnButtonDown(new ButtonEventArgs(i));
                else if ((_buttonState & b) == b && (buttons & b) == 0) OnButtonUp(new ButtonEventArgs(i));

            _buttonState = buttons;
            return 1;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Frame" /> class.
        /// </summary>
        ~Frame()
        {
            Dispose();
        }

        /// <summary>
        ///     Occurs when a button has been pressed.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonDown;

        /// <summary>
        ///     Occurs when a button has been released.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonUp;

        /// <summary>
        ///     Occurs when a frame is being pushed to the device.
        /// </summary>
        public event EventHandler<PushingEventArgs> Pushing;

        /// <summary>
        ///     Occurs when frame has been closed.
        /// </summary>
        public event EventHandler FrameClosed;

        /// <summary>
        ///     Occurs when the 'configure' option is selected in the Logitech software.
        /// </summary>
        public event EventHandler Configure;

        /// <summary>
        ///     Waits for close.
        /// </summary>
        public void WaitForClose()
        {
            while (!IsDisposed)
                Thread.Sleep(1000);
        }

        /// <summary>
        ///     Waits for close.
        /// </summary>
        public async void WaitForCloseAsync()
        {
            while (!IsDisposed)
                await Task.Delay(1000);
        }

        /// <summary>
        ///     Raises the <see cref="ButtonDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonEventArgs" /> instance containing the event data.</param>
        protected virtual void OnButtonDown(ButtonEventArgs e)
        {
            if (ButtonDown != null)
                ButtonDown(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ButtonUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonEventArgs" /> instance containing the event data.</param>
        protected virtual void OnButtonUp(ButtonEventArgs e)
        {
            if (ButtonUp != null)
                ButtonUp(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Pushing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PushingEventArgs" /> instance containing the event data.</param>
        protected virtual void OnPushing(PushingEventArgs e)
        {
            if (Pushing != null)
                Pushing(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="FrameClosed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnFrameClosed(EventArgs e)
        {
            if (FrameClosed != null)
                FrameClosed(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Configure" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnConfigure(EventArgs e)
        {
            if (Configure != null)
                Configure(this, e);
        }

        /// <summary>
        ///     Performs a button press for the specified <paramref name="button" />.
        /// </summary>
        /// <param name="button">The button.</param>
        public void PerformButtonDown(int button)
        {
            int buttonMask = 1 << button;

            if ((_buttonState & buttonMask) == buttonMask)
                return;

            _buttonState = _buttonState | buttonMask;

            OnButtonDown(new ButtonEventArgs(button));
        }

        /// <summary>
        ///     Performs a button release for the specified <paramref name="button" />.
        /// </summary>
        /// <param name="button">The button.</param>
        public void PerformButtonUp(int button)
        {
            int buttonMask = 1 << button;

            if ((_buttonState & buttonMask) == 0)
                return;

            _buttonState = _buttonState ^ buttonMask;

            OnButtonUp(new ButtonEventArgs(button));
        }

        private void UpdateScreen(Snapshot snapshot)
        {
            var e = new PushingEventArgs(snapshot);
            OnPushing(e);

            if (e.PreventPush) return;

            _bitmap.pixels = snapshot.IsEmpty
                ? new byte[LgLcd.BitmapWidth*LgLcd.BitmapHeight]
                : snapshot.Data;

            LgLcd.UpdateBitmap(_openContext.Device, ref _bitmap, (uint) UpdatePriority);
        }

        #region Overrides of Container

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnFrameClosed(EventArgs.Empty);
            }

            new Thread(() =>
            {
                LgLcd.Close(_openContext.Device);
                LgLcd.Disconnect(_connection.Connection);
                LgLcd.DeInit();
            }) {Name = "LogiFrame disposal thread"}.Start();

            base.Dispose(disposing);
        }

        #endregion
    }
}