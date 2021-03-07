using BlazorWasmTesting.Shared;
using BlazorWasmTesting.Shared.Api;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorWasmTesting.Client.Clients
{
    public class PersonsClient : IPersonsApi
    {
        private HttpClient Client { get; }

        public PersonsClient(HttpClient client)
        {
            Client = client;
        }

        public async Task Post(Person person)
        {
            var response = await Client.PostAsJsonAsync("Persons", person);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int key)
        {
            var response = await Client.DeleteAsync($"Persons/?key={key}");
            response.EnsureSuccessStatusCode();
        }

        public Task<IEnumerable<Person>> Get()
        {
            return Client.GetFromJsonAsync<IEnumerable<Person>>("Persons");
        }

        public async Task Put(Person person)
        {
            var response = await Client.PutAsJsonAsync($"Persons", person);
            response.EnsureSuccessStatusCode();
        }
    }
}
