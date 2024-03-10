using Client.Contracts;
using System.ComponentModel;

namespace Server.Services
{
    public interface ITypeCharacteristicService
    {
        Response<List<DictionaryOfCharacteristicDto>> GetMainCharacteristicByTypeId(Guid typeId);
    }
}
