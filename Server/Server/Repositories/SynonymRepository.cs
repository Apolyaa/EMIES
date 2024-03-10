using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class SynonymRepository : ISynonymRepository
    {
        private readonly ApplicationDbContext _context;

        public SynonymRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SynonymEntity> GetAll()
        {
            return _context.Synonyms.ToList();
        }

        public SynonymEntity GetById(int id)
        {
            return _context.Synonyms.Find(id);
        }

        public void Insert(SynonymEntity synonym)
        {
            _context.Synonyms.Add(synonym);
        }

        public void Update(SynonymEntity synonym)
        {
            _context.Entry(synonym).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            SynonymEntity synonym = _context.Synonyms.Find(id);
            if (synonym != null)
            {
                _context.Synonyms.Remove(synonym);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
