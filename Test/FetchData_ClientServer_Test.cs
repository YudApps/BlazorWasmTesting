using BlazorWasmTesting.Client.Pages;
using Bunit;
using FluentAssertions;
using Xunit;
using RichardSzalay.MockHttp;
using BlazorWasmTesting.Shared;
using System;
using BlazorWasmTesting.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace BlazorWasmTesting.Test
{
    public class FetchData_ClientServer_Test
    {
        [Fact]
        public void ClientOnly_MockHttpClient_CounterShouldIncrementWhenClicked()
        {
            // Arrange
            using var ctx = new TestContext();
            
            // mock the http reqeusts
            var mock = ctx.Services.AddMockHttpClient();
            mock.When("/WeatherForecast").RespondJson<WeatherForecast[]>(
                new WeatherForecast[]
                {
                    new () { Date = DateTime.Now, TemperatureC = 10, Summary = "Summary1" },
                    new () { Date = DateTime.Now, TemperatureC = 20, Summary = "Summary2" },
                });

            // Act: render the Counter.razor component
            var cut = ctx.RenderComponent<FetchData>();

            // Assert: Find the <body> element, then verify its has 2 children
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(2));
        }

        [Fact]
        public void ClientServer_WebApplicationFactoryHttpClient_CounterShouldIncrementWhenClicked()
        {
            // Arrange
            using var webApplicatonFactory = new BlazorWasmTestingWebApplicationFactory();
            using var ctx = new TestContext();
            // Register the web application factory client
            ctx.Services.AddSingleton(webApplicatonFactory.CreateClient());

            // Configure the WeatherFetcherMock
            webApplicatonFactory.WeatherForecastFetcher.WeatherForecasts.AddRange(
                new WeatherForecast[]
                {
                    new () { Date = DateTime.Now, TemperatureC = 10, Summary = "Summary1" },
                    new () { Date = DateTime.Now, TemperatureC = 20, Summary = "Summary2" },
                    new () { Date = DateTime.Now, TemperatureC = 30, Summary = "Summary3" },
                });

            // Act: render the Counter.razor component
            var cut = ctx.RenderComponent<FetchData>();

            // Assert: Find the <body> element, then verify its has 3 children
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(3));
        }
    }
}
