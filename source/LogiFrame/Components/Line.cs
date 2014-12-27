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
using System.Drawing;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable line.
    /// </summary>
    public class Line : Component
    {
        #region Fields

        private readonly Location _end = new Location();
        private readonly Location _start = new Location();

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the LogiFrame.components.Line class.
        /// </summary>
        public Line()
        {
            _start.Changed += start_Changed;
            _end.Changed += end_Changed;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should start
        ///     at.
        /// </summary>
        public Location Start
        {
            get { return _start; }
            set
            {
                _start.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - End.X) + 1, Math.Abs(value.Y - End.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the LogiFrame.Location within the parent LogiFrame.Components.Container where the line should end at.
        /// </summary>
        public Location End
        {
            get { return _end; }
            set
            {
                _end.Set(value);
                base.Location = new Location(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
                base.Size = new Size(Math.Abs(value.X - Start.X) + 1, Math.Abs(value.Y - Start.Y) + 1);
                OnChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        protected override Bytemap Render()
        {
            //TODO: More efficient rendering
            var bitmap = new Bitmap(Size.Width, Size.Height);
            var start = new Location(0, Start.Y <= End.Y ? 0 : Size.Height - 1);
            var end = new Location(Size.Width - 1, Start.Y > End.Y ? 0 : Size.Height - 1);
            Graphics.FromImage(bitmap)
                .DrawLine(new Pen(Brushes.Black), start, end);
            return Bytemap.FromBitmap(bitmap);
        }

        /// <summary>
        ///     Listener for Location.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void end_Changed(object sender, EventArgs e)
        {
            End = _end;
        }

        /// <summary>
        ///     Listener for Location.Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Changed(object sender, EventArgs e)
        {
            Start = _start;
        }

        #endregion
    }
}