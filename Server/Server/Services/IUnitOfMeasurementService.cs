using Client.Contracts;

namespace Server.Services
{
    public interface IUnitOfMeasurementService
    {
        Response<List<UnitOfMesurementDto>> GetUnitsOfMeasurement();
        Response<UnitOfMesurementDto> GetUnitByName(string name);
        Response<bool> AddUnit(UnitOfMesurementDto unitOfMesurementDto);
        Response<bool> DeleteUnit(Guid id);
        Response<bool> UpdateUnit(UnitOfMesurementDto unitOfMesurementDto);
    }
}
