using Client.Contracts;
using Microsoft.AspNetCore.Components;

namespace Client.Pages
{
    public partial class ShowCompareCharacteristicsComponent
    {
        [Parameter]
        public List<CharacteristicDto> Characteristics { get; set; }
        [Parameter]
        public List<CharacteristicForFindDto> CharacteristicsForFind { get; set; }  

        public string GetValue(CharacteristicForFindDto characteristicForFind)
        {
            var characteristic = Characteristics.FirstOrDefault(c => c.Name == characteristicForFind.Name);
            if (characteristic is not null)
            {
                if (characteristic.Unit is not null)
                    return characteristic.Value + " " + characteristic.Unit.Name;
                return characteristic.Value;
            }
                
            return string.Empty;
        }
        public string GetValueCharacteristic(CharacteristicForFindDto characteristicForFind)
        {
            if (characteristicForFind.UnitOfMeasurement is not null)
                return characteristicForFind.Value + " " + characteristicForFind.UnitOfMeasurement.Name;
            return characteristicForFind.Value;
        }
    }
}
