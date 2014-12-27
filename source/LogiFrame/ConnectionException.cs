// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System;

namespace LogiFrame
{
    /// <summary>
    ///     Represents errors that occur during a connection with LCDmon.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the LogiFrame.ConnectionException class with a specified
        ///     error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConnectionException(string message)
            : base(message)
        {
        }
    }
}