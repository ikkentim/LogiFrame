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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LogiFrame
{
    public class Timer
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _enabled;
        private int _interval = 100;

        public virtual bool Enabled
        {
            get { return _enabled; }
            set
            {
                ThrowIfDisposed();
                if (_enabled == value) return;

                _enabled = value;

                if (value)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    TickLoop(_cancellationTokenSource.Token);
                }
                else
                {
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        public int Interval
        {
            get { return _interval; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(value), "interval must be at least 1");
                _interval = value;
            }
        }

        public object Tag { get; set; }

        private async void TickLoop(CancellationToken cancellationToken)
        {
            var delay = Interval;
            var sw = new Stopwatch();
            while (_enabled)
            {
                if (delay > 0)
                {
                    var task = Task.Delay(delay, cancellationToken);

                    try
                    {
                        await task;
                    }

                    catch (TaskCanceledException)
                    {
                        return;
                    }
                }

                sw.Start();
                OnTick();
                sw.Stop();
                delay = Interval - (int) sw.ElapsedMilliseconds;
                sw.Reset();
            }
        }

        public event EventHandler Tick;

        public void Start()
        {
            ThrowIfDisposed();
            Enabled = true;
        }
        
        public void Stop()
        {
            ThrowIfDisposed();
            Enabled = false;
        }

        protected virtual void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }



        #region Implementation of IDisposable

        /// <summary>
        /// Releases all resources used by the <see cref="T:LogiFrame.Timer"/>.
        /// </summary>
        public void Dispose()
        {
            ThrowIfDisposed();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~Timer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:LogiFrame.Timer"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            lock (this)
            {
                Disposing = true;
                Stop();
                Disposed?.Invoke(this, EventArgs.Empty);
                Disposing = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the control has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Gets a value indicating whether the control has been disposed of.
        /// </summary>
        /// 
        /// <returns>
        /// true if the control has been disposed of; otherwise, false.
        /// </returns>
        public bool IsDisposed { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the base <see cref="T:LogiFrame.Timer"/> class is in the process of disposing.
        /// </summary>
        /// 
        /// <returns>
        /// true if the base <see cref="T:LogiFrame.Timer"/> class is in the process of disposing; otherwise, false.
        /// </returns>
        public bool Disposing { get; private set; }

        /// <summary>
        /// Occurs when the component is disposed by a call to the <see cref="M:LogiFrame.Timer.Dispose"/> method.
        /// </summary>
        public event EventHandler Disposed;
    }
}