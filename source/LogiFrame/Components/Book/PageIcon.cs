using System;

namespace LogiFrame.Components.Book
{
    /// <summary>
    /// Represents a drawable icon of a LogiFrame.Components.Book.Page.
    /// </summary>
    public class PageIcon : Container
    {
        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.Book.PageIcon class.
        /// </summary>
        public PageIcon()
        {
            base.Size = new Size(16, 16);
        }

        /// <summary>
        /// Gets the LogiFrame.Location this LogiFrame.Components.Book.PageIcon should
        /// be rendered at within the parrent LogiFrame.Components.Container.
        /// </summary>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }


        /// <summary>
        /// Gets the LogiFrame.Size of this LogiFrame.Components.Book.PageIcon.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }
    }
}
