using System;
using System.ComponentModel;

namespace LogiFrame
{
    /// <summary>
    ///     Represents errors that occur during a connection with a lcd device.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="error">The Win32 error code associated with this exception.</param>
        public ConnectionException(int error) : base("failed to connect to device", new Win32Exception(error))
        {
        }
    }
}