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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a Style of drawing a LogiFrame.Components.ProgressBar.
    /// </summary>
    public enum ProgressBarStyle
    {
        /// <summary>
        ///     A drawing type where there is no border around the ProgressBar.
        /// </summary>
        NoBorder,

        /// <summary>
        ///     A drawing type where there is a border around the ProgressBar.
        /// </summary>
        Border,

        /// <summary>
        ///     A drawing type where there is a border around the ProgressBar,
        ///     and a single pixel of whitespace between the border in the inner bar.
        /// </summary>
        WhiteSpacedBorder
    }
}