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

        public virtual bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled == value) return;

                _enabled = value;

                if (value)
                {
                    _cancellationTokenSource =new CancellationTokenSource();
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
                if(value < 1) throw new ArgumentOutOfRangeException(nameof(value), "interval must be at least 1");
                _interval = value;
            }
        }

        public object Tag { get; set; }

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