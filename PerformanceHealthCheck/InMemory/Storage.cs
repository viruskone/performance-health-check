using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceHealthCheck.InMemory
{
    internal class Storage
    {

        private readonly ConcurrentDictionary<int, Counter> _counters = new ConcurrentDictionary<int, Counter>();

        public Storage()
        {
            Monitor.Register(this);
        }

        internal void Add(TimeSpan elapsed)
        {
            var counter = _counters.GetOrAdd(GetTimeIndex(), _ => new Counter());
            counter.Increment(elapsed);
        }

        private int GetTimeIndex()
        {
            try
            {
                return TimeIndex.GetIndex(DateTime.Now);
            }
            catch (TimeIndexOverflowException)
            {
                _counters.Clear();
                TimeIndex.Reset();
                return TimeIndex.GetIndex(DateTime.Now);
            }
        }

        internal void ClearCounters(Func<int, bool> timeIndexPredicate)
        {
            var keysToRemove = _counters.Keys.Where(timeIndexPredicate);
            foreach (var keyToRemove in keysToRemove)
            {
                _counters.Remove(keyToRemove, out var _);
            }
        }

        internal IReadOnlyDictionary<int, Counter> GetCounters() => _counters;
    }

}
