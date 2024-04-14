using BlazorComponentBus;
using Blazored.Modal;
using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;

namespace Client.Pages
{
    public partial class Characteristics
    {
        public List<DictionaryOfCharacteristicDto> _characteristics = new();
        public Dictionary<Guid, string> _synonyms = new();
        public string? _error;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _addCharacteristic;


        protected override async Task OnParametersSetAsync()
        {
            var response = await httpClient.GetAsync("http://localhost:5102/getcharacteristics");
            var result = await response.Content.ReadFromJsonAsync<Response<List<DictionaryOfCharacteristicDto>>>();
            if (result is not null && result.Success)
                _characteristics = result.Data!;
            else
                ShowError(result.Message);
            foreach(var characteristic in _characteristics)
            {
                StringBuilder stringBuilder = new();
                foreach (var synonym in characteristic.Synonyms)
                    stringBuilder.Append(synonym + ",");
                _synonyms.Add(characteristic.Id, stringBuilder.ToString().TrimEnd(','));
            }
            Bus.Subscribe<DictionaryOfCharacteristicDto>(AddCharacteristicInTable);
        }
        public async Task DeleteCharacteristic(DictionaryOfCharacteristicDto characteristicDto)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5102/deletecharacteristic/{characteristicDto.Id}");
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && result.Success)
            {
                _characteristics.Remove(characteristicDto);
                _synonyms.Remove(characteristicDto.Id);
            }
            else
                ShowError(result.Message);
        }
        public async Task UpdateCharacteristic(DictionaryOfCharacteristicDto characteristicDto)
        {
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5102/updatecharacteristic", characteristicDto);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (result is not null && !result.Success)
                _error = result.Message!;
        }
        public void AddCharacteristic()
        {
            _addCharacteristic = Modal.Show<AddCharacteristicInDictionaryComponent>("Добавление характеристики в словарь");
        }
        public async Task AddCharacteristicInTable(MessageArgs message, CancellationToken token)
        {
            if (message is null)
                return;
            var response = message.GetMessage<DictionaryOfCharacteristicDto>();
            _characteristics.Add(response);

            StringBuilder stringBuilder = new();
            foreach (var synonym in response.Synonyms)
               stringBuilder.Append(synonym + ",");
            _synonyms.Add(response.Id, stringBuilder.ToString().TrimEnd(','));

            _addCharacteristic.Close();
            StateHasChanged();
        }
        public void ChangeSynonym(Guid characteristicId, string synonyms)
        {
            if(_synonyms.TryGetValue(characteristicId, out var synonymsInDictionary))
            {
                synonymsInDictionary = synonyms;
                _synonyms[characteristicId] = synonymsInDictionary;
                var characteristic = _characteristics.FirstOrDefault(c => c.Id == characteristicId);
                _characteristics.Remove(characteristic);
                characteristic.Synonyms = synonyms.Split(',').ToList();
                _characteristics.Add(characteristic);
            }
            
        }
        public string GetSynonyms(Guid characteristicId)
        {
            if (_synonyms.TryGetValue(characteristicId, out var synonymsInDictionary))
                return synonymsInDictionary;
            return string.Empty;
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
