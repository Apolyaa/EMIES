using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlazorComponentBus;
using System.Text;

namespace Client.Pages
{
    public partial class AddDeviceComponent
    {
        public DeviceDto _addDevice = new();
        public List<SourceDto> _sources = new();
        public List<TypeOfDeviceDto> _types = new();
        public List<ProducerDto> _producers = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addCharacteristics;
        protected override async Task OnInitializedAsync()
        {
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
            Bus.Subscribe<AddCharacteristicEvent>(AddCharacteristicsOfDevice);
        }
        public async Task AddDevice()
        {
            StringBuilder stringBuilder = new();
            if (_addDevice.Type is null)
                stringBuilder.Append("Укажите тип прибора. ");
            if (_addDevice.Source is null)
                stringBuilder.Append("Укажите источник прибора. ");
            if (_addDevice.Producer is null)
                stringBuilder.Append("Укажите производителя прибора. ");
            if (_addDevice.Characteristics is null || !_addDevice.Characteristics.Any())
                stringBuilder.Append("Укажите характеристики прибора. ");
            var message = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(message))
            {
                ShowError(message);
                return;
            }
            
            _addDevice.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/adddevice", _addDevice);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }
                
            await Bus.Publish(_addDevice);
        }
        public void AddCharacteristics()
        {
            if (_addDevice.Type is null)
            {
                ShowError("Укажите тип прибора.");
                return;
            }

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
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
