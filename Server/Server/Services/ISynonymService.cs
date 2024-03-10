using Client.Contracts;

namespace Server.Services
{
    public interface ISynonymService
    {
        Response<List<SynonymDto>> GetSynonymsByCharacteristicId(Guid characteristicId);
    }
}
