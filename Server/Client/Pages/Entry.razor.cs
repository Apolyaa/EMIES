using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Entry
    {
        public string? _login;
        public string? _password;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;

        public async void Enter()
        {
            if (_login is null)
            {
                ShowError("Поле Логин является обязательным.");
                return;
            }

            if (_password is null)
            {
                ShowError("Поле Пароль является обязательным.");
                return;
            }

            UserDto userDto = new() { Name = "", Login = _login, Password = _password, Id = Guid.NewGuid(), Role = 0 };
            var response = await httpClient.PostAsJsonAsync("http://localhost:5102/entry", userDto);
            var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();

            if (result is null)
                return;

            if (result is not null && !result.Success)
            {
                ShowError(result.Message!);
                return;
            }

            if (result.Data.Role == 0)
            {
                Manager.NavigateTo("/userinterface");
            }                
            else
                Manager.NavigateTo("/expertinterface");
        }

        public void GoToRegistration()
        {
            Manager.NavigateTo("/registration");
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
