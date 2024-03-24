using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class AddUnitComponent
    {
        public UnitOfMesurementDto _unit = new();
        public async Task AddUnit()
        {
            _unit.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addunit", _unit);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            await Bus.Publish(_unit);
        }
    }
}
