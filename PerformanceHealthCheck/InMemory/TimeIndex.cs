using System;

namespace PerformanceHealthCheck.InMemory
{
    public static class TimeIndex
    {
        private const int MaxLeadIndex = 2147;

        private static DateTime maxDate;
        private static int maxLeadIndex = 0;

        private static DateTime[] storedLeadIndex = new DateTime[MaxLeadIndex];

        private static object Locker = new object();

        public static int GetIndex(DateTime dateTime)
        {
            var date = dateTime.Date;
            IncrementLeadIndexIfIsNextDay(date);
            ThrowErrorIfLeadIndexOverflow();
            return GetLeadIndexForDate(date) * 1000000 + dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second;
        }

        public static void Reset()
        {
            maxDate = DateTime.MinValue;
            maxLeadIndex = 0;
            storedLeadIndex = new DateTime[MaxLeadIndex];
        }

        private static void IncrementLeadIndexIfIsNextDay(DateTime date)
        {
            if (DateIsChanged(date))
            {
                lock (Locker)
                {
                    if (DateIsChanged(date) && IsNextDay(date))
                    {
                        maxDate = date;
                        maxLeadIndex++;
                        storedLeadIndex[maxLeadIndex] = date;
                    }

                }
            }
        }

        private static bool IsNextDay(DateTime date)
            => date > maxDate;

        private static bool DateIsChanged(DateTime date)
            => date != maxDate;


        private static void ThrowErrorIfLeadIndexOverflow()
        {
            if (maxLeadIndex > MaxLeadIndex)
                throw new TimeIndexOverflowException();
        }

        private static int GetLeadIndexForDate(DateTime date)
            => Array.IndexOf(storedLeadIndex, date);


    }

}
