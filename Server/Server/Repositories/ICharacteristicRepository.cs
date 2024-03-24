using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface ICharacteristicRepository
    {
        IEnumerable<CharacteristicEntity> GetAll();
        CharacteristicEntity GetById(int id);
        void Insert(CharacteristicEntity characteristic);
        void Update(CharacteristicEntity characteristic);
        void Delete(Guid id);
        void Save();
    }
}
