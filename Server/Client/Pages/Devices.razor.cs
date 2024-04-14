using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlazorComponentBus;

namespace Client.Pages
{
    public partial class Devices
    {
        public List<DeviceDto> _devices = new();
        public List<ProducerDto> _producers = new();
        public List<SourceDto> _sources = new();
        public List<TypeOfDeviceDto> _types = new();
        public DeviceDto _selectedDevice = new();
        public string? _error;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addDevice;
        public IModalReference _changeCharacteristics;
        protected override async Task OnInitializedAsync()            
        {
            var responseDevice = await httpClient.GetAsync("http://localhost:5102/getdevices");
            var resultDevice = await responseDevice.Content.ReadFromJsonAsync<Response<List<DeviceDto>>>();
            if (resultDevice is not null && resultDevice.Success)
                _devices = resultDevice.Data!;
            else
                ShowError(resultDevice.Message);

            var responseProducer = await httpClient.GetAsync("http://localhost:5102/getproducers");
            var resultProducer = await responseProducer.Content.ReadFromJsonAsync<Response<List<ProducerDto>>>();
            if (resultProducer is not null && resultProducer.Success)
                _producers = resultProducer.Data!;
            else
                ShowError(resultProducer.Message);


            var responseSource = await httpClient.GetAsync("http://localhost:5102/getsources");
            var resultSource = await responseSource.Content.ReadFromJsonAsync<Response<List<SourceDto>>>();
            if (resultSource is not null && resultSource.Success)
                _sources = resultSource.Data!;
            else
                ShowError(resultSource.Message);

            var responseType = await httpClient.GetAsync("http://localhost:5102/gettypes");
            var resultType = await responseType.Content.ReadFromJsonAsync<Response<List<TypeOfDeviceDto>>>();
            if (resultType is not null && resultType.Success)
                _types = resultType.Data!;
            else
                ShowError(resultType.Message);

            Bus.Subscribe<DeviceDto>(AddDeviceInTable);
            Bus.Subscribe<List<CharacteristicDto>>(ChangeCharacteristicsInDevice);
        }
        public void AddDevice()
        {
            _addDevice = Modal.Show<AddDeviceComponent>("Добавление прибора");
        }
        public async Task AddDeviceInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<DeviceDto>();
            _devices.Add(response);

            _addDevice.Close();
            StateHasChanged();
        }
        public void ChangeCharacteristics(DeviceDto device)
        {
            _selectedDevice = device;
            var parameters = new ModalParameters().Add(nameof(ChangeCharacteristicsComponent.Type), device.Type)
                .Add(nameof(ChangeCharacteristicsComponent.AddMode), false)
                .Add(nameof(ChangeCharacteristicsComponent.Characteristics), device.Characteristics);
            var options = new ModalOptions()
            {
                Size = ModalSize.Large
            };
            _changeCharacteristics = Modal.Show<ChangeCharacteristicsComponent>("Изменение характеристик прибора", parameters, options); 
        }
        public async Task ChangeCharacteristicsInDevice(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<List<CharacteristicDto>>();
            _devices.Remove(_selectedDevice);
            _selectedDevice.Characteristics = response;
            _devices.Add(_selectedDevice);
            await UpdateDevice(_selectedDevice);

            _changeCharacteristics.Close();
            StateHasChanged();
        }
        public async Task DeleteDevice(DeviceDto deviceDto)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5102/deletedevice/{deviceDto.Id}");
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && result.Success)
                _devices.Remove(deviceDto);
            else
                ShowError(result.Message);
        }
        public async Task UpdateDevice(DeviceDto deviceDto)
        {
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5102/updatedevice", deviceDto);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                ShowError(result.Message);
        }
        public string GetProducerName(DeviceDto deviceDto)
        {
            return deviceDto.Producer.Name;
        }
        public string GetSourceName(DeviceDto deviceDto)
        {
            return deviceDto.Source.Name;
        }
        public string GetTypeName(DeviceDto deviceDto)
        {
            return deviceDto.Type.Name;
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
