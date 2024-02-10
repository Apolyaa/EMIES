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

        public void Register()
        {
            if (_password == _passwordAck)
            {
                Manager.NavigateTo("/");
                _isError = false;
            }

            _isError = true;
            _error = "Пароли не совпадают!";
            StateHasChanged();
        }

        public void GoToEntry()
        {
            Manager.NavigateTo("/entry");
        }
    }
}
