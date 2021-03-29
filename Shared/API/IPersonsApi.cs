using BlazorWasmTesting.Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Shared.Api
{
    public interface IPersonsApi
    {
        Task Delete(int key);
        Task<IEnumerable<Person>> Get();
        Task Post(Person person);
        Task Put(Person person);
    }
}