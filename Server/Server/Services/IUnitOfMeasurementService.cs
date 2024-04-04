using Client.Contracts;

namespace Server.Services
{
    public interface IUnitOfMeasurementService
    {
        Response<List<UnitOfMeasurementDto>> GetUnitsOfMeasurement();
        Response<UnitOfMeasurementDto> GetUnitByName(string name);
        Response<bool> AddUnit(UnitOfMeasurementDto unitOfMesurementDto);
        Response<bool> DeleteUnit(Guid id);
        Response<bool> UpdateUnit(UnitOfMeasurementDto unitOfMesurementDto);
    }
}
