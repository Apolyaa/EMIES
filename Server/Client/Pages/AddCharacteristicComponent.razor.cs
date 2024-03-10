using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class AddCharacteristicComponent
    {
        public List<DictionaryOfCharacteristicDto> _characteristics = new();
        public DictionaryOfCharacteristicDto? _selectCharacteristic;
        public string? _error;
        public string? _findText;

        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getcharacteristics");
            var result = await response.Content.ReadFromJsonAsync<Response<List<DictionaryOfCharacteristicDto>>>();
            if (result is not null && result.Success)
                _characteristics = result.Data!;
            else
                _error = result.Message!;
        }
        public void SelectCharacteristic(string name)
        {
            _selectCharacteristic = _characteristics.FirstOrDefault(x => x.Name == name);
        }
        public void AddCharacteristic()
        {
            Bus.Publish<DictionaryOfCharacteristicDto>(_selectCharacteristic);
        }
    }
}
