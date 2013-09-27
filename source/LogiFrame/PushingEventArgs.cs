using System;

namespace LogiFrame
{
    /// <summary>
    /// Provides data for the LogiFrame.Frame.Pushing event.
    /// </summary>
    public class PushingEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether this frame should be prevented from being 
        /// pushed to the display.
        /// </summary>
        public bool PreventPush { get; set; }

        private Bytemap frame;
        /// <summary>
        /// The frame that is about to be 
        /// </summary>
        public Bytemap Frame
        {
            get
            {
                return frame;
            }
        }
        /// <summary>
        /// Initializes a new instance of the LogiFrame.PushingEventArgs class.
        /// </summary>
        /// <param name="frame">The frame that is about to be pushed.</param>
        public PushingEventArgs(Bytemap frame)
        {
            this.frame = frame;
        }
    }
}
