using System;

namespace LogiFrame.Components.Book
{
    public class PageIcon : Container
    {
        public PageIcon()
        {
            base.Size = new Size(16, 16);
        }

        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }

        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }
    }
}
