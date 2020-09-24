using ExampleApp;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PerformanceHealthCheck.AspNetCore;
using PerformanceHealthCheck.Core;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    [Trait("", "Tests on perf endpoint")]
    public class PerfEndpointTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly WebApplicationFactory<Startup> _factory;

        public PerfEndpointTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "Test on one request")]
        public async Task one_request_test()
        {
            const int worktime = 1000;
            const string TestEndpoint = "/api/test";
            const string PerfEndpoint = "/perf";

            var client = _factory.CreateClient();

            (await client.GetAsync($"{TestEndpoint}?waittime={worktime}")).EnsureSuccessStatusCode();
            await Task.Delay(1000); // results from last second isn't included
            var response = await client.GetAsync(PerfEndpoint);
            var responseContent = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<IDictionary<string, TestWorkResult>>(responseContent, GetJsonOptions());

            result.Should().ContainKey(TestEndpoint);
            result[TestEndpoint].Count.Should().Be(1);
            result[TestEndpoint].AverageCallTime.TotalMilliseconds.Should().BeApproximately(worktime, 100);
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeSpanConverter());
            return options;
        }

    }

    internal class TimeSpanConverter : JsonConverter<TimeSpan>
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
    internal class TestWorkResult : IMeasurementResult
    {
        public TimeSpan AverageCallTime { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }
    }
}
