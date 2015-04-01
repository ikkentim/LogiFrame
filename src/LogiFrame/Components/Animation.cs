// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable animation.
    /// </summary>
    public class Animation : Picture
    {
        private readonly Timer _timer;
        private bool _autoInterval = true;
        private int _frame;
        private Snapshot[] _snapshots;

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Animation" /> class.
        /// </summary>
        public Animation()
        {
            _timer = new Timer();
            _timer.Tick += (sender, args) => Frame++;
        }

        /// <summary>
        ///     Gets or sets the time in milliseconds each frame lasts.
        /// </summary>
        public int Interval
        {
            get { return _timer.Interval; }
            set
            {
                if (!AutoInterval)
                    _timer.Interval = value;
            }
        }

        /// <summary>
        ///     Gets or sets whether this <see cref="Animation" /> should automatically calculate its Interval.
        /// </summary>
        public bool AutoInterval
        {
            get { return _autoInterval; }
            set
            {
                if (_autoInterval == value)
                    return;

                _autoInterval = value;

                if (value)
                    _timer.Interval = GetFrameDuration();
            }
        }

        /// <summary>
        ///     Gets or sets the animated <see cref="System.Drawing.Image" /> to be rendered.
        /// </summary>
        public override Image Image
        {
            get { return base.Image; }
            set
            {
                if (base.Image == value)
                    return;

                IsRendering = true;
                base.Image = value;
                IsRendering = false;

                RenderAnimation();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="LogiFrame.ConversionMethod" /> to be used to render the animation.
        /// </summary>
        public override ConversionMethod ConversionMethod
        {
            get { return base.ConversionMethod; }
            set
            {
                if (base.ConversionMethod == value)
                    return;

                IsRendering = true;
                base.ConversionMethod = value;
                IsRendering = false;

                RenderAnimation();
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets or sets the 0-based frame index to be rendered.
        /// </summary>
        public int Frame
        {
            get { return _frame; }
            set
            {
                if (_frame == value)
                    return;

                if (value >= FrameCount)
                    value = 0;

                if (value < 0)
                    value = FrameCount - 1;

                _frame = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets the number of frames available in this animation.
        /// </summary>
        public int FrameCount
        {
            get { return _snapshots == null ? 0 : _snapshots.Length; }
        }

        /// <summary>
        ///     Gets or sets whether the animation should automatically cycle trough its frames.
        /// </summary>
        public bool Run
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Renders this instance.
        /// </summary>
        /// <returns></returns>
        protected override Snapshot Render()
        {
            //Return current frame
            if (_frame < 0 || _snapshots == null || _frame >= _snapshots.Length)
                return Snapshot.Empty;

            return _snapshots[_frame];
        }

        /// <summary>
        ///     Renders and stores every individual frame.
        /// </summary>
        private void RenderAnimation()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Resource was disposed.");

            //If no image is set, don't render anything.
            if (Image == null)
            {
                _snapshots = null;
                return;
            }

            //Calculate frame dimensions
            var dimension = new FrameDimension(Image.FrameDimensionsList[0]);

            // Get numer of frames
            int frames = Image.GetFrameCount(dimension);

            //Create Snapshot for each frame
            _snapshots = new Snapshot[frames];

            //Render bytemaps
            for (int i = 0; i < frames; i++)
            {
                Image.SelectActiveFrame(dimension, i);
                _snapshots[i] = Snapshot.FromBitmap((Bitmap) Image, ConversionMethod);
            }

            //calculate interval
            if (AutoInterval)
                _timer.Interval = GetFrameDuration();

            //check current frame
            if (_frame < 0 || _frame >= frames)
                _frame = 0;
        }

        /// <summary>
        ///     Gets the frame duration of <see cref="Image" />.
        /// </summary>
        /// <returns>The frame duration.</returns>
        private int GetFrameDuration()
        {
            try
            {
                PropertyItem item = Image.GetPropertyItem(0x5100); // 0x5100 is the FrameDelay in libgdiplus
                // Time is in 1/100th of a second
                return (item.Value[0] + item.Value[1]*256)*10;
            }
            catch (Exception)
            {
                return 200;
            }
        }

        #region Overrides of Component

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();

                if (Image != null)
                    Image.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #endregion
    }
}