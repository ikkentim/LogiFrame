// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
using LogiFrame;
using LogiFrame.Components;

namespace SanAndreas.Components
{
    internal class NitroInfo : Container
    {
        private readonly Square[] _charges;
        private readonly ProgressBar _progressBar;

        private int _count;
        private float _status;

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
                Components.Add(_charges[i] = new Square
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
            get { return _status; }
            set
            {
                if (SwapProperty(ref _status, value, true))
                    Update();
            }
        }

        public int Count
        {
            get { return _count; }
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