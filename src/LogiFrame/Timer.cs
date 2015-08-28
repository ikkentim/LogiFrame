using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogiFrame
{
    public class Timer : FrameControl
    {
        private async Task RepeatActionEvery(Action action, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                action();
                var task = Task.Delay(interval, cancellationToken);

                try
                {
                    await task;
                }

                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}