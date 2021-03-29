using BlazorWasmTesting.Client.Clients;
using BlazorWasmTesting.Server.Controllers;
using BlazorWasmTesting.Shared.Api;
using BlazorWasmTesting.Shared.Contracts;
using BlazorWasmTesting.Test.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorWasmTesting.Test
{
    public class PersonsController_ServerOnly_Test : PersonsApiControllerTest
    {
        public PersonsController_ServerOnly_Test()
        {
            _personsApi = new PersonsController(_dbContextFactory.CreateDbContext(), new NullLogger<PersonsController>());
        }
    }

    public class PersonsClient_ClientServer_Test : PersonsApiControllerTest, IDisposable
    {
        protected BlazorWasmTestingWebApplicationFactory _factory;

        public PersonsClient_ClientServer_Test()
        {
            _factory = new BlazorWasmTestingWebApplicationFactory(_dbContextFactory);

            _personsApi = new PersonsClient(_factory.CreateClient());
        }

        public override void Dispose()
        {
            _factory.Dispose();
            base.Dispose();
        }
    }


    public abstract class PersonsApiControllerTest: IDisposable
    {
        protected DbContextFactory _dbContextFactory;
        protected IPersonsApi _personsApi;

        public PersonsApiControllerTest()
        {
            _dbContextFactory = new DbContextFactory();
        }

        public virtual void Dispose()
        {
            _dbContextFactory.Dispose();
        }

        [Fact]
        public async Task Get_NoData()
        {
            // Arrange


            // Act
            var persons = await _personsApi.Get();

            // Assert
            persons.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ArrangeDB_SomeData()
        {
            // Arrange: put data directly to the Db.
            var db = _dbContextFactory.CreateDbContext();
            db.Persons.AddRange(
                new Person(0, "FirstName1", "LastName1", null),
                new Person(0, "FirstName2", "LastName2", "MiddleName2"));
            db.SaveChanges();

            // Act
            var persons = await _personsApi.Get();

            // Assert
            persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task Post_AssertDb()
        {
            // Act
            await _personsApi.Post(new Person(0, "FirstName1", "LastName1", null));
            await _personsApi.Post(new Person(0, "FirstName2", "LastName2", "MiddleName2"));

            // Assert: verify data was added to the db.
            var db = _dbContextFactory.CreateDbContext();
            db.Persons.Should().HaveCount(2);
        }

        [Fact]
        public void Post_VerifyDbRequiredAttribute()
        {
            // Act
            Func<Task> put = () => _personsApi.Post(new Person(0, "FirstName1", null /*Required*/, null));

            // Assert
            put.Should().Throw<Exception>();
        }

        [Fact]
        public async Task PostAndGet_ApiOnlyTesting()
        {
            // Act
            await _personsApi.Post(new Person(0, "FirstName1", "LastName1", null));
            await _personsApi.Post(new Person(0, "FirstName2", "LastName2", "MiddleName2"));
            var persons = await _personsApi.Get();

            // Assert
            persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task PostAndDelete_ApiOnlyTesting()
        {
            // Act
            await _personsApi.Post(new Person(0, "FirstName1", "LastName1", null));
            await _personsApi.Post(new Person(0, "FirstName2", "LastName2", "MiddleName2"));
            await _personsApi.Post(new Person(0, "FirstName3", "LastName3", null));

            await _personsApi.Delete(2);

            // Assert
            var persons = await _personsApi.Get();
            persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task PostAndDelete_AssertDb()
        {
            // Act
            await _personsApi.Post(new Person(0, "FirstName1", "LastName1", null));
            await _personsApi.Post(new Person(0, "FirstName2", "LastName2", "MiddleName2"));
            await _personsApi.Post(new Person(0, "FirstName3", "LastName3", null));

            await _personsApi.Delete(2);

            // Assert: verify data was added to the db.
            var db = _dbContextFactory.CreateDbContext();
            db.Persons.Should().HaveCount(2);
        }
    }
}
