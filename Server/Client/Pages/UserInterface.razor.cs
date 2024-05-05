using BlazorComponentBus;
using Blazored.Modal;
using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;

namespace Client.Pages
{
    public partial class UserInterface
    {
        public List<TypeOfDeviceDto> _types = new();
        public List<UnitOfMeasurementDto> _unitOfMesurements = new();
        public List<DictionaryOfCharacteristicDto> _mainCharacteristics = new();
        public Dictionary<Guid, CharacteristicForFindDto> _sourceCharacteristics = new();
        public Dictionary<string, string> _typeCharacteristic = new()
        {
            { "Диапазон","Range" },
            { "Число","Number" },
            { "Строка","String" },
            { "Массив значений","ArrayOfValues" },
            { "Булево значение (да, нет)","Boolean" }
        };
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public Guid _selectType;
        public RequestFind _userRequest = new(); 
        public ResultDto _result = new();
        public IModalReference _addCharacteristic;
        public IModalReference _resultShow;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/gettypes");
            var result = await response.Content.ReadFromJsonAsync<Response<List<TypeOfDeviceDto>>>();
            if (result is not null && result.Success)
                _types = result.Data!;
            else
                ShowError(result.Message);
            var responseUnits = await httpClient.GetAsync("http://localhost:5102/getunits");
            var resultUnits = await responseUnits.Content.ReadFromJsonAsync<Response<List<UnitOfMeasurementDto>>>();
            if (resultUnits is not null && resultUnits.Success)
                _unitOfMesurements = resultUnits.Data!;
            else
                ShowError(resultUnits.Message);
            Bus.Subscribe<DictionaryOfCharacteristicDto>(AddCharacteristicInTable);
        }

        public void SelectType(string name)
        {
            _sourceCharacteristics.Clear();
            var type = _types.FirstOrDefault(t => t.Name == name);
            if (type is not null)
            {
                _selectType = type.Id;
                _mainCharacteristics = type.MainCharacteristics;
                foreach (var characteristic in _mainCharacteristics)
                {
                    _sourceCharacteristics.Add(characteristic.Id, new CharacteristicForFindDto());
                }
            }
        }
        public void SelectTypeCharacteristic(Guid id, string type)
        {
            if (_typeCharacteristic.TryGetValue(type, out var typeChar))
            {
                if (_sourceCharacteristics.TryGetValue(id, out var characteristic))
                {
                    characteristic.TypeCharacteristic = typeChar;
                    _sourceCharacteristics.Remove(id);
                    _sourceCharacteristics.Add(id, characteristic);
                }
            }
        }

        public void AddCharacteristic()
        {
            _addCharacteristic = Modal.Show<AddCharacteristicComponent>("Добавление характеристики");
        }
        public async Task AddCharacteristicInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            _mainCharacteristics.Add(message.GetMessage<DictionaryOfCharacteristicDto>());
            _addCharacteristic.Close();
            StateHasChanged();
        }
        public void AddCharacteristicValue(DictionaryOfCharacteristicDto characteristicDto, string value)
        {
            if (_sourceCharacteristics.TryGetValue(characteristicDto.Id, out var characteristic))
            {
                characteristic.Value = value;
                characteristic.Name = characteristicDto.Name;
                _sourceCharacteristics.Remove(characteristicDto.Id);
                _sourceCharacteristics.Add(characteristicDto.Id, characteristic);
            }
        }
        public void AddCharacteristicUnit(Guid id, UnitOfMeasurementDto unit)
        {
            if (_sourceCharacteristics.TryGetValue(id, out var characteristic))
            {
                characteristic.UnitOfMeasurement = unit;
                _sourceCharacteristics.Remove(id);
                _sourceCharacteristics.Add(id, characteristic);
            }
        }
        public void AddIsEssential(Guid id, bool isEssetial)
        {
            if (_sourceCharacteristics.TryGetValue(id, out var characteristic))
            {
                characteristic.IsEssential = isEssetial;
                _sourceCharacteristics.Remove(id);
                _sourceCharacteristics.Add(id, characteristic);
            }
        }
        public void DeleteCharacteristic(DictionaryOfCharacteristicDto characteristicDto)
        {
            _mainCharacteristics.Remove(characteristicDto);
            if (_sourceCharacteristics.ContainsKey(characteristicDto.Id))
                _sourceCharacteristics.Remove(characteristicDto.Id);
        }
        public async Task FindDevices()
        {
            _userRequest.Characteristics = _sourceCharacteristics.Values.ToList();
            var errorCharacteristic = _userRequest.Characteristics
                .Where(c => (c.TypeCharacteristic == TypeCharacteristicConstants.NUMBER || c.TypeCharacteristic == TypeCharacteristicConstants.RANGE) && c.UnitOfMeasurement is null);
            StringBuilder stringBuilder = new();
            if (errorCharacteristic.Any())
            {
                foreach (var characteristic in errorCharacteristic)
                    stringBuilder.AppendLine($"Не указана единица измерения у характеристики {characteristic.Name}.");
                ShowError(stringBuilder.ToString());
                return;
            }

            _userRequest.TypeId = _selectType;
            var response = await httpClient.PostAsJsonAsync("http://localhost:5102/finddevices", _userRequest);
            var result = await response.Content.ReadFromJsonAsync<Response<ResultDto>>();
            if (result is not null && result.Success)
                _result = result.Data!;
            else
                ShowError(result.Message);
            var parameters = new ModalParameters().Add(nameof(ResultShowComponent.Result), _result);
            var options = new ModalOptions()
            {
                Size = ModalSize.Large
            };
            _resultShow = Modal.Show<ResultShowComponent>("Результат подбора оборудования", parameters, options);

        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
