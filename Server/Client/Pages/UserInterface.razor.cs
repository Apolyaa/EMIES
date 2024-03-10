using BlazorComponentBus;
using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;

namespace Client.Pages
{
    public partial class UserInterface
    {
        public List<TypeOfDeviceDto> _types = new();
        public List<UnitOfMesurementDto> _unitOfMesurements = new();
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
        public string _error = string.Empty;
        public bool _IsError;
        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/gettypes");
            var result = await response.Content.ReadFromJsonAsync<Response<List<TypeOfDeviceDto>>>();
            if (result is not null && result.Success)
                _types = result.Data!;
            else
                _error = result.Message!;
            var responseUnits = await httpClient.GetAsync("http://localhost:5102/getunits");
            var resultUnits = await responseUnits.Content.ReadFromJsonAsync<Response<List<UnitOfMesurementDto>>>();
            if (resultUnits is not null && resultUnits.Success)
                _unitOfMesurements = resultUnits.Data!;
            else
                _error = resultUnits.Message!;
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
            Modal.Show<AddCharacteristicComponent>("Добавление характеристики");
        }
        public async Task AddCharacteristicInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            _mainCharacteristics.Add(message.GetMessage<DictionaryOfCharacteristicDto>());
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
        public void AddCharacteristicUnit(Guid id, string unit)
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
        public void FindDevices()
        {

        }
        private async ValueTask DisposeAsync()
        {
            Bus.UnSubscribe<DictionaryOfCharacteristicDto>(AddCharacteristicInTable);
            await Task.CompletedTask;
        }
    }
}
