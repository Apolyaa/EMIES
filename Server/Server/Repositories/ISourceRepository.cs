using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface ISourceRepository
    {
        IEnumerable<SourceEntity> GetAll();
        SourceEntity GetById(int id);
        void Insert(SourceEntity source);
        void Update(SourceEntity source);
        void Delete(Guid id);
        void Save();
    }
}
