using BlazorComponentBus;
using Blazored.Modal;
using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Units
    {
        public List<UnitOfMeasurementDto> _units = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addUnit;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getunits");
            var result = await response.Content.ReadFromJsonAsync<Response<List<UnitOfMeasurementDto>>>();
            if (result is not null && result.Success)
                _units = result.Data!;
            else
                ShowError(result.Message);

            Bus.Subscribe<UnitOfMeasurementDto>(AddUnitInTable);
        }
        public void AddUnit()
        {
            _addUnit = Modal.Show<AddUnitComponent>("Добавление единицы измерения");
        }
        public async Task AddUnitInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<UnitOfMeasurementDto>();
            _units.Add(response);

            _addUnit.Close();
            StateHasChanged();
        }
        public async Task DeleteUnit(UnitOfMeasurementDto unitOfMesurementDto)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5102/deleteunit/{unitOfMesurementDto.Id}");
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && result.Success)
            {
                _units.Remove(unitOfMesurementDto);
            }
            else
                ShowError(result.Message);
        }
        public async Task UpdateUnit(UnitOfMeasurementDto unitOfMesurementDto)
        {
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5102/updateunit", unitOfMesurementDto);
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
