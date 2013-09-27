using System;

namespace LogiFrame
{
    /// <summary>
    /// Represents errors that occur during the connection to LCDmon.
    /// </summary>
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
