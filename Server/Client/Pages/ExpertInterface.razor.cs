namespace Client.Pages
{
    public partial class ExpertInterface
    {
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
    }
}
