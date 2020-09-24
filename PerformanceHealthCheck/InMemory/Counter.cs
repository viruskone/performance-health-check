using System;
using System.Threading;

namespace PerformanceHealthCheck.InMemory
{
    internal class Counter
    {

        private volatile int invokeTimes;
        private volatile int totalTime;

        internal int Count => invokeTimes;
        internal int TotalMilliseconds => totalTime;

        internal void Increment(TimeSpan elapsed)
        {
            Interlocked.Add(ref totalTime, (int)elapsed.TotalMilliseconds);
            Interlocked.Increment(ref invokeTimes);
        }
    }

}
