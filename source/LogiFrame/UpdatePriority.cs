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

namespace LogiFrame
{
    /// <summary>
    ///     Represents the priority of the forthcoming LCD updates.
    /// </summary>
    public enum UpdatePriority : uint
    {
        /// <summary>
        ///     Lowest priority, disable displaying. Use this priority when you don't have
        ///     anything to show.
        /// </summary>
        IdleNoShow = 0,

        /// <summary>
        ///     Priority used for low priority items.
        /// </summary>
        Background = 64,

        /// <summary>
        ///     Normal priority, to be used by most applications most of the time.
        /// </summary>
        Normal = 128,

        /// <summary>
        ///     Highest priority. To be used only for critical screens, such as 'your CPU
        ///     temperature is too high'
        /// </summary>
        Alert = 255
    }
}