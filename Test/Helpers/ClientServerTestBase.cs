using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorWasmTesting.Test.Helpers
{
    public class TestContextTestBase : IDisposable
    {
        protected TestContext _testContext;

        public TestContextTestBase()
        {
            _testContext = new TestContext();
        }

        public virtual void Dispose()
        {
            _testContext.Dispose();
        }
    }

    public class ClientServerTestBase: TestContextTestBase
    {
        protected BlazorWasmTestingWebApplicationFactory _webApplicatonFactory;

        public ClientServerTestBase()
        {
            _webApplicatonFactory = new BlazorWasmTestingWebApplicationFactory();
            // Register the web application factory client
            _testContext.Services.AddSingleton(_webApplicatonFactory.CreateClient());
        }

        public override void Dispose()
        {
            _webApplicatonFactory.Dispose();
            base.Dispose();
        }
    }
}
