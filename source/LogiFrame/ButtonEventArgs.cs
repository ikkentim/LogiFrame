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
    ///     Provides data for the LogiFrame.Frame.ButtonDown and LogiFrame.Frame.ButtonUp events.
    /// </summary>
    public class ButtonEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the LogiFrame.ButtonEventArgs class.
        /// </summary>
        /// <param name="button">0-based number of the button being pressed.</param>
        public ButtonEventArgs(int button)
        {
            Button = button;
        }

        /// <summary>
        ///     Gets the 0-based number of the button being pressed.
        /// </summary>
        public int Button { get; private set; }
    }
}