using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IProducerRepository
    {
        IEnumerable<ProducerEntity> GetAll();
        ProducerEntity GetById(int id);
        void Insert(ProducerEntity producer);
        void Update(ProducerEntity producer);
        void Delete(Guid id);
        void Save();
    }
}
