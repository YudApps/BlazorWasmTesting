using Bunit;
using FluentAssertions;
using Xunit;
using BlazorWasmTesting.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using BlazorWasmTesting.Shared.Contracts;
using BlazorWasmTesting.Shared.Api;
using BlazorWasmTesting.Test.Mocks;
using BlazorWasmTesting.Client.Pages;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Test
{
    public class CrudOperations_ClientOnly_Test : TestContextTestBase
    {
        [Fact]
        public void DataFetchFromApiMock()
        {
            // Arrange
            PersonsApiMock personsApiMock = new PersonsApiMock();
            _testContext.Services.AddSingleton<IPersonsApi>(personsApiMock);
            personsApiMock.Persons[1] = new Person(1, "FirstName1", "LastName1", null);
            personsApiMock.Persons[2] = new Person(2, "FirstName2", "LastName2", "MiddleName2");

            // Act: render the razor component
            var cut = _testContext.RenderComponent<CrudOperations>();

            // Assert: Find the <body> element, then verify its has 2 children
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(2));
        }

        [Fact]
        public async Task Delete_CodeOnly()
        {
            // Arrange
            PersonsApiMock personsApiMock = new PersonsApiMock();
            _testContext.Services.AddSingleton<IPersonsApi>(personsApiMock);
            personsApiMock.Persons[1] = new Person(1, "FirstName1", "LastName1", null);
            personsApiMock.Persons[2] = new Person(2, "FirstName2", "LastName2", "MiddleName2");

            var cut = _testContext.RenderComponent<CrudOperations>();

            // Act: Call the code to delete a person
            await cut.Instance.DeletePerson(1);

            // Assert
            personsApiMock.Persons.Count.Should().Be(1);
            personsApiMock.Persons.Any(p => p.Key == 1).Should().BeFalse();
        }

        [Fact]
        public async Task Delete_CodeAct_UIAssert()
        {
            // Arrange
            PersonsApiMock personsApiMock = new PersonsApiMock();
            _testContext.Services.AddSingleton<IPersonsApi>(personsApiMock);
            personsApiMock.Persons[1] = new Person(1, "FirstName1", "LastName1", null);
            personsApiMock.Persons[2] = new Person(2, "FirstName2", "LastName2", "MiddleName2");

            var cut = _testContext.RenderComponent<CrudOperations>();

            // Act: Call the code to delete a person
            await cut.Instance.DeletePerson(1);
            cut.Render();

            // Assert: Find the <body> element, then verify its has only 1 children after deletion
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(1));
        }

        [Fact]
        public void Delete_UIOnly()
        {
            // Arrange
            PersonsApiMock personsApiMock = new PersonsApiMock();
            _testContext.Services.AddSingleton<IPersonsApi>(personsApiMock);
            personsApiMock.Persons[1] = new Person(1, "FirstName1", "LastName1", null);
            personsApiMock.Persons[2] = new Person(2, "FirstName2", "LastName2", "MiddleName2");

            var cut = _testContext.RenderComponent<CrudOperations>();
            cut.WaitForState(() => cut.Instance.IsInitialized);

            // Act: Call the UI button to delete a person.
            cut.Find($"#persons_table__delete_button__{1}").Click();
            cut.WaitForState(() => personsApiMock.Persons.Count is 1);

            // Assert: Find the <body> element, then verify its has only 1 children after deletion
            cut.WaitForAssertion(() => cut.Find("tbody").ChildElementCount.Should().Be(1));
            // Assert using the mock ApiClient to verify deletion
            personsApiMock.Persons.Any(p => p.Key == 1).Should().BeFalse();
        }
    }
}
