using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IResultDeviceRepository
    {
        IEnumerable<ResultDeviceEntity> GetAll();
        ResultDeviceEntity GetById(int id);
        void Insert(ResultDeviceEntity resultDevice);
        void Update(ResultDeviceEntity resultDevice);
        void Delete(int id);
        void Save();
    }
}
