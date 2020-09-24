using System;

namespace PerformanceHealthCheck.Core
{
    public interface IPerfConfiguration
    {
        TimeSpan ForTime { get; }
    }

}
