using FluentAssertions;
using PerformanceHealthCheck.InMemory;
using System;
using Xunit;

namespace UnitTests
{

    [Trait("", "Time index tests")]
    public class TimeIndexTests
    {

        [Fact(DisplayName = "Check order behaviour")]
        public void time_index_order_test()
        {
            var yearTotalSeconds = TimeSpan.FromDays(366).TotalSeconds;
            var refDate = DateTime.Now.AddDays(30);
            var prevValue = 0;
            for (int i = 0; i < yearTotalSeconds; i++)
            {
                var index = TimeIndex.GetIndex(refDate.AddSeconds(i));
                index.Should().BeGreaterThan(prevValue);
                prevValue = index;
            }
        }

        [Fact(DisplayName = "Check correct ask for index from past time")]
        public void test_time_ask_past_index()
        {
            var date1 = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            var date2 = DateTime.Now.AddDays(1);

            var index = TimeIndex.GetIndex(date1);
            TimeIndex.GetIndex(date2);
            var indexToCompare = TimeIndex.GetIndex(date1);

            indexToCompare.Should().Be(index);
        }

    }
}
