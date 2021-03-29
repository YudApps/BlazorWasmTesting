using BlazorWasmTesting.Server.Db;
using BlazorWasmTesting.Server.ExternalApis;
using BlazorWasmTesting.Test.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BlazorWasmTesting.Test.Helpers
{
    public class BlazorWasmTestingWebApplicationFactory : WebApplicationFactory<Server.Startup>
    {
        public BlazorWasmTestingWebApplicationFactory() : this(new DbContextFactory()) {}

        public BlazorWasmTestingWebApplicationFactory(DbContextFactory dbContextFactory)
        {
            DbContextFactory = dbContextFactory;
            WeatherForecastFetcher = new WeatherForecastFetcherMock();
        }

        public DbContextFactory DbContextFactory { get; }
        public WeatherForecastFetcherMock WeatherForecastFetcher { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(
                    d => d.ServiceType == typeof(IWeatherForecastFetcher)));
                services.AddSingleton<IWeatherForecastFetcher>(WeatherForecastFetcher);

                services.Remove(services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BlazorWasmTestingDbContext>)));

                services.AddDbContext<BlazorWasmTestingDbContext>(options =>
                {
                    options.UseSqlite(DbContextFactory.Connection);
                });
            });
        }
    }
}
