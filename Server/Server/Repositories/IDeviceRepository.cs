using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IDeviceRepository
    {
        IEnumerable<DeviceEntity> GetAll();
        DeviceEntity GetById(int id);
        void Insert(DeviceEntity device);
        void Update(DeviceEntity device);
        void Delete(Guid id);
        void Save();
    }
}
