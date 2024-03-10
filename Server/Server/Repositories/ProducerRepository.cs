using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly ApplicationDbContext _context;

        public ProducerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProducerEntity> GetAll()
        {
            return _context.Producers.ToList();
        }

        public ProducerEntity GetById(int id)
        {
            return _context.Producers.Find(id);
        }

        public void Insert(ProducerEntity producer)
        {
            _context.Producers.Add(producer);
        }

        public void Update(ProducerEntity producer)
        {
            _context.Entry(producer).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProducerEntity producer = _context.Producers.Find(id);
            if (producer != null)
            {
                _context.Producers.Remove(producer);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
