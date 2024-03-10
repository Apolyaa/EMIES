using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IDictionaryOfCharacteristicRepository
    {
        IEnumerable<DictionaryOfCharacteristicEntity> GetAll();
        DictionaryOfCharacteristicEntity GetById(int id);
        void Insert(DictionaryOfCharacteristicEntity dictionaryOfCharacteristic);
        void Update(DictionaryOfCharacteristicEntity dictionaryOfCharacteristic);
        void Delete(int id);
        void Save();
    }
}
