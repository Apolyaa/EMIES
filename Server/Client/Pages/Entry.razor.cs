using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Entry
    {
        public string? _login;
        public string? _password;
        public string? _error;
        public bool _isError = false;

        public async void Enter()
        {
            if (_login is null)
            {
                PrintError("Поле Логин является обязательным.");
                return;
            }

            if (_password is null)
            {
                PrintError("Поле Пароль является обязательным.");
                return;
            }

            UserDto userDto = new() { Name = "", Login = _login, Password = _password, Id = Guid.NewGuid(), Role = 0 };
            var response = await httpClient.PostAsJsonAsync("http://localhost:5102/entry", userDto);
            var result = await response.Content.ReadFromJsonAsync<Response<UserDto>>();

            if (result is null)
                return;

            if (result is not null && !result.Success)
            {
                PrintError(result.Message!);
                return;
            }

            if (result.Data.Role == 0)
            {
                Manager.NavigateTo("/userinterface");
            }                
            else
                Manager.NavigateTo("/expertinterface");
            _isError = false;
        }

        public void GoToRegistration()
        {
            Manager.NavigateTo("/registration");
        }

        private void PrintError(string message)
        {
            _isError = true;
            _error = message;
            StateHasChanged();
        }
    }
}
