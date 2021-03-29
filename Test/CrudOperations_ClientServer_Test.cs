using Bunit;
using FluentAssertions;
using Xunit;
using BlazorWasmTesting.Test.Helpers;
using BlazorWasmTesting.Shared.Contracts;
using BlazorWasmTesting.Client.Pages;
using BlazorWasmTesting.Shared.Api;
using BlazorWasmTesting.Client.Clients;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;

namespace BlazorWasmTesting.Test
{
    public class CrudOperations_ClientServer_Test : ClientServerTestBase
    {
        public CrudOperations_ClientServer_Test()
        {
            _testContext.Services.AddSingleton<IPersonsApi, PersonsClient>();

            var dbContext = _webApplicatonFactory.DbContextFactory.CreateDbContext();
            dbContext.Persons.AddRange(
                    new Person(0, "FirstName1", "LastName1", null),
                    new Person(0, "FirstName2", "LastName2", "MiddleName2")
                );
            dbContext.SaveChanges();
        }

        [Fact]
        public void DataFetchFromServerUsingMockExternalAPI()
        {
            // Arrange

            // Act: render the razor component
            var cut = _testContext.RenderComponent<CrudOperations>();

            // Assert: Find the <body> element, then verify its has 2 children
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(2));
        }

        [Fact]
        public async Task Delete_CodeAct_UIAndCodeAssert()
        {
            // Arrange
            var cut = _testContext.RenderComponent<CrudOperations>();

            // Act: Call the code to delete a person
            await cut.Instance.DeletePerson(1);
            cut.Render();

            // Assert: Find the <body> element, then verify its has only 1 children after deletion
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(1));
            // Assert using the DB state to verify deletion
            var db = _webApplicatonFactory.DbContextFactory.CreateDbContext();
            db.Persons.Count().Should().Be(1);
            db.Persons.Any(p => p.Key == 1).Should().BeFalse();
        }


        [Fact]
        public async Task Delete_UIOnly()
        {
            // Arrange
            var db = _webApplicatonFactory.DbContextFactory.CreateDbContext();
            var cut = _testContext.RenderComponent<CrudOperations>();
            cut.WaitForState(() => cut.Instance.IsInitialized);
            
            // Act: Call the UI button to delete a person.
            cut.Find($"#persons_table__delete_button__{1}").Click();
            cut.WaitForState(() => db.Persons.Count() is 1);

            // Assert: Find the <body> element, then verify its has only 1 children after deletion
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(1));
            // Assert using the DB state to verify deletion
            db.Persons.Any(p => p.Key == 1).Should().BeFalse();
        }
    }
}
