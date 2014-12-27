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
    ///     Provides data for the LogiFrame.Components.ComponentCollection.ComponentAdded and
    ///     LogiFrame.Components.ComponentCollection.ComponentRemoved event.
    /// </summary>
    public class ComponentChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initilizes a new instance of the LogiFrame.Components.ComponentChangedEventArgs
        /// </summary>
        /// <param name="component">The component that has been changed.</param>
        public ComponentChangedEventArgs(Component component)
        {
            Component = component;
        }

        /// <summary>
        ///     Gets the component which has been changed.
        /// </summary>
        public Component Component { get; private set; }
    }
}