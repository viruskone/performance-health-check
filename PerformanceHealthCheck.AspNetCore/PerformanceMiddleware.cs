using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PerformanceHealthCheck.Core;
using System.Threading.Tasks;

namespace PerformanceHealthCheck.AspNetCore
{
    internal class PerformanceMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var factory = context.RequestServices.GetRequiredService<IUnitMeasurementFactory>();
            var unit = factory.Get(context.Request.Path);
            unit.Start();
            await next(context);
            unit.Stop();
        }
    }
}
