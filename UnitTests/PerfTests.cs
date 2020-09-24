using FluentAssertions;
using Moq;
using PerformanceHealthCheck.Core;
using PerformanceHealthCheck.InMemory;
using PerformanceHealthCheck.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{

    [Trait("", "Counting tests")]
    public class PerfTests
    {
        [Fact(DisplayName ="Build counting collection")]
        public void counting_test()
        {
            const int tasks = 10;
            const int times = 1000;

            var config = new Mock<IPerfConfiguration>();
            config.SetupGet(c => c.ForTime).Returns(TimeSpan.FromDays(1));

            var factory = new UnitMeasurementFactory();
            var resultFactory = new ResultFactory(config.Object);

            var threads = Enumerable.Range(0, tasks).Select(x => Task.Run(async () =>
            {
                for (int i = 0; i < times; i++)
                {
                    var unit = factory.Get(nameof(counting_test));
                    unit.Start();
                    await Task.Delay(10);
                    unit.Stop();
                }
            })).ToArray();
            Task.WaitAll(threads);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

            var result = resultFactory.Calculate(nameof(counting_test));
            result.Count.Should().Be(times * tasks);
        }

        [Fact(DisplayName ="Check counting returns only 'forTime' defined result")]
        public void counting_only_last_time_test()
        {
            var config = new Mock<IPerfConfiguration>();
            config.SetupGet(c => c.ForTime).Returns(TimeSpan.FromSeconds(1));

            var factory = new UnitMeasurementFactory();
            var resultFactory = new ResultFactory(config.Object);

            var unit = factory.Get(nameof(counting_only_last_time_test));
            unit.Start();
            unit.Stop();

            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

            var unit2 = factory.Get(nameof(counting_only_last_time_test));
            unit2.Start();
            unit2.Stop();

            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

            var result = resultFactory.Calculate(nameof(counting_only_last_time_test));
            result.Count.Should().Be(1);
        }

    }
}
