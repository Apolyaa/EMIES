using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IResultRepository
    {
        IEnumerable<ResultEntity> GetAll();
        ResultEntity GetById(int id);
        void Insert(ResultEntity result);
        void Update(ResultEntity result);
        void Delete(int id);
        void Save();
    }
}
