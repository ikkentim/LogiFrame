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
using System.Runtime.InteropServices;

namespace LogiFrame
{
    /// <summary>
    ///     Logitech LCD class. This class simply exposes the constants
    ///     and functions provided by the logitech sdk.  No wrapping is done
    ///     to ease any of this as the goal of this class is to simply reflect
    ///     exactly what is done in the C API provided by logitech.
    /// </summary>
    internal static class LgLcd
    {
        /// <summary>
        ///     Function that should be called when the user wants to configure your
        ///     application. If no configuration panel is provided or needed,
        ///     leave this parameter NULL.
        /// </summary>
        /// <param name="connection">Current connection</param>
        /// <param name="pContext">Current context</param>
        /// <returns></returns>
        public delegate int OnConfigureDelegate(int connection, IntPtr pContext);

        /// <summary>
        ///     Function that should be called when the state of the soft buttons changes.
        ///     If no notification is needed, leave this parameter NULL.
        /// </summary>
        /// <param name="device">Device sending buttons</param>
        /// <param name="dwButtons">Mask showing which buttons were pressed</param>
        /// <param name="pContext">Current context</param>
        /// <returns></returns>
        public delegate int OnSoftButtonsDelegate(int device, int dwButtons, IntPtr pContext);

        /// <summary>
        ///     Invalid connection constant.
        /// </summary>
        public const int InvalidConnection = -1;

        /// <summary>
        ///     Invalid Device constant.
        /// </summary>
        public const int InvalidDevice = -1;

        /// <summary>
        ///     Button mask for button 0.
        /// </summary>
        public const uint ButtonButton0 = 0x00000001;

        /// <summary>
        ///     Button mask for button 1.
        /// </summary>
        public const uint ButtonButton1 = 0x00000002;

        /// <summary>
        ///     Button mask for button 2.
        /// </summary>
        public const uint ButtonButton2 = 0x00000004;

        /// <summary>
        ///     Button mask for button 3.
        /// </summary>
        public const uint ButtonButton3 = 0x00000008;

        /// <summary>
        ///     Constant for G15 display resolution (160x43x1)
        /// </summary>
        public const uint BitmapFormat160X43X1 = 0x00000001;

        /// <summary>
        ///     Constant for G15 display width
        /// </summary>
        public static uint BitmapWidth = 160;

        /// <summary>
        ///     Constant for G15 display height
        /// </summary>
        public static uint BitmapHeight = 43;

        /// <summary>
        ///     Lowest priority, disable displaying. Use this priority when you don't have
        ///     anything to show.
        /// </summary>
        public static uint PriorityIdleNoShow = 0;

        /// <summary>
        ///     Priority used for low priority items.
        /// </summary>
        public static uint PriorityBackground = 64;

        /// <summary>
        ///     Normal priority, to be used by most applications most of the time.
        /// </summary>
        public static uint PriorityNormal = 128;

        /// <summary>
        ///     Highest priority. To be used only for critical screens, such as 'your CPU
        ///     temperature is too high'
        /// </summary>
        public static uint PriorityAlert = 255;

        /// <summary>
        ///     The Init() function initializes the Logitech LCD library. You must call this
        ///     function prior to any other function of the library.
        /// </summary>
        /// <remarks>
        ///     No other function in the library can be called before Init() is executed.
        ///     For result codes RPC_S_SERVER_UNAVAILABLE, ERROR_OLD_WIN_VERSION, and
        ///     ERROR_NO_SYSTEM_RESOURCES, the calling application can safely assume
        ///     that the machine it is running on is not set up to do LCD output and therefore
        ///     disable its LCD-related functionality.
        /// </remarks>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     RPC_S_SERVER_UNAVAILABLE
        ///     The Logitech LCD subsystem is not available (this is the case for systems that
        ///     don't have the software installed)
        ///     ERROR_OLD_WIN_VERSION
        ///     Attempted to initialize for Windows 9x. The library only works on Windows 2000
        ///     and above.
        ///     ERROR_NO_SYSTEM_RESOURCES
        ///     Not enough system resources.
        ///     ERROR_ALREADY_INITIALIZED
        ///     Init() has been called before.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdInit")]
        public static extern int Init();

        /// <summary>
        ///     Use DeInit() after you are done using the library in order to release all resources
        ///     that were allocated during Init().
        /// </summary>
        /// <remarks>
        ///     All resources that were allocated during use of the library will be released when
        ///     this function is called. After this function has been called, no further calls to
        ///     the library are permitted with the exception of Init().
        /// </remarks>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     This function does not fail.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdDeInit")]
        public static extern int DeInit();

