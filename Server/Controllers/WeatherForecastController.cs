using BlazorWasmTesting.Server.ExternalApis;
using BlazorWasmTesting.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastFetcher _weatherForecastFetcher;

        public WeatherForecastController(IWeatherForecastFetcher weatherForecastFetcher, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _weatherForecastFetcher = weatherForecastFetcher;
        }

        [HttpGet]
        public Task<IEnumerable<WeatherForecast>> Get()
        {
            return _weatherForecastFetcher.Get();
        }
    }
}
