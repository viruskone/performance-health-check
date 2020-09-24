using PerformanceHealthCheck.Core;
using PerformanceHealthCheck.InMemory;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceHealthCheck.Results
{
    public class ResultFactory : IMeasurementResultFactory
    {

        private readonly IPerfConfiguration _configuration;

        public ResultFactory(IPerfConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMeasurementResult Calculate(string name)
        {
            return new MeasurementResult(_configuration, name);
        }

        public IEnumerable<IMeasurementResult> CalculateAll()
            => StorageCollection.Keys.Select(key => Calculate(key)).ToList();
    }

}
