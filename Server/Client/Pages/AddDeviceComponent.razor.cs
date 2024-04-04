using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlazorComponentBus;

namespace Client.Pages
{
    public partial class AddDeviceComponent
    {
        public DeviceDto _addDevice = new();
        public List<SourceDto> _sources = new();
        public List<TypeOfDeviceDto> _types = new();
        public List<ProducerDto> _producers = new();
        public string? _error;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addCharacteristics;
        protected override async Task OnInitializedAsync()
        {
            var responseProducer = await httpClient.GetAsync("http://localhost:5102/getproducers");
            var resultProducer = await responseProducer.Content.ReadFromJsonAsync<Response<List<ProducerDto>>>();
            if (resultProducer is not null && resultProducer.Success)
                _producers = resultProducer.Data!;
            else
                _error = resultProducer.Message!;


            var responseSource = await httpClient.GetAsync("http://localhost:5102/getsources");
            var resultSource = await responseSource.Content.ReadFromJsonAsync<Response<List<SourceDto>>>();
            if (resultSource is not null && resultSource.Success)
                _sources = resultSource.Data!;
            else
                _error = resultSource.Message!;

            var responseType = await httpClient.GetAsync("http://localhost:5102/gettypes");
            var resultType = await responseType.Content.ReadFromJsonAsync<Response<List<TypeOfDeviceDto>>>();
            if (resultType is not null && resultType.Success)
                _types = resultType.Data!;
            else
                _error = resultType.Message!;
            Bus.Subscribe<AddCharacteristicEvent>(AddCharacteristicsOfDevice);
        }
        public async Task AddDevice()
        {
            _addDevice.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/adddevice", _addDevice);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            await Bus.Publish(_addDevice);
        }
        public void AddCharacteristics()
        {
            bool addMode;
            string? title;
            if (_addDevice.Characteristics is not null && _addDevice.Characteristics.Any())
            {
                addMode = false;
                title = "Изменение характеристик прибора";
            }
            else
            {
                addMode = true;
                title = "Добавление характеристик прибора";
            }
            var parameter = new ModalParameters().Add(nameof(ChangeCharacteristicsComponent.Type), _addDevice.Type)
                .Add(nameof(ChangeCharacteristicsComponent.AddMode), addMode)
                .Add(nameof(ChangeCharacteristicsComponent.Characteristics), _addDevice.Characteristics);

            var options = new ModalOptions()
            {
                Size = ModalSize.Large
            };

            _addCharacteristics = Modal.Show<ChangeCharacteristicsComponent>(title, parameter, options);

        }
        public async Task AddCharacteristicsOfDevice(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<AddCharacteristicEvent>();

            _addDevice.Characteristics = response.Characteristics;
            _addCharacteristics.Close();
            StateHasChanged();
        }
    }
}