        /// <summary>
        ///     Use Connect() to establish a connection to the LCD monitor process. This
        ///     connection is required for any other function to find, open and communicate with LCD.
        /// </summary>
        /// <remarks>
        ///     A connection needs to be established for an application to start using LCD
        ///     devices. Connect() attempts to establish that connection. If the LCD
        ///     Monitor process is not running (either because it has not been started or not
        ///     installed (the user is using a different keyboard)), the connection attempt
        ///     will not succeed. In that case, your application should consider running without
        ///     any LCD support.
        ///     Since a string is part of the connection context, this function exists in an ANSI
        ///     and a UNICODE version. The header file picks the appropriate version depending on
        ///     whether the symbol UNICODE is present or not.
        /// </remarks>
        /// <param name="ctx">
        ///     Pointer to a structure which holds all the relevant information about the connection
        ///     which you wish to establish. Upon calling, all fields except the 'connection' member
        ///     need to be filled in; on return from the function, the 'connection' member will be set.
        ///     See <see cref="ConnectContext" /> for details.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     Either ctx or ctx->AppFriendlyName are NULL.
        ///     ERROR_FILE_NOT_FOUND
        ///     LCDMon is not running on the system.
        ///     ERROR_ALREADY_EXISTS
        ///     The same client is already connected.
        ///     RPC_X_WRONG_PIPE_VERSION
        ///     LCDMon does not understand the protocol.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdConnect")]
        public static extern int Connect(ref ConnectContext ctx);

        /// <summary>
        ///     Use Disconnect() to close an existing connection to the LCD monitor process.
        /// </summary>
        /// <remarks>
        ///     Closing a connection invalidates all devices that have been opened using that connection.
        ///     These invalid handles, if used after closing the connection, will cause errors to be
        ///     returned by calls to UpdateBitmap(), ReadSoftButtons(), and Close().
        ///     Additionally, if a callback for configuration had been registered in the call to
        ///     Connect(), it will not be called anymore.
        /// </remarks>
        /// <param name="connection">
        ///     Specifies the connection handle that was returned from a previous successful call
        ///     to Connect()
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     Specified connection handle does not exist.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdDisconnect")]
        public static extern int Disconnect(int connection);

        /// <summary>
        ///     The Enumerate() function is used to retrieve information about all the
        ///     currently attached and supported Logitech LCD devices.
        /// </summary>
        /// <remarks>
        ///     The connection parameter is returned by a call to Connect().
        ///     To enumerate the attached devices, you should call Enumerate() and
        ///     pass in 0 as index parameter. On subsequent calls, increase the index
        ///     parameter by 1 until the function returns ERROR_NO_MORE_ITEMS.
        /// </remarks>
        /// <param name="connection">Specifies the connection that this enumeration refers to.</param>
        /// <param name="index">Specifies which Device information is requested. See Remarks.</param>
        /// <param name="description">
        ///     Points to an DeviceDescription structure which will be filled with information about the
        ///     Device.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_NO_MORE_ITEMS
        ///     There are no more devices to be enumerated. If this error is returned on the first
        ///     call, then there are no devices attached.
        ///     ERROR_INVALID_PARAMETER
        ///     The description pointer is NULL.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdEnumerate")]
        public static extern int Enumerate(int connection, int index, out DeviceDescription description);

        /// <summary>
        ///     The Open() function starts the communication with an attached Device. You have
        ///     to call this function before retrieving button information or updating LCD bitmaps.
        /// </summary>
        /// <remarks>
        ///     A handle retrieved through this function stays valid until either of the following conditions occurs:
        ///     <list type="unordered">
        ///         <item>
        ///             The Device has been unplugged. Any operation with the given handle will result in an
        ///             error code of ERROR_DEVICE_NOT_CONNECTED.
        ///         </item>
        ///         <item>
        ///             The handle has been closed using Close().
        ///         </item>
        ///     </list>
        ///     Part of the opening context is a callback that can be pointed to a function that is to
        ///     be called when soft button changes take place on the LCD. This callback is issued when
        ///     the LCDs soft buttons change while your application owns the LCD space. See the
        ///     definition of lgLcdOpenContext and lgLcdSoftbuttonsChangedContext for details.
        /// </remarks>
        /// <param name="ctx">
        ///     Specifies a pointer to a structure with all the information that is needed to open
        ///     the Device. See lgLcdOpenContext for details. Before calling Open(), all fields
        ///     must be set, except the 'Device' member. Upon successful return, the 'Device' member
        ///     contains the Device handle that can be used in subsequent calls to UpdateBitmap(),
        ///     ReadSoftButtons(), and Close().
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     Either ctx is NULL, or ctx->connection is not valid, or ctx->index does not hold a valid Device.
        ///     ERROR_ALREADY_EXISTS
        ///     The specified Device has already been opened in the given connection.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdOpen")]
        public static extern int Open(ref OpenContext ctx);

