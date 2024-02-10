namespace Client.Pages
{
    public partial class Entry
    {
        public string? _login;
        public string? _password;

        public void Enter()
        {
            Manager.NavigateTo("/");
        }

        public void GoToRegistration()
        {
            Manager.NavigateTo("/registration");
        }
    }
}
