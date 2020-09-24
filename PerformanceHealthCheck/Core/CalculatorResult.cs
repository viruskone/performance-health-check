using System;

namespace PerformanceHealthCheck.Core
{
    public struct CalculatorResult
    {
        public int Count { get; set; }
        public TimeSpan AverageTime { get; set; }
    }

}
