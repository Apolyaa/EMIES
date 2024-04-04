using Client.Contracts;

namespace Server.Services
{
    public interface IDeviceService
    {
        Response<ResultDto> GetSuitableDevices(Guid typeId, List<CharacteristicForFindDto> characteristicForFinds);
        Response<bool> AddDevice(DeviceDto deviceDto);
        Response<bool> DeleteDevice(Guid deviceId);
        Response<bool> UpdateDevice(DeviceDto deviceDto);
        Response<List<DeviceDto>> GetDevices();
    }
}
