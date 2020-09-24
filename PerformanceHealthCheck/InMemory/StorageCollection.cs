using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PerformanceHealthCheck.InMemory
{
    internal static class StorageCollection
    {
        private static ConcurrentDictionary<string, Storage> Storage = new ConcurrentDictionary<string, Storage>();

        internal static ICollection<string> Keys => Storage.Keys;

        internal static Storage GetOrCreate(string name)
            => Storage.GetOrAdd(name, x => new Storage());
    }

}
