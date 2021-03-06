using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorWasmTesting.Test.Helpers
{
    class DbContextFactory : IDisposable
    {
        private SqliteConnection _connection;

        public DbContextFactory()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var dbContext = CreateDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        public Server.Db.BlazorWasmTestingDbContext CreateDbContext() => new
        (
            new DbContextOptionsBuilder<Server.Db.BlazorWasmTestingDbContext>()
                .UseSqlite(_connection)
                .Options
        );

        public void Dispose()
        {
            _connection!.Dispose();
            _connection = null!;
        }
    }
}
