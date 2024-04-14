using Blazored.Modal.Services;
using Blazored.Modal;
using Client.Contracts;
using Microsoft.AspNetCore.Components;

namespace Client.Pages
{
    public partial class ResultShowComponent
    {
        [Parameter]
        public ResultDto Result { get; set; }

        public Dictionary<Guid, DeviceDto> _devices = new();
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        public IModalReference _compareCharacteristic;
        protected override async Task OnParametersSetAsync()
        {
            _devices.Clear();
            foreach (var device in Result.Devices)
            {
                _devices.Add(device.Id, device);
            }
        }
        public void CompareCharacteristics(DeviceDto device)
        {
            var parameters = new ModalParameters().Add(nameof(ShowCompareCharacteristicsComponent.CharacteristicsForFind), Result.InitialData)
                .Add(nameof(ShowCompareCharacteristicsComponent.Characteristics), device.Characteristics);
            var options = new ModalOptions()
            {
                Size = ModalSize.Large
            };
            _compareCharacteristic = Modal.Show<ShowCompareCharacteristicsComponent>("Сравнение характеристик", parameters, options);
        }
    }
}
