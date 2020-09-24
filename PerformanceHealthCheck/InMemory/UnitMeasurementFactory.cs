using PerformanceHealthCheck.Core;

namespace PerformanceHealthCheck.InMemory
{
    public class UnitMeasurementFactory : IUnitMeasurementFactory
    {

        public IUnitMeasurement Get(string name)
            => new UnitMeasurement(name);
    }
}
