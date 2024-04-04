using Client.Contracts;

namespace Server.Services
{
    public interface ITypeOfDeviceService
    {
        Response<List<TypeOfDeviceDto>> GetTypes();
        Response<TypeOfDeviceDto> GetType(Guid typeId);
        Response<bool> AddType(TypeOfDeviceDto typeOfDeviceDto);
        Response<bool> UpdateType(TypeOfDeviceDto typeOfDeviceDto);
        Response<bool> DeleteType(Guid typeId);
    }
}
