using Client.Contracts;
using System.ComponentModel;

namespace Server.Services
{
    public interface ITypeCharacteristicService
    {
        Response<List<DictionaryOfCharacteristicDto>> GetMainCharacteristicByTypeId(Guid typeId);
        Response<bool> ChangeTypesCharacteristics(List<DictionaryOfCharacteristicDto> characteristics, Guid typeId);
        Response<bool> AddTypesCharacterictics(List<DictionaryOfCharacteristicDto> characteristics, Guid typeId);
        Response<bool> DeleteTypesCharacteristics(Guid typeId);
    }
}
