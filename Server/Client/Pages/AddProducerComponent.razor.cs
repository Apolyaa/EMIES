using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class AddProducerComponent
    {
        public ProducerDto _producer = new();
        public async Task AddProducer()
        {
            _producer.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addproducer", _producer);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            await Bus.Publish(_producer);
        }
    }
}
