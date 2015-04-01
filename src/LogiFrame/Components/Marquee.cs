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
using System.Linq;
using LogiFrame.Collections;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a scrolling text.
    /// </summary>
    public class Marquee : Container
    {
        private readonly Label _label = new Label {IsAutoSize = true};
        private readonly WatchableCollection<Marquee> _syncedMarquees = new WatchableCollection<Marquee>();
        private readonly Timer _timer = new Timer();
        private int _currentStep;
        private int _endStepsCount;
        private int _stepSize = 1;
        private bool _isVertical;

        /// <summary>
        /// Initializes a new instance of the <see cref="Marquee"/> class.
        /// </summary>
        public Marquee()
        {
            _syncedMarquees.ItemAdded += (sender, args) => { args.Item.Disposed += Component_Disposed; };
            _timer.Tick += (sender, args) => { CurrentStep += StepSize; };
            Components.Add(_label);
            Components.Add(_timer);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font Font
        {
            get { return _label.Font; }
            set { _label.Font = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is use cache.
        /// </summary>
        public bool IsUseCache
        {
            get { return _label.IsUseCache; }
            set { _label.IsUseCache = value; }
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public int Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is running.
        /// </summary>
        public bool IsRunning
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is vertical.
        /// </summary>
        public bool IsVertical
        {
            get { return _isVertical; }
            set { SwapProperty(ref _isVertical, value); }
        }

        /// <summary>
        ///     Gets or sets the number of pixels shifted each interval.
        /// </summary>
        public int StepSize
        {
            get { return _stepSize; }
            set
            {
                if (value <= 0)
                    throw new IndexOutOfRangeException("Marquee.StepSize must at least contain a value of 1 or higher.");

                SwapProperty(ref _stepSize, value);
            }
        }

        /// <summary>
        ///     Gets a collection of synced marquees.
        /// </summary>
        public WatchableCollection<Marquee> SyncedMarquees
        {
            get { return _syncedMarquees; }
        }

        /// <summary>
        ///     Gets or sets the marquee style.
        /// </summary>
        public MarqueeStyle MarqueeStyle { get; set; }

        /// <summary>
        ///     Gets or sets the number of steps for the marquee to stop at the preset moments.
        /// </summary>
        public int EndStepsCount
        {
            get { return _endStepsCount; }
            set { SwapProperty(ref _endStepsCount, value); }
        }

        /// <summary>
        ///     Gets the amount of steps it takes to rotate.
        /// </summary>
        public int Steps
        {
            get
            {
                switch (MarqueeStyle)
                {
                    case MarqueeStyle.Loop:
                        return EndStepsCount + Size.Width + _label.Size.Width*2;
                    case MarqueeStyle.Visibility:
                        return EndStepsCount*2 +
                               (_label.Size.Width - Size.Width > 0 ? _label.Size.Width - Size.Width : 0);
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current step.
        /// </summary>
        public int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                if (!SwapProperty(ref _currentStep, value, false, false)) return;
                int ms = _syncedMarquees.Select(m => m.Steps).Concat(new[] {Steps}).Max();

                _currentStep %= ms;
                while (_currentStep < 0) _currentStep += ms;
                int step = _currentStep >= Steps ? Steps - 1 : _currentStep;

                foreach (Marquee marquee in _syncedMarquees)
                    marquee.CurrentStep = Math.Min(_currentStep, marquee.Steps - 1);

                switch (MarqueeStyle)
                {
                    case MarqueeStyle.Loop:
                        if (IsVertical)
                        {
                            int y = Size.Width - step;
                            if (step > Size.Width) y += Math.Min(step - Size.Width, EndStepsCount);
                            _label.Location = new Location(0, y);
                        }
                        else
                        {
                            int x = Size.Width - step;
                            if (step > Size.Width) x += Math.Min(step - Size.Width, EndStepsCount);
                            _label.Location = new Location(x, 0);
                        }
                        break;
                    case MarqueeStyle.Visibility:
                        if (Steps == EndStepsCount*2)
                        {
                            _label.Location = new Location(0, 0);
                            break;
                        }
                        if (IsVertical)
                        {
                            int y = -step + Math.Min(step, EndStepsCount);
                            int h = _label.Size.Height - Size.Height > 0 ? _label.Size.Height - Size.Height : 0;
                            if (step > h + EndStepsCount) y += Math.Min(step - (h + EndStepsCount), EndStepsCount);
                            _label.Location = new Location(0, y);
                        }
                        else
                        {
                            int x = -step + Math.Min(step, EndStepsCount);
                            int w = _label.Size.Width - Size.Width > 0 ? _label.Size.Width - Size.Width : 0;
                            if (step > w + EndStepsCount) x += Math.Min(step - (w + EndStepsCount), EndStepsCount);
                            _label.Location = new Location(x, 0);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Clears all items from the Label's cache.
        /// </summary>
        public void ClearCache()
        {
            _label.ClearCache();
        }


        private void Component_Disposed(object sender, EventArgs e)
        {
            var m = sender as Marquee;
            m.Disposed -= Component_Disposed;
            _syncedMarquees.Remove(m);
        }
    }
}