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
using System.Threading;

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a ticking timer.
    /// </summary>
    public class Timer : Component
    {
        private bool _enabled;
        private int _interval = 100;
        private Thread _thread;

        /// <summary>
        /// Gets or sets the interval in milliseconds between ticks.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Timer.Interval must at least contain a value of 1 or higher.</exception>
        public int Interval
        {
            get { return _interval; }
            set
            {
                if (value <= 0)
                    throw new IndexOutOfRangeException("Timer.Interval must at least contain a value of 1 or higher.");

                _interval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Timer"/> is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value)
                    return;

                _enabled = value;
                if (value && !IsDisposed && _thread == null)
                    (_thread = new Thread(() =>
                    {
                        while (!IsDisposed && Enabled && Interval > 0)
                        {
                            OnTick(EventArgs.Empty);

                            if (Interval < 2000)
                                Thread.Sleep(Interval);
                            else
                            {
                                int loop = Interval/2000;
                                int rest = Interval%2000;

                                for (int i = 0; i < loop; i++)
                                {
                                    if (IsDisposed || !Enabled || Interval <= 0)
                                        break;
                                    Thread.Sleep(2000);
                                }

                                if (!IsDisposed && Enabled && Interval > 0)
                                    Thread.Sleep(rest);
                            }
                        }
                        _thread = null;
                    }) {Name = "LogiFrame timer thread"}).Start();
            }
        }

        /// <summary>
        /// Occurs when Enabled and the set <see cref="Interval"/> has elapsed.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        ///     Called when the timer ticks.
        /// </summary>
        /// <param name="e">Contains information about the event.</param>
        public virtual void OnTick(EventArgs e)
        {
            if (Tick != null)
                Tick(this, e);
        }

        #region Overrides of Component

        /// <summary>
        /// Raises the <see cref="Changed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public override void OnChanged(EventArgs e)
        {
            // Prevent rendering
        }

        /// <summary>
        /// Renders this instance to a <see cref="Snapshot" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            // No visible elements
            return Snapshot.Empty;
        }

        /// <summary>
        ///     Performs tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            _enabled = false;
            base.Dispose(disposing);
        }

        #endregion
    }
}