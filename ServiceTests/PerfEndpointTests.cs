using ExampleApp;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PerformanceHealthCheck.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

            (await client.PostAsync(TestEndpoint,
                        new StringContent(JsonSerializer.Serialize(new WorkModel { WaitTime = worktime }), Encoding.UTF8, "application/json")))
                        .EnsureSuccessStatusCode();
            await Task.Delay(1000); // results from last second isn't included
            var response = await client.GetAsync(PerfEndpoint);
            var responseContent = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<IDictionary<string, TestWorkResult>>(responseContent, GetJsonOptions());

            result.Should().ContainKey("POST test workone");
            result["POST test workone"].Count.Should().Be(1);
            result["POST test workone"].AverageCallTime.TotalMilliseconds.Should().BeApproximately(worktime, 100);
        }

        [Fact(DisplayName = "Test on couple request")]
        public async Task couple_request_test()
        {
            const int worktime = 100;
            const string TestEndpoint = "/api/test";
            const string PerfEndpoint = "/perf";

            const int tasks = 10;
            const int times = 1000;

            var threads = Enumerable.Range(0, tasks).Select(x => Task.Run(async () =>
            {
                var client = _factory.CreateClient();

                for (int i = 0; i < times; i++)
                {
                    (await client.PostAsync(TestEndpoint,
                        new StringContent(JsonSerializer.Serialize(new WorkModel { WaitTime = worktime }), Encoding.UTF8, "application/json")))
                        .EnsureSuccessStatusCode();
                }
            })).ToArray();
            Task.WaitAll(threads);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

            await Task.Delay(1000); // results from last second isn't included

            var client = _factory.CreateClient();
            var response = await client.GetAsync(PerfEndpoint);
            var responseContent = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<IDictionary<string, TestWorkResult>>(responseContent, GetJsonOptions());

            result.Should().ContainKey("POST test workone");
            result["POST test workone"].Count.Should().Be(tasks * times);
            result["POST test workone"].AverageCallTime.TotalMilliseconds.Should().BeApproximately(worktime, 10);
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeSpanConverter());
            return options;
        }

    }
}
