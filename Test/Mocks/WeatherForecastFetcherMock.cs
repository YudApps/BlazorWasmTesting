using BlazorWasmTesting.Server.ExternalApis;
using BlazorWasmTesting.Shared;
using BlazorWasmTesting.Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Test.Mocks
{
    public class WeatherForecastFetcherMock : IWeatherForecastFetcher
    {
        public List<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();

        public Task<IEnumerable<WeatherForecast>> Get()
        {
            return Task.FromResult<IEnumerable<WeatherForecast>>(WeatherForecasts);
        }
    }
}
