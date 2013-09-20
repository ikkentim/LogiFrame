using System;
using System.Threading;

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

        private bool disposed = false;

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
        /// Represents the method that handles a LogiFrame.Frame.Tick.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LogiFrame.TickEventArgs that contains the event data.</param>
        public delegate void TickEventHandler(object sender, TickEventArgs e);

        /// <summary>
        /// Represents the method that handles a LogiFrame.Frame.Tick.
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
        /// Occurs when a button is being pressed.
        /// </summary>
        public event ButtonDownEventHandler ButtonDown;

        /// <summary>
        /// Occurs when a button is being released.
        /// </summary>
        public event ButtonUpEventHandler ButtonUp;

        /// <summary>
        /// Occurs after a tick.
        /// </summary>
        public event TickEventHandler Tick;

        /// <summary>
        /// Occurs before a frame is being pushed to the display.
        /// </summary>
        public event FramePushEventHandler FramePush;

        /// <summary>
        /// Occurs after the frame has been closed or disposed
        /// </summary>
        public event FrameClosedEventHandler FrameClosed;
        #endregion

        #region Properties
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

        /// <summary>
        /// The priority of the forthcoming LCD updates.
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }
        #endregion

        #region Constructor/Deconstructor
        /// <summary>
        /// Initializes a new instance of the LogiFrame.Frame class.
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent)
        {
            //Initialize connection and store properties
            connection.appFriendlyName = this.applicationName = applicationName;
            connection.isAutostartable = this.isAutostartable = isAutostartable;
            connection.isPersistent = this.isPersistent = isPersistent;
            connection.connection = LgLcd.LGLCD_INVALID_CONNECTION;

            //Store connection
            LgLcd.lgLcdInit();
            LgLcd.lgLcdConnect(ref connection);

            openContext.connection = connection.connection;
            openContext.onSoftbuttonsChanged.softbuttonsChangedCallback = lgLcd_onSoftButtonsCB;
            openContext.index = 1;

            LgLcd.lgLcdOpen(ref openContext);

            

            //Store bitmap format
            bitmap.hdr = new LgLcd.lgLcdBitmapHeader();
            bitmap.hdr.Format = LgLcd.LGLCD_BMP_FORMAT_160x43x1;
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
                LgLcd.lgLcdClose(openContext.device);
                LgLcd.lgLcdDisconnect(connection.connection);
                LgLcd.lgLcdDeInit();

                disposed = true;
            }
        }

        /*
        public void UpdateScreen(Bytemap bytemap)
        {
            bmp.pixels = bytemap == null ? new byte[LgLcd.LGLCD_BMP_WIDTH * LgLcd.LGLCD_BMP_HEIGHT] : bytemap.Data;

            LgLcd.lgLcdUpdateBitmap(openContext.device, ref bmp, UpdatePriority);

        }*/

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
        /// Represents the 1-based number of the button being pressed.
        /// </summary>
        public int Button;

        /// <summary>
        /// Initializes a new instance of the LogiFrame.ButtonDownEventArgs class.
        /// </summary>
        /// <param name="button"></param>
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
        /// Represents the 1-based number of the button being released.
        /// </summary>
        public int Button;

        /// <summary>
        /// Initializes a new instance of the LogiFrame.ButtonUpEventArgs class.
        /// </summary>
        /// <param name="button"></param>
        public ButtonUpEventArgs(int button)
        {
            Button = button;
        }
    }

    /// <summary>
    /// Provides data for the LogiFrame.Frame.Tick event.
    /// </summary>
    public class TickEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.TickEventArgs class.
        /// </summary>
        public TickEventArgs()
        {

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

    #endregion

}
