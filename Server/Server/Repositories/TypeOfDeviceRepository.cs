using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class TypeOfDeviceRepository : ITypeOfDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public TypeOfDeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TypeOfDevicesEntity> GetAll()
        {
            return _context.TypesOfDevices.ToList();
        }

        public TypeOfDevicesEntity GetById(int id)
        {
            return _context.TypesOfDevices.Find(id);
        }

        public void Insert(TypeOfDevicesEntity type)
        {
            _context.TypesOfDevices.Add(type);
        }

        public void Update(TypeOfDevicesEntity type)
        {
            _context.Entry(type).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            TypeOfDevicesEntity type = _context.TypesOfDevices.Find(id);
            if (type != null)
            {
                _context.TypesOfDevices.Remove(type);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