        /// <summary>
        ///     The Close() function stops the communication with the previously opened Device.
        /// </summary>
        /// <remarks>
        ///     After calling Close, the soft button callback specified in the call to Open()
        ///     will not be called anymore.
        /// </remarks>
        /// <param name="device">
        ///     Specifies the Device handle retrieved in the lgLcdOpenContext by a previous call to Open().
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     The specified Device handle is invalid.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdClose")]
        public static extern int Close(int device);

        /// <summary>
        ///     The ReadSoftButtons() function reads the current state of the soft buttons
        ///     for the specified Device.
        /// </summary>
        /// <remarks>
        ///     The resulting DWORD contains the current state of the soft buttons, 1 bit per
        ///     button. You can use the mask definitions LGLCDBUTTON_BUTTON0 through
        ///     LGLCDBUTTON_BUTTON3 to check for any particular button in the DWORD.
        ///     If your application is not being currently displayed, you will receive a
        ///     resulting 'buttons' DWORD of 0, even if some soft buttons are being pressed.
        ///     This is in order to avoid users inadvertently interacting with an application that
        ///     does not presently show on the display.
        /// </remarks>
        /// <param name="device">Specifies the Device handle for which to read the soft button state.</param>
        /// <param name="buttons">
        ///     Specifies a pointer to a DWORD that receives the state of the soft buttons at the
        ///     time of the call. See comments for details.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     The specified Device handle or the result pointer is invalid.
        ///     ERROR_DEVICE_NOT_CONNECTED
        ///     The specified Device has been disconnected.
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdReadSoftButtons")]
        public static extern int ReadSoftButtons(int device, out int buttons);

