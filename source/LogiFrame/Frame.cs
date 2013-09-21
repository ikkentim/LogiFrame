using System;
using System.Threading;
using System.Diagnostics;
using LogiFrame.Components;

namespace LogiFrame
{
    /// <summary>
    /// Represents the framework.
    /// </summary>
    public class Frame : IDisposable
    {
        private LgLcd.lgLcdConnectContext connection = new LgLcd.lgLcdConnectContext();
        private LgLcd.lgLcdOpenContext openContext = new LgLcd.lgLcdOpenContext();
        private LgLcd.lgLcdBitmap160x43x1 bitmap = new LgLcd.lgLcdBitmap160x43x1();

        private int buttonState = 0;

        #region Events
        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.ButtonDown.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ButtonEventArgs that contains the event data.</param>
        public delegate void ButtonDownEventHandler(object sender, ButtonEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.ButtonUp.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ButtonEventArgs that contains the event data.</param>
        public delegate void ButtonUpEventHandler(object sender, ButtonEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.FramePush.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.PushingEventArgs that contains the event data.</param>
        public delegate void PushingEventHandler(object sender, PushingEventArgs e);

        /// <summary>
        /// Occurs when a button is being pressed.
        /// </summary>
        public event ButtonDownEventHandler ButtonDown;

        /// <summary>
        /// Occurs when a button is being released.
        /// </summary>
        public event ButtonUpEventHandler ButtonUp;

        /// <summary>
        /// Occurs before a frame is being pushed to the display.
        /// </summary>
        public event PushingEventHandler Pushing;

        /// <summary>
        /// Occurs after the frame has been closed or disposed
        /// </summary>
        public event EventHandler FrameClosed;

        /// <summary>
        /// Occurs when the 'configure' button has been pressed in LCDmon.
        /// </summary>
        public event EventHandler Configure;
        #endregion

        #region Properties
        private bool disposed;

        /// <summary>
        /// Whether the LogiFrame.Frame has been disposed.
        /// </summary>
        public bool Disposed
        {
            get
            {
                return disposed;
            }
        }

        private string applicationName;

        /// <summary>
        /// A string that contains the 'friendly name' of the application. 
        /// This name is presented to the user whenever a list of applications is shown.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
        }

        private bool isAutostartable;

        /// <summary>
        /// Whether application can be started by LCDMon or not.
        /// </summary>
        public bool IsAutostartable
        {
            get
            {
                return isAutostartable;
            }
        }

        private bool isPersistent;

        /// <summary>
        /// Whether connection is temporary or whether it is
        /// a regular connection that should be added to the list.
        /// </summary>
        public bool IsPersistent
        {
            get
            {
                return isPersistent;
            }
        }

        private bool allowConfiguration;

        /// <summary>
        /// Whether the 'configure' option is being shown in LCDmon.
        /// </summary>
        public bool AllowConfiguration
        {
            get
            {
                return allowConfiguration;
            }
        }
        /// <summary>
        /// The priority of the forthcoming LCD updates.
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }

        private Container mainContainer;

        /// <summary>
        /// The main components container
        /// </summary>
        public Container MainContainer
        {
            get
            {
                return mainContainer;
            }
        }
        #endregion

        #region Constructor/Deconstructor
        /// <summary>
        /// Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        /// <param name="allowConfiguration">Whether the application is configurable via LCDmon</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent, bool allowConfiguration)
        {
            //Check for LgLcd.dll file
            if (!System.IO.File.Exists("LgLcd.dll"))
                throw new DllNotFoundException("Could not find LgLcd.dll.");

            //Initialize connection and store properties
            connection.appFriendlyName = this.applicationName = applicationName;
            connection.isAutostartable = this.isAutostartable = isAutostartable;
            connection.isPersistent = this.isPersistent = isPersistent;

            if(this.allowConfiguration = allowConfiguration)
                connection.onConfigure.configCallback = lgLcd_onConfigureCB;

            connection.connection = LgLcd.LGLCD_INVALID_CONNECTION;

            //Set default updatepriority
            UpdatePriority = LogiFrame.UpdatePriority.Normal;

            //Initialize main container
            mainContainer = new Container();
            mainContainer.Size = new Size((int)LgLcd.LGLCD_BMP_WIDTH, (int)LgLcd.LGLCD_BMP_HEIGHT);
            mainContainer.Changed += new EventHandler(mainContainer_ComponentChanged);

            //Store connection
            LgLcd.lgLcdInit();
            int connectionResponse = LgLcd.lgLcdConnect(ref connection);

            //Check if a connection is set or throw an Exception
            if(connectionResponse != Win32Error.ERROR_SUCCESS)
                throw new ConnectionException(Win32Error.ToString(connectionResponse));


            openContext.connection = connection.connection;
            openContext.onSoftbuttonsChanged.softbuttonsChangedCallback = lgLcd_onSoftButtonsCB;
            openContext.index = 0;

            LgLcd.lgLcdOpen(ref openContext);

            //Store bitmap format
            bitmap.hdr = new LgLcd.lgLcdBitmapHeader();
            bitmap.hdr.Format = LgLcd.LGLCD_BMP_FORMAT_160x43x1;

            //Send empty bytemap
            if(!Disposed)
                updateScreen(null);
        }

