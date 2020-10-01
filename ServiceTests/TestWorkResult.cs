using PerformanceHealthCheck.Core;
using System;

namespace ServiceTests
{
    internal class TestWorkResult : IMeasurementResult
    {
        public TimeSpan AverageCallTime { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }
    }
}
