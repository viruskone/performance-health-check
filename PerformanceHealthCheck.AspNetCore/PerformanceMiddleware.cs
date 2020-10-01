using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
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
            var unit = factory.Get(GetName(context));
            unit.Start();
            await next(context);
            unit.Stop();
        }

        private string GetName(HttpContext context)
        {
            var actionDescription = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
            var method = context.Request.Method;
            if (method.Length < 4) method += new string(' ', 4 - method.Length);
            var description = actionDescription != null ? $"{actionDescription.ControllerName}.{actionDescription.ActionName}" : context.Request.Path.ToString();
            return $"{method} {description.ToLower()}";
        }

    }
}
