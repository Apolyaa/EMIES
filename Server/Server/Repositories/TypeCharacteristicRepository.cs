using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class TypeCharacteristicRepository : ITypeCharacteristicRepository
    {
        private readonly ApplicationDbContext _context;

        public TypeCharacteristicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TypeCharacteristicEntity> GetAll()
        {
            return _context.TypesCharacteristics.ToList();
        }

        public TypeCharacteristicEntity GetById(int id)
        {
            return _context.TypesCharacteristics.Find(id);
        }

        public void Insert(TypeCharacteristicEntity characteristic)
        {
            _context.TypesCharacteristics.Add(characteristic);
        }

        public void Update(TypeCharacteristicEntity characteristic)
        {
            Delete(characteristic.Id);
            Save();
            Insert(characteristic);
        }

        public void Delete(Guid id)
        {
            TypeCharacteristicEntity characteristic = _context.TypesCharacteristics.Find(id);
            if (characteristic != null)
            {
                _context.TypesCharacteristics.Remove(characteristic);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
