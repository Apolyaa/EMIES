using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using BlazorComponentBus;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Types
    {
        public List<TypeOfDeviceDto> _types = new();
        public Guid _selectedType;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _changeMainCharacteristics;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/gettypes");
            var result = await response.Content.ReadFromJsonAsync<Response<List<TypeOfDeviceDto>>>();
            if (result is not null && result.Success)
                _types = result.Data!;
            else
                _error = result.Message!;

            Bus.Subscribe<List<DictionaryOfCharacteristicDto>>(ChangeMainCharacteristics);
        }
        public void ChangeMainCharacteristics(TypeOfDeviceDto typeOfDeviceDto)
        {
            _selectedType = typeOfDeviceDto.Id;
            var parameters = new ModalParameters().Add(nameof(ChangeMainCharacteristicsComponent.Characteristics),
                typeOfDeviceDto.MainCharacteristics);
            _changeMainCharacteristics = Modal
                .Show<ChangeMainCharacteristicsComponent>("Изменение основных характеристик типа прибора", parameters);
        }
        public async Task ChangeMainCharacteristics(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<List<DictionaryOfCharacteristicDto>>();
            var type = _types.FirstOrDefault(t => t.Id == _selectedType);
            _types.Remove(type);
            type.MainCharacteristics = response;
            _types.Add(type);
            var responseHttp = await httpClient.PostAsJsonAsync($"http://localhost:5102/updatetype", type);
            var result = await responseHttp.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                _error = result.Message!;

            _changeMainCharacteristics.Close();
            StateHasChanged();
        }
        public async Task AddType()
        {
            TypeOfDeviceDto typeOfDeviceDto = new() { Id = Guid.NewGuid()};
            var responseHttp = await httpClient.PutAsJsonAsync($"http://localhost:5102/addtype", typeOfDeviceDto);
            var result = await responseHttp.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                _error = result.Message!;
            else
                _types.Add(typeOfDeviceDto);
        }
        public async Task UpdateType(TypeOfDeviceDto typeOfDeviceDto)
        {
            var responseHttp = await httpClient.PostAsJsonAsync($"http://localhost:5102/updatetype", typeOfDeviceDto);
            var result = await responseHttp.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                _error = result.Message!;
        }
        public async Task DeleteType(TypeOfDeviceDto typeOfDeviceDto)
        {
            var responseHttp = await httpClient.DeleteAsync($"http://localhost:5102/deletetype/{typeOfDeviceDto.Id}");
            var result = await responseHttp.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                _error = result.Message!;
            else
                _types.Remove(typeOfDeviceDto);
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
