using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _context.Users.ToList();
        }

        public UserEntity GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Insert(UserEntity user)
        {
            _context.Users.Add(user);
        }

        public void Update(UserEntity user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            UserEntity user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
