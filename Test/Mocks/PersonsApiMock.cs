using BlazorWasmTesting.Shared.Api;
using BlazorWasmTesting.Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Test.Mocks
{
    class PersonsApiMock : IPersonsApi
    {
        public IDictionary<int,Person> Persons { get; } = new Dictionary<int, Person>();

        public Task Delete(int key)
        {
            Persons.Remove(key);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Person>> Get()
        {
            return Task.FromResult<IEnumerable<Person>>(Persons.Values);
        }

        public Task Post(Person person)
        {
            person.Key = Persons.Count + 1;
            Persons.Add(person.Key, person);
            return Task.CompletedTask;
        }

        public Task Put(Person person)
        {
            Persons[person.Key] = person;
            return Task.CompletedTask;
        }
    }
}
