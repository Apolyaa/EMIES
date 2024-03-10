using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface ISynonymRepository
    {
        IEnumerable<SynonymEntity> GetAll();
        SynonymEntity GetById(int id);
        void Insert(SynonymEntity synonym);
        void Update(SynonymEntity synonym);
        void Delete(int id);
        void Save();
    }
}
