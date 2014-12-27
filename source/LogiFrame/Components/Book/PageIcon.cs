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
using System.Collections.Generic;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a drawable icon of a LogiFrame.Components.Book.Page.
    /// </summary>
    public class PageIcon : Container
    {
        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.Book.PageIcon class.
        /// </summary>
        public PageIcon()
        {
            base.Size = new Size(16, 16);
        }

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.Components.Book.PageIcon class.
        /// </summary>
        /// <param name="components">Compoents to be added to this container.</param>
        public PageIcon(IEnumerable<Component> components) : this()
        {
            foreach (Component c in components)
                Components.Add(c);
        }

        /// <summary>
        ///     Gets the LogiFrame.Location this LogiFrame.Components.Book.PageIcon should
        ///     be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set
            {
                throw new ArgumentException("The Location of a LogiFrame.Components.Book.PageIcon cannot be changed.");
            }
        }


        /// <summary>
        ///     Gets the LogiFrame.Size of this LogiFrame.Components.Book.PageIcon.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }
    }
}