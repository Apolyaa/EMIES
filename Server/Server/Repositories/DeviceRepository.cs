using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DeviceEntity> GetAll()
        {
            return _context.Devices.ToList();
        }

        public DeviceEntity GetById(int id)
        {
            return _context.Devices.Find(id);
        }

        public void Insert(DeviceEntity device)
        {
            _context.Devices.Add(device);
        }

        public void Update(DeviceEntity device)
        {
            _context.Entry(device).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DeviceEntity device = _context.Devices.Find(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
