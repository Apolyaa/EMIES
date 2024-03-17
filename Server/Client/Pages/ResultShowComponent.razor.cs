using Client.Contracts;
using Microsoft.AspNetCore.Components;

namespace Client.Pages
{
    public partial class ResultShowComponent
    {
        [Parameter]
        public ResultDto Result { get; set; }

        public Dictionary<Guid, DeviceDto> _devices = new();
        protected override async Task OnParametersSetAsync()
        {
            _devices.Clear();
            foreach (var device in Result.Devices)
            {
                _devices.Add(device.Id, device);
            }
        }
    }
}
