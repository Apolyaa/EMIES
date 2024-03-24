using Client.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace Client.Pages
{
    public partial class ChangeMainCharacteristicsComponent
    {
        [ParameterAttribute]
        public List<DictionaryOfCharacteristicDto> Characteristics { get; set; }
        public HashSet<Guid> _setCharacteristicsId = new();
        public List<DictionaryOfCharacteristicDto> _allCharacteristics = new();
        protected override async Task OnInitializedAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getcharacteristics");
            var result = await response.Content.ReadFromJsonAsync<Response<List<DictionaryOfCharacteristicDto>>>();
            if (result is not null && result.Success)
                _allCharacteristics = result.Data!;

            _setCharacteristicsId = Characteristics.Select(c => c.Id).ToHashSet();
        }
        public void SetCharacteristic(bool value, DictionaryOfCharacteristicDto characteristicDto)
        {
            if (value)
            {
                if (!_setCharacteristicsId.Contains(characteristicDto.Id))
                    _setCharacteristicsId.Add(characteristicDto.Id);
                return;
            }
            if(!value)
                if (_setCharacteristicsId.Contains(characteristicDto.Id))
                    _setCharacteristicsId.Remove(characteristicDto.Id);
        }
        public void SaveCharacteristics()
        {
            var result = _allCharacteristics.Where(c => _setCharacteristicsId.Contains(c.Id)).ToList();
            Bus.Publish(result);
        }
    }
}
