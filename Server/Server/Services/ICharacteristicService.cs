using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Services
{
    public interface ICharacteristicService
    {
        Response<List<CharacteristicEntity>> GetCharacteristicsByDeviceId(Guid deviceId);
        Response<ResultCompareCharacteristicsDto> CompareCharacteristicsOfDevice(Guid deviceId, List<CharacteristicForFindDto> characteristicForFinds);
        //Response<bool> AddCharacteristic(CharacteristicDto characteristicDto);
        //Response<bool> DeleteCharacteristic(Guid characteristicId);
        //Response<bool> UpdateCharacteristic(CharacteristicDto characteristicDto);
    }
}
