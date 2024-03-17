using Client.Contracts;
using System.Net.Http.Json;

namespace Client.Pages
{
    public partial class Registration
    {
        public string? _name;
        public string? _login;
        public string? _password;
        public string? _passwordAck;
        public string? _error;
        public bool _isError = false;
        public bool _isExpert = false;

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
                    PrintError("Поле ФИО является обязательным.");
                    return;
                }

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

                UserDto userDto = new() { Name = _name, Login = _login, Password = _password, Id = Guid.NewGuid(), Role = role};
                var response = await httpClient.PostAsJsonAsync("http://localhost:5102/createuser", userDto);
                var result = await response.Content.ReadFromJsonAsync<Response<bool>>();
                if (result is not null && !result.Success)
                {
                    PrintError(result.Message!);
                    return;
                }

                if (role == 0)
                {
                    Manager.NavigateTo("/userinterface");
                }
                else
                    Manager.NavigateTo("/expertinterface");

                _isError = false;
            }

            PrintError("Пароли не совпадают!");
            StateHasChanged();
        }

        public void GoToEntry()
        {
            Manager.NavigateTo("/entry");
        }
        private void PrintError(string message)
        {
            _isError = true;
            _error = message;
            StateHasChanged();
        }
    }
}
