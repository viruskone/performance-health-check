using PerformanceHealthCheck.Core;
using PerformanceHealthCheck.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceHealthCheck.Results
{
    internal class Calculator
    {

        private readonly IReadOnlyDictionary<int, Counter> _counters;

        public Calculator(IReadOnlyDictionary<int, Counter> counters)
        {
            _counters = counters;
        }

        internal CalculatorResult Calculate(TimeSpan forTime)
        {
            var minIdx = TimeIndex.GetIndex(DateTime.Now - forTime);
            var maxIdx = TimeIndex.GetIndex(DateTime.Now - TimeSpan.FromSeconds(1));
            var splitCounters = _counters.Where(kv => kv.Key >= minIdx && kv.Key <= maxIdx).Select(kv => kv.Value).ToList();

            if (splitCounters.Any() == false) return new CalculatorResult { Count = 0, AverageTime = TimeSpan.MinValue };

            var totalCount = splitCounters.Sum(c => c.Count);
            var totalTime = splitCounters.Sum(c => c.TotalMilliseconds);

            return new CalculatorResult
            {
                Count = totalCount,
                AverageTime = TimeSpan.FromMilliseconds(totalTime / totalCount)
            };

        }
    }

}
