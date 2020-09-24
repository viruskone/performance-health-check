using System;
using System.Collections.Generic;

namespace PerformanceHealthCheck.InMemory
{
    public static class Monitor
    {

        private static List<Storage> storages = new List<Storage>();

        internal static void Register(Storage perfStorage)
        {
            storages.Add(perfStorage);
        }

        public static void CleanUp(TimeSpan olderThan)
        {

            var cleanUpTimeIndex = TimeIndex.GetIndex(DateTime.Now - olderThan);

            foreach (var storage in storages)
            {
                storage.ClearCounters(i => i < cleanUpTimeIndex);
            }
        }

    }

}