        /// <summary>
        ///     The UpdateBitmap() function updates the bitmap of the Device.
        /// </summary>
        /// <remarks>
        ///     The bitmap header parameter should point to an actual bitmap. The current revision of the
        ///     library defines a structure called lgLcdBitmap160x43x1 which holds as a first member a
        ///     bitmap header. You would typically instantiate once of these structures, set the hdr.Format
        ///     to LGLCD_BMP_FORMAT_160x43x1, then fill in the bitmap to be displayed in the pixels[] member.
        ///     Finally, you call UpdateBitmap(' yourBitmap.hdr ') to issue the bitmap update. Future
        ///     versions of the SDK could have additional bitmap types declared, but all of them will have
        ///     the same header at the beginning.
        ///     At any given time there may be multiple applications attempting to display a bitmap on the LCD.
        ///     The priority parameter is a hint for LCDMon's display scheduling algorithm. In a scenario
        ///     where there is contention for screen display time, LCDMon needs to determine which application's
        ///     bitmap to display. In order to aid this scheduling, it can (but depending on user settings
        ///     might not) take into account the hints that an application gives through the priority parameter.
        ///     It is highly advisable that your application gives the appropriate priority for any given screen
        ///     update to improve the user experience. A well-behaved LCD-enabled application should not use
        ///     high priorities except for alerts.
        ///     The difference between asynchronous and synchronous updates is as follows: an asynchronous
        ///     update will place the bitmap to be displayed into LCDMon and return immediately, before the
        ///     bitmap is actually sent out to the Device. For synchronous updates, the call to
        ///     UpdateBitmap() will only return after the bitmap has been sent out to the Device,
        ///     which takes 30 milliseconds or more. In case the application currently does not show on
        ///     the LCD because another application is displayed, the synchronous update returns after a
        ///     time that is similar to an update when the application is visible. If the macro
        ///     LGLCD_SYNC_COMPLETE_WITHIN_FRAME() is used, an error is returned to the calling
        ///     application when this condition arises.
        ///     The benefit of using the synchronous update is that your application will run 'locked' with
        ///     the LCD updates. It will be suspended for the entire duration of writing to the screen,
        ///     and only get to run when the display is ready to accept a new screen. A 'mini-game' on the
        ///     LCD would profit from this behavior in order to get the highest possible frame rates while
        ///     minimizing CPU usage.
        ///     The asynchronous updates are beneficial to applications that don't care about the exact
        ///     sequence and timing of screen updates and have many other things to do. They just deposit
        ///     a bitmap to be sent to the Device every once in a while and don't worry about it actually
        ///     going out and being in sync with this event.
        /// </remarks>
        /// <param name="device">Specifies the Device handle for which to update the display.</param>
        /// <param name="bitmap">Specifies a pointer to a bitmap header structure. See comments for details.</param>
        /// <param name="priority">
        ///     Specifies a priority hint for this screen update, as well as whether the update should
        ///     take place synchronously or asynchronously. See comments for details.
        ///     The following priorities are defined:
        ///     LGLCD_PRIORITY_IDLE_NO_SHOW
        ///     Lowest priority, disable displaying. Use this priority when you don't have
        ///     anything to show.
        ///     LGLCD_PRIORITY_BACKGROUND
        ///     Priority used for low priority items.
        ///     LGLCD_PRIORITY_NORMAL
        ///     Normal priority, to be used by most applications most of the time.
        ///     LGLCD_PRIORITY_ALERT
        ///     Highest priority. To be used only for critical screens, such as 'your CPU
        ///     temperature is too high'
        ///     In addition, there are three macros that can be used to indicate whether the screen
        ///     should be updated synchronously (LGLCD_SYNC_UPDATE()) or asynchronously (LGLCD_ASYNC_UPDATE()).
        ///     When using synchronous update the LCD library can notify the calling application of whether
        ///     the bitmap was displayed or not on the LCD, using the macro (LGLCD_SYNC_COMPLETE_WITHIN_FRAME()).
        ///     Use these macros to indicate the behavior of the library.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value can be one of the following:
        ///     ERROR_SERVICE_NOT_ACTIVE
        ///     Init() has not been called yet.
        ///     ERROR_INVALID_PARAMETER
        ///     The specified Device handle, the bitmap header
        ///     pointer or the type of bitmap is invalid.
        ///     ERROR_DEVICE_NOT_CONNECTED
        ///     The specified Device has been disconnected.
        ///     ERROR_ACCESS_DENIED
        ///     Synchronous operation was not displayed on the LCD within the frame interval
        ///     (30 ms). This error code is only returned when the priority field of the
        ///     lgLCDUpdateBitmap uses the macro LGLCD_SYNC_COMPLETE_WITHIN_FRAME().
        ///     Other (system) error with appropriate error code.
        /// </returns>
        [DllImport("LgLcd.dll", EntryPoint = "lgLcdUpdateBitmap")]
        public static extern int UpdateBitmap(int device, ref Bitmap160X43X1 bitmap, uint priority);

        /// <summary>
        ///     160x43x1 bitmap.  This includes a header and an array
        ///     of bytes (1 for each pixel.)
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Bitmap160X43X1
        {
            /// <summary>
            ///     Header information telling what kind of bitmap this structure
            ///     represents (currently only one format exists, see lgLcdBitmapHeader.)
            /// </summary>
            public BitmapHeader hdr;

            /// <summary>
            ///     Contains the display bitmap with 160x43 pixels. Every byte represents
            ///     one pixel, with &gt;=128 being 'on' and &lt;128 being 'off'.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6880)] public byte[] pixels;
        }

        /// <summary>
        ///     The BitmapHeader exists at the beginning of any bitmap structure
        ///     defined in lgLcd. Following the header is the actual bitmap as an array
        ///     of bytes, as illustrated by lgLcdBitmap160x43x1.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct BitmapHeader
        {
            /// <summary>
            ///     Specifies the format of the structure following the header.
            ///     Currently, only LGLCD_BMP_FORMAT_160x43x1 is supported
            /// </summary>
            public uint Format;
        }

        /// <summary>
        ///     The ConfigureContext is part of the ConnectContext and
        ///     is used to give the library enough information to allow the user
        ///     to configure your application. The registered callback is called when the user
        ///     clicks the 'Configure'' button in the LCDMon list of applications.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ConfigureContext
        {
            /// <summary>
            ///     Specifies a pointer to a function that should be called when the
            ///     user wants to configure your application. If no configuration panel
            ///     is provided or needed, leave this parameter NULL.
            /// </summary>
            public OnConfigureDelegate ConfigCallback;

            /// <summary>
            ///     Specifies an arbitrary context value of the application that is passed
            ///     back to the client in the event that the registered ConfigCallback
            ///     function is invoked.
            /// </summary>
            public IntPtr ConfigContext;
        }

