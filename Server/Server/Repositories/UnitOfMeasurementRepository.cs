using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;
using Server.Model;

namespace Server.Repositories
{
    public class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasurementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UnitOfMeasurementEntity> GetAll()
        {
            return _context.UnitOfMeasurements.ToList();
        }

        public UnitOfMeasurementEntity GetById(int id)
        {
            return _context.UnitOfMeasurements.Find(id);
        }

        public void Insert(UnitOfMeasurementEntity unitOfMeasurement)
        {
            _context.UnitOfMeasurements.Add(unitOfMeasurement);
        }

        public void Update(UnitOfMeasurementEntity unitOfMeasurement)
        {
            Delete(unitOfMeasurement.Id);
            Insert(unitOfMeasurement);
        }

        public void Delete(Guid id)
        {
            UnitOfMeasurementEntity unitOfMeasurement = _context.UnitOfMeasurements.Find(id);
            if (unitOfMeasurement != null)
            {
                _context.UnitOfMeasurements.Remove(unitOfMeasurement);
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
