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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents modifications which can be set to the Rotation property of LogiFrame.Components.Rotator.
    /// </summary>
    [Flags]
    public enum Rotation
    {
        /// <summary>
        ///     Rotates the container 0 degrees.
        /// </summary>
        Rotate0Degrees = 0,

        /// <summary>
        ///     Rotates the container 90 degrees.
        /// </summary>
        Rotate90Degrees = 1,

        /// <summary>
        ///     Rotates the container 180 degrees.
        /// </summary>
        Rotate180Degrees = 2,

        /// <summary>
        ///     Rotates the container 270 degrees.
        /// </summary>
        Rotate270Degrees = 4,

        /// <summary>
        ///     Flips the container horizontally.
        /// </summary>
        FlipHorizontal = 8,

        /// <summary>
        ///     Flips the container vertically.
        /// </summary>
        FlipVertical = 16
    }
}