        /// <summary>
        ///     The ConnectContext contains all the information that is needed to
        ///     connect your application to LCDMon through Connect(). Upon successful connection,
        ///     it also contains the connection handle that has to be used in subsequent calls to
        ///     Enumerate() and Open().
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ConnectContext
        {
            /// <summary>
            ///     Specifies a string that contains the 'friendly name' of your application.
            ///     This name is presented to the user whenever a list of applications is shown.
            /// </summary>
            public string AppFriendlyName;

            /// <summary>
            ///     Specifies whether your connection is temporary (.isPersistent = FALSE) or
            ///     whether it is a regular connection that should be added to the list
            ///     (.isPersistent = TRUE).
            /// </summary>
            public bool IsPersistent;

            /// <summary>
            ///     Specifies whether your application can be started by LCDMon or not.
            /// </summary>
            public bool IsAutostartable;

            /// <summary>
            ///     Specifies context that is necessary to call back for configuration of
            ///     your application. See ConfigureContext for more details.
            /// </summary>
            public ConfigureContext OnConfigure;

            /// <summary>
            ///     Upon successful connection, this member holds the 'connection handle'
            ///     which is used in subsequent calls to Enumerate() and Open().
            ///     A value of LGLCD_INVALID_CONNECTION denotes an invalid connection.
            /// </summary>
            public int Connection;
        }

        /// <summary>
        ///     The lgLcdDeviceDesc structure describes the properties of an attached Device.
        ///     This information is returned through a call to Enumerate().
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceDescription
        {
            /// <summary>
            ///     Specifies the width of the display in pixels.
            /// </summary>
            public int Width;

            /// <summary>
            ///     Specifies the height of the display in pixels.
            /// </summary>
            public int Height;

            /// <summary>
            ///     Specifies the depth of the bitmap in bits per pixel.
            /// </summary>
            public int BitsPerPixel;

            /// <summary>
            ///     Specifies the number of soft buttons that the Device has.
            /// </summary>
            public int SoftButtonsCount;
        }

        /// <summary>
        ///     The lgLcdOpenContext contains all the information that is needed to open
        ///     a specified LCD display through Open(). Upon successful completion
        ///     of the open it contains the Device handle that has to be used in subsequent
        ///     calls to ReadSoftButtons(), UpdateBitmap() and Close().
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OpenContext
        {
            /// <summary>
            ///     Specifies the connection (previously opened through Connect()) which
            ///     this Open() call is for.
            /// </summary>
            public int Connection;

            /// <summary>
            ///     Specifies the index of the Device to open (see Enumerate() for details).
            /// </summary>
            public int Index;

            /// <summary>
            ///     Specifies the details for the callback function that should be invoked when
            ///     Device has changes in its soft button status, i.e. the user has pressed or
            ///     a soft button. For details, see SoftButtonsChangedContext.
            /// </summary>
            public SoftButtonsChangedContext OnSoftButtonsChanged;

            /// <summary>
            ///     Upon successful opening, this member holds the Device handle which is used
            ///     in subsequent calls to ReadSoftButtons(), UpdateBitmap() and
            ///     Close(). A value of LGLCD_INVALID_DEVICE denotes an invalid Device.
            /// </summary>
            public int Device;
        }

        /// <summary>
        ///     The lgLcdSoftbuttonsChangedContext is part of the lgLcdOpenContext and
        ///     is used to give the library enough information to allow changes in the
        ///     state of the soft buttons to be signaled into the calling application
        ///     through a callback.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SoftButtonsChangedContext
        {
            /// <summary>
            ///     Specifies a pointer to a function that should be called when the
            ///     state of the soft buttons changes. If no notification is needed,
            ///     leave this parameter NULL.
            /// </summary>
            public OnSoftButtonsDelegate Callback;

            /// <summary>
            ///     Specifies an arbitrary context value of the application that is
            ///     passed back to the client in the event that soft buttons are being
            ///     pressed or released. The new value of the soft buttons is reported
            ///     in the dwButtons parameter of the callback function.
            /// </summary>
            public IntPtr Context;
        }
    }
}