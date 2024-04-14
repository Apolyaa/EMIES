using BlazorComponentBus;
using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Producers
    {
        public List<ProducerDto> _producers = new();
        public string? _error;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addProducer;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getproducers");
            var result = await response.Content.ReadFromJsonAsync<Response<List<ProducerDto>>>();
            if (result is not null && result.Success)
                _producers = result.Data!;
            else
                ShowError(result.Message);

            Bus.Subscribe<ProducerDto>(AddProducerInTable);
        }
        public void AddProducer()
        {
            _addProducer = Modal.Show<AddProducerComponent>("Добавление производителя");
        }
        public async Task AddProducerInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<ProducerDto>();
            _producers.Add(response);

            _addProducer.Close();
            StateHasChanged();
        }
        public async Task DeleteProducer(ProducerDto producerDto)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5102/deleteproducer/{producerDto.Id}");
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && result.Success)
                _producers.Remove(producerDto);
            else
                ShowError(result.Message);
        }
        public async Task UpdateProducer(ProducerDto producerDto)
        {
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5102/updateproducer", producerDto);
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
