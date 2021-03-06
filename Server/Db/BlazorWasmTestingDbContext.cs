using BlazorWasmTesting.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmTesting.Server.Db
{
    public class BlazorWasmTestingDbContext: DbContext
    {
        public BlazorWasmTestingDbContext(DbContextOptions<BlazorWasmTestingDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
    }
}
