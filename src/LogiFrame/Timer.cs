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
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        protected virtual void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}