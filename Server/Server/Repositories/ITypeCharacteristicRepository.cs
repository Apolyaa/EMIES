using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface ITypeCharacteristicRepository
    {
        IEnumerable<TypeCharacteristicEntity> GetAll();
        TypeCharacteristicEntity GetById(int id);
        void Insert(TypeCharacteristicEntity typeCharacteristic);
        void Update(TypeCharacteristicEntity typeCharacteristic);
        void Delete(int id);
        void Save();
    }
}
