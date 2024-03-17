using Client.Contracts;

namespace Server.Services
{
    public interface IDictionaryOfCharacteristicService
    {
        Response<List<DictionaryOfCharacteristicDto>> GetMainCharacteristicsByCharacteristicId(HashSet<Guid> characteristicsId);
        Response<List<DictionaryOfCharacteristicDto>> GetCharacteristics();
        Response<bool> AddCharacteristic(DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto);
        Response<bool> DeleteCharacteristic(Guid characteristicId);
        Response<bool> UpdateCharacteristic(DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto);
    }
}
