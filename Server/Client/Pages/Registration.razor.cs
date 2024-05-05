using Blazored.Modal.Services;
using Client.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Registration
    {
        public string? _name;
        public string? _login;
        public string? _password;
        public string? _passwordAck;
        public bool _isExpert = false;
        [CascadingParameter] public IModalService Modal { get; set; } = default!;

        public async void Register()
        {
            if (_password == _passwordAck)
            {
                int role;
                if (_isExpert)
                    role = 1;
                else
                    role = 0;

                if (_name is null)
                {
                    ShowError("Поле ФИО является обязательным.");
                    return;
                }

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

                UserDto userDto = new() { Name = _name, Login = _login, Password = _password, Id = Guid.NewGuid(), Role = role};
                var response = await httpClient.PostAsJsonAsync("http://localhost:5102/createuser", userDto);
                var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
                if (result is not null && !result.Success)
                {
                    ShowError(result.Message!);
                    return;
                }

                if (role == 0)
                {
                    Manager.NavigateTo("/userinterface");
                }
                else
                    Manager.NavigateTo("/expertinterface");
            }

            ShowError("Пароли не совпадают!");
        }

        public void GoToEntry()
        {
            Manager.NavigateTo("/entry");
        }
        public void ShowError(string message)
        {
            Modal.Show<ErrorComponent>(message);
        }
    }
}
