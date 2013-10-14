//     Animation.cs
// 
//     LogiFrame rendering library.
//     Copyright (C) 2013  Tim Potze
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable animation.
    /// </summary>
    public class Animation : Picture
    {
        #region Fields

        private bool _autoInterval = true;
        private Bytemap[] _bytemaps;
        private int _frame;
        private int _interval;
        private bool _run;
        private Thread _thread;

        #endregion

        #region Properties

        /// <summary>
        ///     The time in MS each frame lasts.
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                if (!AutoInterval)
                    _interval = value;
            }
        }

        /// <summary>
        ///     Whether the current LogiFrame.Components.Animation should
        ///     automatically calculate the Interval.
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
                    Interval = GetFrameDuration();
            }
        }

        /// <summary>
        ///     The animated image to be rendered.
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
                HasChanged = true;
            }
        }

        /// <summary>
        ///     The conversion method to be used to render the animation.
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
                HasChanged = true;
            }
        }

        /// <summary>
        ///     The 0-based frame index to be rendered.
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
                HasChanged = true;
            }
        }

        /// <summary>
        ///     The number of frames in the current animation.
        /// </summary>
        public int FrameCount
        {
            get
            {
                if (_bytemaps == null)
                    return 0;

                return _bytemaps.Length;
            }
        }

        /// <summary>
        ///     Whether the animation should automatically cycle trough its frames.
        /// </summary>
        public bool Run
        {
            get { return _run; }
            set
            {
                if (_run == value)
                    return;

                _run = value;

                if (_run && _thread == null)
                {
                    _thread = new Thread(AnimationThread);
                    _thread.Start();
                }
            }
        }

        #endregion

        #region Methods

        protected override Bytemap Render()
        {
            //Return current frame
            if (_frame < 0 || _bytemaps == null || _frame >= _bytemaps.Length)
                return null;

            return _bytemaps[_frame];
        }

        protected override void DisposeComponent()
        {
            _run = false;
            Image.Dispose();
        }

        private void RenderAnimation()
        {
            //If no image is set, don't render anything.
            if (Image == null)
            {
                _bytemaps = null;
                return;
            }

            //Calculate frame dimensions
            FrameDimension dimension = new FrameDimension(Image.FrameDimensionsList[0]);

            // Get numer of frames
            int frames = Image.GetFrameCount(dimension);

            //Create bytemap for each frame
            _bytemaps = new Bytemap[frames];

            //Render bytemaps
            for (int i = 0; i < frames; i++)
            {
                Image.SelectActiveFrame(dimension, i);
                _bytemaps[i] = Bytemap.FromBitmap((Bitmap) Image, ConversionMethod);
            }

            //calculate interval
            if (AutoInterval)
                Interval = GetFrameDuration();

            //check current frame
            if (_frame < 0 || _frame >= frames)
                _frame = 0;
        }

        private int GetFrameDuration()
        {
            try
            {
                PropertyItem item = Image.GetPropertyItem(0x5100); // FrameDelay in libgdiplus
                // Time is in 1/100th of a second
                return (item.Value[0] + item.Value[1]*256)*10;
            }
            catch //any exception
            {
                return 200;
            }
        }

        private void AnimationThread()
        {
            while (Run && Interval > 0)
            {
                Frame++;
                Thread.Sleep(Interval);
            }
            _thread = null;
        }

        #endregion
    }
}