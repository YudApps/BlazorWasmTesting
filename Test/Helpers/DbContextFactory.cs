using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorWasmTesting.Test.Helpers
{
    public class DbContextFactory : IDisposable
    {
        public  SqliteConnection Connection { get; private set; }

        public DbContextFactory()
        {
            Connection = new SqliteConnection("Filename=:memory:");
            Connection.Open();

            var dbContext = CreateDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.SaveChanges();
        }

        public Server.Db.BlazorWasmTestingDbContext CreateDbContext() => new
        (
            new DbContextOptionsBuilder<Server.Db.BlazorWasmTestingDbContext>()
                .UseSqlite(Connection)
                .Options
        );

        public void Dispose()
        {
            Connection!.Dispose();
            Connection = null!;
        }
    }
}
