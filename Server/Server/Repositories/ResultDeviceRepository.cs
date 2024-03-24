using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class ResultDeviceRepository : IResultDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResultDeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResultDeviceEntity> GetAll()
        {
            return _context.ResultsDevices.ToList();
        }

        public ResultDeviceEntity GetById(int id)
        {
            return _context.ResultsDevices.Find(id);
        }

        public void Insert(ResultDeviceEntity resultDevice)
        {
            _context.ResultsDevices.Add(resultDevice);
        }

        public void Update(ResultDeviceEntity resultDevice)
        {
            Delete(resultDevice.Id);
            Insert(resultDevice);
        }

        public void Delete(Guid id)
        {
            ResultDeviceEntity resultDevice = _context.ResultsDevices.Find(id);
            if (resultDevice != null)
            {
                _context.ResultsDevices.Remove(resultDevice);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
