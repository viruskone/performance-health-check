using PerformanceHealthCheck.Core;
using System.Diagnostics;

namespace PerformanceHealthCheck.InMemory
{

    internal class UnitMeasurement : IUnitMeasurement
    {
        private readonly Storage _storage;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public UnitMeasurement(string name)
        {
            _storage = StorageCollection.GetOrCreate(name);
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _storage.Add(_stopwatch.Elapsed);
        }
    }

}
