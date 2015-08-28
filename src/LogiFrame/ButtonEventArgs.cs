using System;

namespace LogiFrame
{
    public class ButtonEventArgs : EventArgs {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ButtonEventArgs(int button)
        {
            Button = button;
        }

        public int Button { get; }
    }
}