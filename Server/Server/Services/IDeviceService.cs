using Client.Contracts;

namespace Server.Services
{
    public interface IDeviceService
    {
        Response<ResultDto> GetSuitableDevices(Guid typeId, List<CharacteristicForFindDto> characteristicForFinds);
    }
}
