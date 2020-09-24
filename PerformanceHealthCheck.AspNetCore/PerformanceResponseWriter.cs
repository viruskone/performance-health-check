using Microsoft.AspNetCore.Http;
using PerformanceHealthCheck.Core;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PerformanceHealthCheck.AspNetCore
{
    internal class PerformanceResponseWriter
    {
        private readonly IMeasurementResultFactory _factory;

        public PerformanceResponseWriter(IMeasurementResultFactory factory)
        {
            _factory = factory;
        }

        internal async Task WriteAsync(HttpResponse response)
        {
            var result = _factory
                .CalculateAll()
                .Where(r => r.Count > 0)
                .OrderBy(r => r.Name)
                .ToDictionary(r => r.Name);

            response.ContentType = "application/json";
            response.StatusCode = 200;
            await JsonSerializer.SerializeAsync(response.Body, result, GetResultOptions());
            await response.CompleteAsync();
        }

        private JsonSerializerOptions GetResultOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeSpanConverter());
            return options;
        }

        private class TimeSpanConverter : JsonConverter<TimeSpan>
        {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return TimeSpan.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

    }

}
