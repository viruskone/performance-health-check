using System;

namespace PerformanceHealthCheck.Core
{
    public interface IMeasurementResult
    {
        string Name { get; }
        int Count { get; }
        TimeSpan AverageCallTime { get; }
    }
}