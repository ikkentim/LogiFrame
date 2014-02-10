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
using LogiFrame.Components.Book;

namespace SanAndreas.Pages
{
    internal class Detector : Page
    {
        private readonly Timer _timer;

        public Detector()
        {
            Components.Add(new Label
            {
                AutoSize = true,
                Text = "Looking for GTA..."
            });

            Components.Add(_timer = new Timer
            {
                Interval = 1000
            });
            _timer.Tick += (sender, args) =>
            {
                if (!GTA.IsRunning) return;
                var book = GetParentComponent<Book>();
                if (book == null) return;
                book.SwitchTo<OnFoot>();
            };
        }

        public override void OnShow(EventArgs e)
        {
            _timer.Enabled = true;
            var frame = GetParentComponent<Frame>();
            if (frame == null) return;
            frame.UpdatePriority = UpdatePriority.IdleNoShow;
        }

        public override void OnHide(EventArgs e)
        {
            _timer.Enabled = false;
            var frame = GetParentComponent<Frame>();
            if (frame == null) return;
            frame.UpdatePriority = UpdatePriority.Normal;
        }

        public override bool IsBrowsable()
        {
            return false;
        }
    }
}