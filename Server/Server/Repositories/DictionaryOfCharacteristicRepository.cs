using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class DictionaryOfCharacteristicRepository : IDictionaryOfCharacteristicRepository
    {
        private readonly ApplicationDbContext _context;

        public DictionaryOfCharacteristicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DictionaryOfCharacteristicEntity> GetAll()
        {
            return _context.DictionaryOfCharacteristics.ToList();
        }

        public DictionaryOfCharacteristicEntity GetById(int id)
        {
            return _context.DictionaryOfCharacteristics.Find(id);
        }

        public void Insert(DictionaryOfCharacteristicEntity dictionaryOfCharacteristic)
        {
            _context.DictionaryOfCharacteristics.Add(dictionaryOfCharacteristic);
        }

        public void Update(DictionaryOfCharacteristicEntity dictionaryOfCharacteristic)
        {
            Delete(dictionaryOfCharacteristic.Id);
            Insert(dictionaryOfCharacteristic);
        }

        public void Delete(Guid id)
        {
            DictionaryOfCharacteristicEntity dictionaryOfCharacteristic = _context.DictionaryOfCharacteristics.Find(id);
            if (dictionaryOfCharacteristic != null)
            {
                _context.DictionaryOfCharacteristics.Remove(dictionaryOfCharacteristic);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
