using BlazorWasmTesting.Server.Controllers;
using BlazorWasmTesting.Test.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorWasmTesting.Test
{
    public class PersonsController_ServerOnly_Test : IDisposable
    {
        private readonly DbContextFactory _dbContextFactory;
        private readonly PersonsController _controller;

        public PersonsController_ServerOnly_Test()
        {
            _dbContextFactory = new DbContextFactory();
            _controller = new PersonsController(_dbContextFactory.CreateDbContext(), new NullLogger<PersonsController>());
        }

        public void Dispose()
        {
            _dbContextFactory.Dispose();
        }

        [Fact]
        public async Task Get_NoData()
        {
            // Arrange


            // Act
            var persons = await _controller.Get();

            // Assert
            persons.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ArrangeDB_SomeData()
        {
            // Arrange: put data directly to the Db.
            var db = _dbContextFactory.CreateDbContext();
            db.Persons.AddRange(
                new Shared.Person(0, "FirstName1", "LastName1", null),
                new Shared.Person(0, "FirstName2", "LastName2", "MiddleName2"));
            db.SaveChanges();

            // Act
            var persons = await _controller.Get();

            // Assert
            persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task Put_AssertDb()
        {
            // Act
            await _controller.Put(new Shared.Person(0, "FirstName1", "LastName1", null));
            await _controller.Put(new Shared.Person(0, "FirstName2", "LastName2", "MiddleName2"));

            // Assert: verify data was added to the db.
            var db = _dbContextFactory.CreateDbContext();
            db.Persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task Put_VerifyDbRequiredAttribute()
        {
            // Act
            Func<Task> put = () => _controller.Put(new Shared.Person(0, "FirstName1", null /*Required*/, null));

            // Assert: verify data was added to the db.
            put.Should().Throw<DbUpdateException>(because: "LastName is null");
        }

        [Fact]
        public async Task PutAndGet_ApiOnlyTesting()
        {
            // Act
            await _controller.Put(new Shared.Person(0, "FirstName1", "LastName1", null));
            await _controller.Put(new Shared.Person(0, "FirstName2", "LastName2", "MiddleName2"));
            var persons = await _controller.Get();

            // Assert
            persons.Should().HaveCount(2);
        }
    }
}
