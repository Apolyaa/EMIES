using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Pages
{
    public partial class AddUnitComponent
    {
        public UnitOfMeasurementDto _unit = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public async Task AddUnit()
        {
            _unit.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addunit", _unit);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }
            await Bus.Publish(_unit);
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
