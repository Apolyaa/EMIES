﻿using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Reflection;

namespace Client.Pages
{
    public partial class AddCharacteristicInDictionaryComponent
    {
        public DictionaryOfCharacteristicDto _characteristic = new();
        public string _stringSynonyms = string.Empty;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public async Task AddCharacteristic()
        {
            _characteristic.Synonyms = _stringSynonyms.Split(',').ToList();
            _characteristic.Id = Guid.NewGuid();
            var response = await httpClient.PutAsJsonAsync($"http://localhost:5102/addcharacteristic", _characteristic);
            var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }
                
            await Bus.Publish(_characteristic);
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
