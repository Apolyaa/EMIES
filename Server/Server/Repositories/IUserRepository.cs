using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetAll();
        UserEntity GetById(int id);
        void Insert(UserEntity user);
        void Update(UserEntity user);
        void Delete(Guid id);
        void Save();
    }
}
