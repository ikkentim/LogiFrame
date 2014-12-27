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
    ///     Provides data for the LogiFrame.Frame.Pushing event.
    /// </summary>
    public class PushingEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the LogiFrame.PushingEventArgs class.
        /// </summary>
        /// <param name="frame">The frame that is about to be pushed.</param>
        public PushingEventArgs(Bytemap frame)
        {
            Frame = frame;
        }

        /// <summary>
        ///     Gets or sets whether this frame should be prevented from being
        ///     pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        /// <summary>
        ///     Gets the frame that is about to be
        /// </summary>
        public Bytemap Frame { get; private set; }
    }
}