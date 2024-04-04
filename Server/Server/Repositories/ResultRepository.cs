using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;
using Server.Services;

namespace Server.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly ApplicationDbContext _context;

        public ResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ResultEntity> GetAll()
        {
            return _context.Results.ToList();
        }

        public ResultEntity GetById(int id)
        {
            return _context.Results.Find(id);
        }

        public void Insert(ResultEntity result)
        {
            _context.Results.Add(result);
        }

        public void Update(ResultEntity result)
        {
            Delete(result.Id);
            Save();
            Insert(result);
        }

        public void Delete(Guid id)
        {
            ResultEntity result = _context.Results.Find(id);
            if (result != null)
            {
                _context.Results.Remove(result);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
