using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Services
{
    public interface ICharacteristicService
    {
        Response<List<CharacteristicEntity>> GetCharacteristicsByDeviceId(Guid deviceId);
        Response<ResultCompareCharacteristicsDto> CompareCharacteristicsOfDevice(Guid deviceId, List<CharacteristicForFindDto> characteristicForFinds);
        Response<bool> AddCharacteristics(List<CharacteristicDto> characteristicDtos, Guid deviceId);
        Response<bool> DeleteCharacteristics(Guid deviceId);
        Response<bool> UpdateCharacteristics(List<CharacteristicDto> characteristicDtos, Guid deviceId);
        Response<List<CharacteristicDto>> GetCharacteristics(Guid deviceId);
    }
}
