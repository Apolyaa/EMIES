using Client.Contracts;

namespace Server.Services
{
    public interface IUnitOfMeasurementService
    {
        Response<List<UnitOfMesurementDto>> GetUnitsOfMeasurement();
    }
}
