using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace LogiFrame.Components
{
    public class Animation : Picture
    {
        private Bytemap[] _bytemaps;

        private bool _run = false;
        private int _frame;

        private bool _autoInterval = true;

        private Thread _thread = null;

        public int Interval { get; set; }

        public bool AutoInterval
        {
            get { return _autoInterval; }
            set
            {
                if (_autoInterval == value)
                    return;

                _autoInterval = value;

                if(value)
                    Interval = GetFrameDuration();
            }
        }

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

        public int FrameCount
        {
            get
            {
                if (_bytemaps == null)
                    return 0;

                return _bytemaps.Length;
            }
        }

        public bool Run
        {
            get
            {
                return _run;
            }
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

        public void Next()
        {
            Frame++;
        }

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
            if (Image == null)
            {
                _bytemaps = null;
                return;
            }
            FrameDimension dimension = new FrameDimension(Image.FrameDimensionsList[0]);

            // Get numer of frames
            int frames = Image.GetFrameCount(dimension);

            //Create bytemap for each frame
            _bytemaps = new Bytemap[frames];

            //Render bytemaps
            for (int i = 0; i < frames; i++)
            {
                Image.SelectActiveFrame(dimension, i);
                _bytemaps[i] = Bytemap.FromBitmap((Bitmap)Image, ConversionMethod);
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
                return (item.Value[0] + item.Value[1] * 256) * 10;
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
                Next();
                Thread.Sleep(Interval);
            }
            _thread = null;
        }
    }
}
