using System;
using LogiFrame;
using LogiFrame.Components;

namespace SanAndreas.Components
{
    class NitroInfo : Container
    {
        private readonly ProgressBar _progressBar;
        private readonly Square[] _charges;

        private float _status;
        private int _count;

        public NitroInfo()
        {
            Components.Add(_progressBar = new ProgressBar
            {
                Size = new Size(10, 30),
                Location = new Location(20, 0),
                MaximumValue = 1f,
                Horizontal = false,
                Visible = false
            });

            _charges = new Square[9];
            for (int i = 0; i < 9; i++)
            {
               Components.Add( _charges[i] = new Square
                {
                    Fill = true,
                    Size = new Size(4, 4),
                    Location = new Location(6*(i%3), 7 + 6*(i/3)),
                    Visible = false
                });
            }

            base.Size = new Size(30, 30);
        }

        public float Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (SwapProperty(ref _status, value, true))
                    Update();
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (SwapProperty(ref _count, value, true))
                    Update();
            }
        }

        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a SanAndreas.Component.NitroInfo cannot be changed."); }
        }

        private void Update()
        {
            _progressBar.Visible = Count > 0 || Status < 0;
            _progressBar.Value = Status < 0 ? 1 + Status : Status;
            _progressBar.Inverted = Status >= 0;

            for (int i = 0; i < 9; i++)
                _charges[i].Visible = i < Count - ((Status == 1 || Status < 0) ? 1 : 0);
        }
    }
}
