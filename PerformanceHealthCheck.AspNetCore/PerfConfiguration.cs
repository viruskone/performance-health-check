using Microsoft.Extensions.Options;
using PerformanceHealthCheck.Core;
using System;

namespace PerformanceHealthCheck.AspNetCore
{
    internal class PerfConfiguration : IPerfConfiguration
    {
        private readonly IOptionsSnapshot<PerfConfigurationOptions> _optionsAccessor;
        
        public TimeSpan ForTime => TimeSpan.FromSeconds(_optionsAccessor.Value.MeansurePeriodInSeconds);

        public PerfConfiguration(IOptionsSnapshot<PerfConfigurationOptions> options)
        {
            _optionsAccessor = options;
        }

    }

    internal class PerfConfigurationOptions
    {
        internal const string Name = "Performance";

        public int MeansurePeriodInSeconds { get; set; }
    }

}
