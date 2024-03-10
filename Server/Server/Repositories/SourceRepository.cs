using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class SourceRepository : ISourceRepository
    {
        private readonly ApplicationDbContext _context;

        public SourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SourceEntity> GetAll()
        {
            return _context.Sources.ToList();
        }

        public SourceEntity GetById(int id)
        {
            return _context.Sources.Find(id);
        }

        public void Insert(SourceEntity source)
        {
            _context.Sources.Add(source);
        }

        public void Update(SourceEntity source)
        {
            _context.Entry(source).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            SourceEntity source = _context.Sources.Find(id);
            if (source != null)
            {
                _context.Sources.Remove(source);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
