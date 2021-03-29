using BlazorWasmTesting.Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Server.ExternalApis
{
    public interface IWeatherForecastFetcher
    {
        Task<IEnumerable<WeatherForecast>> Get();
    }
}
