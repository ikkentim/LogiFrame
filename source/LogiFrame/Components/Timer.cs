// Animation.cs
// 
// LogiFrame rendering library.
// Copyright (C) 2013 Tim Potze
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

        private int _interval;
        private bool _run;
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
                _interval = value;
                CheckThreadRunning();
            }
        }

        /// <summary>
        /// Gets or sets whether the timer should cycle.
        /// </summary>
        public bool Run
        {
            get { return _run; }
            set
            {
                if (_run == value)
                    return;

                _run = value;

                CheckThreadRunning();
            }
        }

        #endregion

        #region Methods

        protected override Bytemap Render()
        {
            //No visible 
            return null;
        }

        protected override void DisposeComponent()
        {
            _run = false;
        }

        /// <summary>
        /// Checks whether the thread is still running and restarts it if necessary.
        /// </summary>
        private void CheckThreadRunning()
        {
            //Check if the thread is running
            if (!Disposed && Run && _thread == null)
            {
                _thread = new Thread(AnimationThread);
                _thread.Start();
            }
        }

        /// <summary>
        /// Triggers the tick every so often
        /// </summary>
        private void AnimationThread()
        {
            while (!Disposed && Run && Interval > 0)
            {
                if (Tick != null)
                    Tick(this, EventArgs.Empty);
                Thread.Sleep(Interval);
            }
            _thread = null;
        }

        #endregion
    }
}