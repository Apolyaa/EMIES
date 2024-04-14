using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlazorComponentBus;

namespace Client.Pages
{
    public partial class Sources
    {
        public List<SourceDto> _sources = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addSource;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getsources");
            var result = await response.Content.ReadFromJsonAsync<Response<List<SourceDto>>>();
            if (result is not null && result.Success)
                _sources = result.Data!;
            else
                ShowError(result.Message);

            Bus.Subscribe<SourceDto>(AddSourceInTable);
        }
        public void AddSource()
        {
            _addSource = Modal.Show<AddSourceComponent>("Добавление источника");
        }
        public async Task AddSourceInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<SourceDto>();
            _sources.Add(response);

            _addSource.Close();
            StateHasChanged();
        }
        public async Task DeleteSource(SourceDto sourceDto)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5102/deletesource/{sourceDto.Id}");
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && result.Success)
                _sources.Remove(sourceDto);
            else
                ShowError(result.Message);
        }
        public async Task UpdateSource(SourceDto sourceDto)
        {
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5102/updatesource", sourceDto);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                ShowError(result.Message);
        }
        public void GoToCharacteristics()
        {
            Manager.NavigateTo("/characteristics");
        }

        public void GoToDevices()
        {
            Manager.NavigateTo("/devices");
        }

        public void GoToTypes()
        {
            Manager.NavigateTo("/types");
        }
        public void GoToUnits()
        {
            Manager.NavigateTo("/units");
        }
        public void GoToSources()
        {
            Manager.NavigateTo("/sources");
        }
        public void GoToProducers()
        {
            Manager.NavigateTo("/producers");
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
