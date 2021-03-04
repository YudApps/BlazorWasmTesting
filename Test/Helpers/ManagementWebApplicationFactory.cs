using BlazorWasmTesting.Server.ExternalApis;
using BlazorWasmTesting.Test.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BlazorWasmTesting.Test.Helpers
{
    class BlazorWasmTestingWebApplicationFactory : WebApplicationFactory<Server.Startup>
    {
        public WeatherForecastFetcherMock WeatherForecastFetcher { get; } = new WeatherForecastFetcherMock();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(
                    d => d.ServiceType == typeof(IWeatherForecastFetcher)));
                services.AddSingleton<IWeatherForecastFetcher>(WeatherForecastFetcher);
            });
        }
    }
}
