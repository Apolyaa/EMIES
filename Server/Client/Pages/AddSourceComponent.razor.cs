using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class AddSourceComponent
    {
        public SourceDto _source = new();
        public async Task AddSource()
        {
            _source.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addsource", _source);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            await Bus.Publish(_source);
        }
    }
}
