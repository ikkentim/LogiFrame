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
using System.Threading;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a ticking timer.
    /// </summary>
    public class Timer : Component
    {
        #region Fields

        private bool _enabled;
        private int _interval = 100;
        private Thread _thread;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Invetval of the current LogiFrame.Components.Timer elapsed whilst running.
        /// </summary>
        public event EventHandler Tick;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the time in miliseconds each frame lasts.
        /// </summary>
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
        /// Gets or sets whether the timer is enabled.
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
                                var loop = Interval/2000;
                                var rest = Interval%2000;

                                for (var i = 0; i < loop; i++)
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

        #endregion

        #region Methods

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        /// <param name="e">Contains information about the event.</param>
        public virtual void OnTick(EventArgs e)
        {
            if (Tick != null)
                Tick(this, e);
        }

        public override void OnChanged(EventArgs e)
        {
            //Prevent rendering
        }

        protected override Bytemap Render()
        {
            //No visible elements
            return null;
        }

        protected override void DisposeComponent()
        {
            _enabled = false;
        }

        #endregion
    }
}