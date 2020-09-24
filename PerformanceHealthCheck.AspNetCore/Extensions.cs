using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerformanceHealthCheck.Core;
using PerformanceHealthCheck.InMemory;
using PerformanceHealthCheck.Results;

namespace PerformanceHealthCheck.AspNetCore
{
    public static class Extensions
    {

        public static IServiceCollection AddPerformanceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PerfConfigurationOptions>(configuration.GetSection(PerfConfigurationOptions.Name));

            services.AddSingleton<IUnitMeasurementFactory, UnitMeasurementFactory>();
            services.AddScoped<IMeasurementResultFactory, ResultFactory>();
            services.AddScoped<IPerfConfiguration, PerfConfiguration>();

            services.AddSingleton<PerformanceMiddleware>();
            services.AddScoped<PerformanceResponseWriter>();
            return services;
        }

        public static IApplicationBuilder UsePerformanceHealthChecks(this IApplicationBuilder app)
        {
            app.UseMiddleware<PerformanceMiddleware>();
            return app;
        }

        public static IEndpointConventionBuilder MapPerformanceEndpoint(this IEndpointRouteBuilder endpoints, string pattern)
        =>
            endpoints.MapGet(pattern, async c =>
            {
                var writer = c.RequestServices.GetRequiredService<PerformanceResponseWriter>();
                await writer.WriteAsync(c.Response);
            });
    }

}
