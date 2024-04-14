using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Pages
{
    public partial class AddSourceComponent
    {
        public SourceDto _source = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public async Task AddSource()
        {
            _source.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addsource", _source);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }
                
            await Bus.Publish(_source);
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
