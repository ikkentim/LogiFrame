// Frame.cs
// 
// LogiFrame rendering library.
// Copyright (C) 2013 Tim Potze
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
using System.Threading;
using LogiFrame.Components;

namespace LogiFrame
{
    /// <summary>
    ///     Represents the framework.
    /// </summary>
    public sealed class Frame : Container
    {
        #region Fields

        private LgLcd.lgLcdBitmap160x43x1 _bitmap;

        private int _buttonState;
        private LgLcd.lgLcdConnectContext _connection;
        private LgLcd.lgLcdOpenContext _openContext;

        #endregion

        #region Constructor/Deconstructor

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        public Frame(string applicationName)
            : this(applicationName, false, false, false, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        public Frame(string applicationName, bool isAutostartable)
            : this(applicationName, isAutostartable, false, false, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent)
            : this(applicationName, isAutostartable, isPersistent, false, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        /// <param name="allowConfiguration">Whether the application is configurable via LCDmon.</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent, bool allowConfiguration)
            : this(applicationName, isAutostartable, isPersistent, allowConfiguration, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        /// <param name="allowConfiguration">Whether the application is configurable via LCDmon.</param>
        /// <param name="simulate">Whether LogiFrame should start in simulation mode.</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent, bool allowConfiguration,
            bool simulate)
        {
            //Check for LgLcd.dll file
            if (!System.IO.File.Exists("LgLcd.dll"))
                throw new DllNotFoundException("Could not find LgLcd.dll.");

            //Initialize connection and store properties
            _connection.appFriendlyName = ApplicationName = applicationName;
            _connection.isAutostartable = IsAutostartable = isAutostartable;
            _connection.isPersistent = IsPersistent = isPersistent;
            Simulate = simulate;

            if (AllowConfiguration = allowConfiguration)
                _connection.onConfigure.configCallback = lgLcd_onConfigureCB;

            _connection.connection = LgLcd.LGLCD_INVALID_CONNECTION;

            //Set default updatepriority
            UpdatePriority = LogiFrame.UpdatePriority.Normal;

            //Start simulation thread
            if (simulate)
                new Thread(() => { Simulation.Start(this); }).Start();

            //Initialize main container
            Size = new Size((int) LgLcd.LGLCD_BMP_WIDTH, (int) LgLcd.LGLCD_BMP_HEIGHT);
            Changed += mainContainer_ComponentChanged;

            //Store connection
            LgLcd.lgLcdInit();
            int connectionResponse = LgLcd.lgLcdConnect(ref _connection);

            //Check if a connection is set or throw an Exception
            if (connectionResponse != Win32Error.ERROR_SUCCESS && !simulate)
                throw new ConnectionException(Win32Error.ToString(connectionResponse));

            //Open connection
            _openContext.connection = _connection.connection;
            _openContext.onSoftbuttonsChanged.softbuttonsChangedCallback = lgLcd_onSoftButtonsCB;
            _openContext.index = 0;

            LgLcd.lgLcdOpen(ref _openContext);

            //Store bitmap format
            _bitmap.hdr = new LgLcd.lgLcdBitmapHeader();
            _bitmap.hdr.Format = LgLcd.LGLCD_BMP_FORMAT_160x43x1;

            //Send empty bytemap
            UpdateScreen(null);
        }

        /// <summary>
        ///     Releases all resources used by LogiFrame.Frame
        /// </summary>
        ~Frame()
        {
            Dispose();
        }

        #endregion

        #region Events

        /// <summary>
        ///     Represents the method that handles a LogiFrame.Frame.ButtonDown.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ButtonEventArgs that contains the event data.</param>
        public delegate void ButtonDownEventHandler(object sender, ButtonEventArgs e);

        /// <summary>
        ///     Represents the method that handles a LogiFrame.Frame.ButtonUp.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ButtonEventArgs that contains the event data.</param>
        public delegate void ButtonUpEventHandler(object sender, ButtonEventArgs e);

        /// <summary>
        ///     Represents the method that handles a LogiFrame.Frame.FramePush.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.PushingEventArgs that contains the event data.</param>
        public delegate void PushingEventHandler(object sender, PushingEventArgs e);

        /// <summary>
        ///     Occurs when a button is being pressed.
        /// </summary>
        public event ButtonDownEventHandler ButtonDown;

        /// <summary>
        ///     Occurs when a button is being released.
        /// </summary>
        public event ButtonUpEventHandler ButtonUp;

        /// <summary>
        ///     Occurs before a frame is being pushed to the display.
        /// </summary>
        public event PushingEventHandler Pushing;

        /// <summary>
        ///     Occurs after the frame has been closed or disposed
        /// </summary>
        public event EventHandler FrameClosed;

        /// <summary>
        ///     Occurs when the 'configure' button has been pressed in LCDmon.
        /// </summary>
        public event EventHandler Configure;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a string that contains the 'friendly name' of the application.
        ///     This name is presented to the user whenever a list of applications is shown.
        /// </summary>
        public string ApplicationName { get; private set; }

        /// <summary>
        ///     Gets whether application can be started by LCDMon or not.
        /// </summary>
        public bool IsAutostartable { get; private set; }

        /// <summary>
        ///     Gets whether connection is temporary or whether it is
        ///     a regular connection that should be added to the list.
        /// </summary>
        public bool IsPersistent { get; private set; }

        /// <summary>
        ///     Gets whether the 'configure' option is being shown in LCDmon.
        /// </summary>
        public bool AllowConfiguration { get; private set; }

        /// <summary>
        ///     Gets whether LogiFrame.Frame is simulating the LCD display on-screen.
        /// </summary>
        public bool Simulate { get; private set; }

        /// <summary>
        ///     Gets or sets the priority for the forthcoming LCD updates.
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Releases all resources used by LogiFrame.Frame
        /// </summary>
        public new void Dispose()
        {
            if (!Disposed)
            {
                base.Dispose();

                if (FrameClosed != null)
                    FrameClosed(this, EventArgs.Empty);

                //Cannot de-initialize LgLcd from LgLcd-thread.
                //As a precausion disposing resources from another thread
                new Thread(() =>
                {
                    LgLcd.lgLcdClose(_openContext.device);
                    LgLcd.lgLcdDisconnect(_connection.connection);
                    LgLcd.lgLcdDeInit();
                }).Start();
            }
        }

        /// <summary>
        ///     Waits untill the LogiFrame.Frame was disposed.
        /// </summary>
        public void WaitForClose()
        {
            while (!Disposed)
                Thread.Sleep(1500);
        }

        /// <summary>
        ///     Emulates a button being pushed.
        /// </summary>
        public void PerformButtonDown(int button)
        {
            int power = (int) Math.Pow(2, button);

            if ((_buttonState & power) == power)
                return;

            _buttonState = power | _buttonState;

            if (ButtonDown != null)
                ButtonDown(this, new ButtonEventArgs(button));
        }

        /// <summary>
        ///     Emulates a button being released.
        /// </summary>
        public void PerformButtonUp(int button)
        {
            int power = (int) Math.Pow(2, button);

            if ((_buttonState & power) == 0)
                return;

            _buttonState = _buttonState ^ power;

            if (ButtonUp != null)
                ButtonUp(this, new ButtonEventArgs(button));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Pushes the given <paramref name="bytemap"/> to the display.
        /// </summary>
        /// <param name="bytemap">The LogiFrame.Bytemap to push.</param>
        private void UpdateScreen(Bytemap bytemap)
        {
            bool push = true;

            if (Pushing != null)
            {
                PushingEventArgs e = new PushingEventArgs(bytemap);
                Pushing(this, e);

                if (e.PreventPush)
                    push = false;
            }

            if (push)
            {
                _bitmap.pixels = bytemap == null ? new byte[LgLcd.LGLCD_BMP_WIDTH*LgLcd.LGLCD_BMP_HEIGHT] : bytemap.Data;
                LgLcd.lgLcdUpdateBitmap(_openContext.device, ref _bitmap, (uint) UpdatePriority);
            }
        }

        //Callbacks
        /// <summary>
        /// Listener for LgLcd.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="dwButtons"></param>
        /// <param name="pContext"></param>
        /// <returns></returns>
        private int lgLcd_onSoftButtonsCB(int device, int dwButtons, IntPtr pContext)
        {
            for (int i = 0, b = 1; i < 4; i++, b *= 2)
                if (ButtonDown != null && (_buttonState & b) == 0 && (dwButtons & b) == b)
                    ButtonDown(this, new ButtonEventArgs(i));
                else if (ButtonUp != null && (_buttonState & b) == b && (dwButtons & b) == 0)
                    ButtonUp(this, new ButtonEventArgs(i));

            _buttonState = dwButtons;
            return 1;
        }

        /// <summary>
        /// Listener for LgLcd.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="pContext"></param>
        /// <returns></returns>
        private int lgLcd_onConfigureCB(int connection, IntPtr pContext)
        {
            if (Configure != null)
                Configure(this, EventArgs.Empty);
            return 1;
        }

        /// <summary>
        /// Listener for LgLcd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainContainer_ComponentChanged(object sender, EventArgs e)
        {
            if (Disposed || sender == null)
                return;

            if (Size.Width != LgLcd.LGLCD_BMP_WIDTH || Size.Height != LgLcd.LGLCD_BMP_HEIGHT)
                throw new InvalidOperationException("The size of the LogiFrame.Frame container may not be changed.");

            UpdateScreen((sender as Component).Bytemap);
        }

        #endregion
    }
}