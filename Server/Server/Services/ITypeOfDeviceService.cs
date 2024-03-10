using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Services
{
    public interface ITypeOfDeviceService
    {
        Response<List<TypeOfDeviceDto>> GetTypes();
    }
}
