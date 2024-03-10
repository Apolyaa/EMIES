using AutoMapper;
using Client.Contracts;
using Server.Repositories;

namespace Server.Services
{
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly IUnitOfMeasurementRepository _unitOfMeasurementService;
        protected readonly IMapper _mapper;

        public UnitOfMeasurementService(IUnitOfMeasurementRepository unitOfMeasurementService, IMapper mapper)
        {
            _unitOfMeasurementService = unitOfMeasurementService;
            _mapper = mapper;
        }

        public Response<List<UnitOfMesurementDto>> GetUnitsOfMeasurement()
        {
            Response<List<UnitOfMesurementDto>> response = new();
            response.Data = new();
            try
            {
                var units = _unitOfMeasurementService.GetAll();
                foreach (var unit in units)
                {
                   response.Data.Add(_mapper.Map<UnitOfMesurementDto>(unit));
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get units failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении единиц измерения.";
                return response;
            }
        }
    }
}
