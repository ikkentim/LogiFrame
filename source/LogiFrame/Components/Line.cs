using System;
using LogiFrame;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable line.
    /// </summary>
    public class Line : Component
    {

        private Location start = new Location();
        private Location end = new Location();

        /// <summary>
        /// Read-only location of the current LogiFrame.Components.Line.
        /// </summary>
        public override Location Location
        {
            get
            {
                return base.Location;
            }
        }

        /// <summary>
        /// Read-only size of the current LogiFrame.Components.Line.
        /// </summary>
        public override Size Size
        {
            get
            {
                return base.Size;
            }
        }

        /// <summary>
        /// Location in the parent LogiFrame.Components.Container where the line should start at.
        /// </summary>
        public Location Start
        {
            get
            {
                return start;
            }
            set
            {
                start.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - End.X) + 1, Math.Abs(value.Y - End.Y) + 1);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Location in the parent LogiFrame.Components.Container where the line should end at.
        /// </summary>
        public Location End
        {
            get
            {
                return end;
            }
            set
            {
                end.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - Start.X) + 1, Math.Abs(value.Y - Start.Y) + 1);
                HasChanged = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the LogiFrame.components.Line class.
        /// </summary>
        public Line()
            : base()
        {
            start.Changed += start_Changed;
            end.Changed += end_Changed;
        }

        protected override Bytemap Render()
        {
            //TODO: More efficient rendering
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
            Location start = new Location(0, Start.Y <= End.Y ? 0 : Size.Height - 1);
            Location end = new Location(Size.Width - 1, Start.Y > End.Y ? 0 : Size.Height - 1);
            System.Drawing.Graphics.FromImage(bitmap).DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Black), start, end);
            return Bytemap.FromBitmap(bitmap);
        }

        private void end_Changed(object sender, EventArgs e)
        {
            End = end;
        }

        private void start_Changed(object sender, EventArgs e)
        {
            Start = start;
        }
    }
}
