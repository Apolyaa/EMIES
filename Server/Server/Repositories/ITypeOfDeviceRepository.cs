using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface ITypeOfDeviceRepository
    {
        IEnumerable<TypeOfDevicesEntity> GetAll();
        TypeOfDevicesEntity GetById(int id);
        void Insert(TypeOfDevicesEntity type);
        void Update(TypeOfDevicesEntity type);
        void Delete(int id);
        void Save();
    }
}
