using BlazorWasmTesting.Server.Db;
using BlazorWasmTesting.Server.ExternalApis;
using BlazorWasmTesting.Shared;
using BlazorWasmTesting.Shared.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase, IPersonsApi
    {
        private readonly ILogger<PersonsController> _logger;
        private readonly BlazorWasmTestingDbContext _dbContext;

        public PersonsController(BlazorWasmTestingDbContext dbContext, ILogger<PersonsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> Get()
        {
            _logger.LogInformation($"{nameof(Get)}()");
            return await _dbContext.Persons.ToArrayAsync();
        }

        [HttpPost]
        public Task Post(Person person)
        {
            _logger.LogInformation($"{nameof(Post)}({person})");
            _dbContext.Persons.Add(person);
            return _dbContext.SaveChangesAsync();
        }

        [HttpPut]
        public Task Put(Person person)
        {
            _logger.LogInformation($"{nameof(Put)}({person})");
            _dbContext.Persons.Update(person);
            return _dbContext.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            _logger.LogInformation($"{nameof(Delete)}({key})");
            
            _dbContext.Persons.Remove(await _dbContext.Persons.SingleAsync(s => s.Key == key));
            await _dbContext.SaveChangesAsync();
        }
    }
}
