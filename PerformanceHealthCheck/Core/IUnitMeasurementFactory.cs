namespace PerformanceHealthCheck.Core
{
    public interface IUnitMeasurementFactory
    {
        IUnitMeasurement Get(string name);
    }

}
