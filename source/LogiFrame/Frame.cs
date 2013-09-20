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
        /// <param name="e">A LogiFrame.ButtonDownEvengArgs that contains the event data.</param>
        public delegate void ButtonDownEventHandler(object sender, ButtonDownEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.ButtonUp.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.ButtonUpEvengArgs that contains the event data.</param>
        public delegate void ButtonUpEventHandler(object sender, ButtonUpEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.FramePush.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.FramePushEventArgs that contains the event data.</param>
        public delegate void FramePushEventHandler(object sender, FramePushEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.FrameClosed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.FrameClosedEventArgs that contains the event data.</param>
        public delegate void FrameClosedEventHandler(object sender, FrameClosedEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.Configure.
        /// </summary>
        /// <param name="sender">The soruce of the event.</param>
        /// <param name="e">A LogiFrame.ConfigureEventArgs that contains the event data.</param>
        public delegate void ConfigureEventHandler(object sender, ConfigureEventArgs e);

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
        public event FramePushEventHandler FramePush;

        /// <summary>
        /// Occurs after the frame has been closed or disposed
        /// </summary>
        public event FrameClosedEventHandler FrameClosed;

        /// <summary>
        /// Occurs when the 'configure' button has been pressed in LCDmon.
        /// </summary>
        public event ConfigureEventHandler Configure;
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
            mainContainer = new Container()
            
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
            UpdateScreen(null);
        }

        /// <summary>
        /// Releases all resources used by LogiFrame.Frame
        /// </summary>
        ~Frame()
        {
            Dispose();
        }
        #endregion

        /// <summary>
        /// Releases all resources used by LogiFrame.Frame
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                //Cannot de-initialize LgLcd from LgLcd-thread.
                //As a precausion disposing resources from another thread
                disposed = true;

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

        //Should be private!
        public void UpdateScreen(Bytemap bytemap)
        {
            bitmap.pixels = bytemap == null ? new byte[LgLcd.LGLCD_BMP_WIDTH * LgLcd.LGLCD_BMP_HEIGHT] : bytemap.Data;
            LgLcd.lgLcdUpdateBitmap(openContext.device, ref bitmap, (uint)UpdatePriority);
        }

        private int ReadSoftButtons()
        {
            if (disposed)
                throw new ObjectDisposedException("LogiFrame.Frame has already been disposed.");

            int intButtons = 0;
            LgLcd.lgLcdReadSoftButtons(openContext.device, out intButtons);
            return intButtons;
        }

        //Callbacks
        private int lgLcd_onSoftButtonsCB(int device, int dwButtons, IntPtr pContext)
        {
            for (int i = 0, b = 1; i < 4; i++, b *= 2)
                if (ButtonDown != null && (buttonState & b) == 0 && (dwButtons & b) == b)
                    ButtonDown(this, new ButtonDownEventArgs(i));
                else if (ButtonUp != null && (buttonState & b) == b && (dwButtons & b) == 0)
                    ButtonUp(this, new ButtonUpEventArgs(i));

            buttonState = dwButtons;
            return 1;
        }

        private int lgLcd_onConfigureCB(int connection, IntPtr pContext)
        {
            if (Configure != null)
                Configure(this, new ConfigureEventArgs());
            return 1;
        }
    }

    #region EventArgs
    /// <summary>
    /// Provides data for the LogiFrame.Frame.ButtonDown event.
    /// </summary>
    public class ButtonDownEventArgs : EventArgs
    {
        /// <summary>
        /// Represents the 0-based number of the button being pressed.
        /// </summary>
        public int Button { get; set; }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.ButtonDownEventArgs class.
        /// </summary>
        /// <param name="button">0-based number of the button being pressed.</param>
        public ButtonDownEventArgs(int button)
        {
            Button = button;
        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.ButtonUp event.
    /// </summary>
    public class ButtonUpEventArgs : EventArgs
    {
        /// <summary>
        /// Represents the 0-based number of the button being released.
        /// </summary>
        public int Button { get; set; }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.ButtonUpEventArgs class.
        /// </summary>
        /// <param name="button">0-based number of the button being released.</param>
        public ButtonUpEventArgs(int button)
        {
            Button = button;
        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.FrameClosed event.
    /// </summary>
    public class FrameClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.FrameClosedEventArgs class.
        /// </summary>
        public FrameClosedEventArgs()
        {

        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.FramePush event.
    /// </summary>
    public class FramePushEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether this frame should be prevented from being 
        /// pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.FramePushEventArgs class.
        /// </summary>
        public FramePushEventArgs()
        {

        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.Configure event.
    /// </summary>
    public class ConfigureEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.ConfigureEventArgs class.
        /// </summary>
        public ConfigureEventArgs()
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
