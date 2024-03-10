using Server.EfCore.Model;

namespace Server.Repositories
{
    public interface IUnitOfMeasurementRepository
    {
        IEnumerable<UnitOfMeasurementEntity> GetAll();
        UnitOfMeasurementEntity GetById(int id);
        void Insert(UnitOfMeasurementEntity unitOfMeasurement);
        void Update(UnitOfMeasurementEntity unitOfMeasurement);
        void Delete(int id);
        void Save();
    }
}
