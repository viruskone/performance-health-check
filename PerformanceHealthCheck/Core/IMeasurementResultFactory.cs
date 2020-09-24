using System.Collections.Generic;

namespace PerformanceHealthCheck.Core
{
    public interface IMeasurementResultFactory
    {
        IMeasurementResult Calculate(string name);
        IEnumerable<IMeasurementResult> CalculateAll();
    }
}