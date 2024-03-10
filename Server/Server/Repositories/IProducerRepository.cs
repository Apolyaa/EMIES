using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IProducerRepository
    {
        IEnumerable<ProducerEntity> GetAll();
        ProducerEntity GetById(int id);
        void Insert(ProducerEntity producer);
        void Update(ProducerEntity producer);
        void Delete(int id);
        void Save();
    }
}
