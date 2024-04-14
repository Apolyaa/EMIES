using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class ChangeMainCharacteristicsComponent
    {
        [Parameter]
        public List<DictionaryOfCharacteristicDto> Characteristics { get; set; }
        public HashSet<Guid> _setCharacteristicsId = new();
        public List<DictionaryOfCharacteristicDto> _allCharacteristics = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getcharacteristics");
            var result = await response.Content.ReadFromJsonAsync<Response<List<DictionaryOfCharacteristicDto>>>();
            if (result is not null && result.Success)
                _allCharacteristics = result.Data!;
            else
                ShowError(result.Message);

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
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
