using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class AddCharacteristicInDictionaryComponent
    {
        public DictionaryOfCharacteristicDto _characteristic = new();
        public string _stringSynonyms = string.Empty;
        public async Task AddCharacteristic()
        {
            _characteristic.Synonyms = _stringSynonyms.Split(',').ToList();
            _characteristic.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addcharacteristic", _characteristic);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            await Bus.Publish(_characteristic);
        }
    }
}
