﻿using BlazorComponentBus;
using Blazored.Modal;
using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class ChangeCharacteristicsComponent
    {
        [Parameter]
        public TypeOfDeviceDto Type { get; set; }
        [Parameter]
        public bool AddMode { get; set; }
        [Parameter]
        public List<CharacteristicDto> Characteristics { get; set; } = new ();
        public List<CharacteristicDto> _characteristics = new();
        public List<DictionaryOfCharacteristicDto> _dictionaryOfCharacteristics = new();
        public List<UnitOfMeasurementDto> _units = new();
        public Dictionary<string, string> _typesCharacteristic = new()
        {
            { "Диапазон",TypeCharacteristicConstants.RANGE },
            { "Число",TypeCharacteristicConstants.NUMBER },
            { "Строка",TypeCharacteristicConstants.STRING },
            { "Массив значений",TypeCharacteristicConstants.ARRAYOFVALUES },
            { "Булево значение (да, нет)", TypeCharacteristicConstants.BOOLEAN }
        };
        public string? _error;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addCharacteristic;
        protected override async Task OnInitializedAsync()
        {

            if (AddMode)
            {
                foreach (var mainCharacteristic in Type.MainCharacteristics)
                {
                    var characteristic = new CharacteristicDto() { Id = Guid.NewGuid(), Name = mainCharacteristic.Name, 
                        DictionaryOfCharacteristic = mainCharacteristic, 
                        Type = string.Empty, 
                        Unit = new UnitOfMeasurementDto(),
                        Value = string.Empty
                    };
                    _characteristics.Add(characteristic);
                }
            }
            else
                _characteristics = Characteristics;

            var responseUnits = await httpClient.GetAsync("http://localhost:5102/getunits");
            var resultUnits = await responseUnits.Content.ReadFromJsonAsync<Response<List<UnitOfMeasurementDto>>>();
            if (resultUnits is not null && resultUnits.Success)
                _units = resultUnits.Data!;
            else
                _error = resultUnits.Message!;

            var responseDictionary = await httpClient.GetAsync("http://localhost:5102/getcharacteristics");
            var resultDictionary = await responseDictionary.Content.ReadFromJsonAsync<Response<List<DictionaryOfCharacteristicDto>>>();
            if (resultDictionary is not null && resultDictionary.Success)
                _dictionaryOfCharacteristics = resultDictionary.Data!;
            else
                _error = resultDictionary.Message!;

            Bus.Subscribe<DictionaryOfCharacteristicDto>(AddCharacteristicInTable);
        }
        public void SaveCharacteristics()
        {
            var data = new AddCharacteristicEvent() { Characteristics = _characteristics };
            Bus.Publish(data);
        }
        public void AddCharacteristic()
        {
            _addCharacteristic = Modal.Show<AddCharacteristicComponent>("Добавление характеристики из словаря");
        }
        public async Task AddCharacteristicInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<DictionaryOfCharacteristicDto>();
            _characteristics.Add(new CharacteristicDto()
            {
                Id = Guid.NewGuid(),
                Name = response.Name,
                DictionaryOfCharacteristic = response,
                Type = string.Empty,
                Unit = new UnitOfMeasurementDto(),
                Value = string.Empty
            });
            _addCharacteristic.Close();
            StateHasChanged();
        }
    }
}