        /// <summary>
        /// Releases all resources used by LogiFrame.Frame
        /// </summary>
        ~Frame()
        {
            Dispose();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Releases all resources used by LogiFrame.Frame
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                mainContainer.Dispose();

                disposed = true;

                if (FrameClosed != null)
                    FrameClosed(this, EventArgs.Empty);

                //Cannot de-initialize LgLcd from LgLcd-thread.
                //As a precausion disposing resources from another thread
                new Thread(() =>
                {
                    LgLcd.lgLcdClose(openContext.device);
                    LgLcd.lgLcdDisconnect(connection.connection);
                    LgLcd.lgLcdDeInit();

                }).Start();
            }
        }

        /// <summary>
        /// Waits untill the LogiFrame.Frame was disposed.
        /// </summary>
        public void WaitForClose()
        {
            while(!disposed)
                Thread.Sleep(1500);
        }

        private void updateScreen(Bytemap bytemap)
        {
            bool push = true;

            if (Pushing != null)
            {
                PushingEventArgs e = new PushingEventArgs();
                Pushing(this, e);

                if (e.PreventPush)
                    push = false;
            }

            if (push)
            {
                bitmap.pixels = bytemap == null ? new byte[LgLcd.LGLCD_BMP_WIDTH * LgLcd.LGLCD_BMP_HEIGHT] : bytemap.Data;
                LgLcd.lgLcdUpdateBitmap(openContext.device, ref bitmap, (uint)UpdatePriority);
            }
        }

        private int ReadSoftButtons()
        {
            int intButtons = 0;
            LgLcd.lgLcdReadSoftButtons(openContext.device, out intButtons);
            return intButtons;
        }

        //Callbacks
        private int lgLcd_onSoftButtonsCB(int device, int dwButtons, IntPtr pContext)
        {
            for (int i = 0, b = 1; i < 4; i++, b *= 2)
                if (ButtonDown != null && (buttonState & b) == 0 && (dwButtons & b) == b)
                    ButtonDown(this, new ButtonEventArgs(i));
                else if (ButtonUp != null && (buttonState & b) == b && (dwButtons & b) == 0)
                    ButtonUp(this, new ButtonEventArgs(i));

            buttonState = dwButtons;
            return 1;
        }

        private int lgLcd_onConfigureCB(int connection, IntPtr pContext)
        {
            if (Configure != null)
                Configure(this, EventArgs.Empty);
            return 1;
        }

        private void mainContainer_ComponentChanged(object sender, EventArgs e)
        {
            if (Disposed)
                return;

            updateScreen((sender as Component).Bytemap);
        }
        #endregion
    }

    #region EventArgs
    /// <summary>
    /// Provides data for the LogiFrame.Frame.ButtonDown and LogiFrame.Frame.ButtonUp events.
    /// </summary>
    public class ButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Represents the 0-based number of the button being pressed.
        /// </summary>
        public int Button { get; set; }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.ButtonEventArgs class.
        /// </summary>
        /// <param name="button">0-based number of the button being pressed.</param>
        public ButtonEventArgs(int button)
        {
            Button = button;
        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.Pushing event.
    /// </summary>
    public class PushingEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether this frame should be prevented from being 
        /// pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.PushingEventArgs class.
        /// </summary>
        public PushingEventArgs()
        {

        }
    }

    #endregion

    public class ConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.ConnectionException class with a specified
        /// error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConnectionException(string message)
            : base(message)
        {
        }
    }
}
