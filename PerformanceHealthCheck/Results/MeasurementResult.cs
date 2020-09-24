using PerformanceHealthCheck.Core;
using PerformanceHealthCheck.InMemory;
using System;

namespace PerformanceHealthCheck.Results
{
    internal class MeasurementResult : IMeasurementResult
    {
        public string Name { get; }

        public int Count { get; }

        public TimeSpan AverageCallTime { get; }

        internal MeasurementResult(IPerfConfiguration configuration, string name)
        {
            CalculatorResult calcResult = Calculate(StorageCollection.GetOrCreate(name), configuration.ForTime);
            Name = name;
            Count = calcResult.Count;
            AverageCallTime = calcResult.AverageTime;
        }

        private CalculatorResult Calculate(Storage storage, TimeSpan forTime)
        {
            Calculator calculator = new Calculator(storage.GetCounters());
            return calculator.Calculate(forTime);
        }
    }
}