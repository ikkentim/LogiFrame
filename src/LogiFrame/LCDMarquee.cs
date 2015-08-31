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

using System.Drawing;

namespace LogiFrame
{
    /// <summary>
    /// Represents a marquee.
    /// </summary>
    public class LCDMarquee : LCDControl
    {
        private readonly LCDLabel _label;
        private readonly Timer _timer;
        private int _breakSteps;
        private int _maxSteps;
        private int _steps;

        /// <summary>
        /// Initializes a new instance of the <see cref="LCDMarquee"/> class.
        /// </summary>
        public LCDMarquee()
        {
            _label = new LCDLabel { AutoSize = true };
            _label.AssignParent(this);

            _timer = new Timer { Enabled = true };
            _timer.Tick += (sender, args) => PerformStep();
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font Font
        {
            get { return _label.Font; }
            set
            {
                _label.Font = value;
                CalculateSteps();
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
                CalculateSteps();
            }
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
        /// Gets or sets a value indicating whether this <see cref="LCDMarquee"/> is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets the size of the step.
        /// </summary>
        public int StepSize { get; set; } = 1;
        /// <summary>
        /// Gets or sets a value indicating whether to break at the break at end of the text.
        /// </summary>
        public bool BreakAtEnd { get; set; } = true;
        /// <summary>
        /// Gets or sets the break steps.
        /// </summary>
        public int BreakSteps { get; set; } = 5;

        private void CalculateSteps()
        {
            var scroll = _label.Width - Width;
            if (scroll <= 0)
            {
                _maxSteps = 0;
                Reset();
            }

            _maxSteps = scroll;
        }

        /// <summary>
        /// Performs a step.
        /// </summary>
        public void PerformStep()
        {
            ThrowIfDisposed();

            if (_maxSteps == 0) return;

            if (_breakSteps > 0)
            {
                _breakSteps--;

                if (_breakSteps == 0)
                {
                    if (_steps == _maxSteps)
                    {
                        _breakSteps = BreakSteps;
                        Reset();
                    }
                }
                return;
            }

            _steps += StepSize;

            if (_steps > _maxSteps)
            {
                _steps = _maxSteps;
                _breakSteps = BreakSteps;
            }

            Invalidate();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            ThrowIfDisposed();

            _steps = 0;
            Invalidate();
        }

        #region Overrides of LCDControl

        /// <summary>
        ///     Gets or sets the size of the control.
        /// </summary>
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                CalculateSteps();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            _label.PerformLayout();
            e.Bitmap.MergeOverride(_label.Bitmap, new Point(-_steps, 0));
            base.OnPaint(e);
        }

        #endregion
    }
}