using System;

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
        /// The priority of the forthcoming LCD updates
        /// </summary>
        public UpdatePriority UpdatePriority { get; set; }

        /// <summary>
        /// Initializes resources
        /// </summary>
        /// <param name="applicationName">A string that contains the 'friendly name' of the application.</param>
        /// <param name="isAutostartable"> Whether true application can be started by LCDMon or not.</param>
        /// <param name="isPersistent">Whether connection is regular.</param>
        public Frame(string applicationName, bool isAutostartable, bool isPersistent)
        {
            //Store parameters
            this.applicationName = applicationName;

            //Initialize connection
            connection.appFriendlyName = applicationName;
            connection.isAutostartable = isAutostartable;
            connection.isPersistent = isPersistent;
            connection.connection = LgLcd.LGLCD_INVALID_CONNECTION;

            //Store connection
            LgLcd.lgLcdInit();
            LgLcd.lgLcdConnect(ref connection);

            openContext.connection = connection.connection;
            openContext.index = 1;

            LgLcd.lgLcdOpen(ref openContext);

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
    }
}
