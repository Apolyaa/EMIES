using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
        protected readonly IMapper _mapper;

        public UnitOfMeasurementService(IUnitOfMeasurementRepository unitOfMeasurementRepository, IMapper mapper)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _mapper = mapper;
        }

        public Response<List<UnitOfMeasurementDto>> GetUnitsOfMeasurement()
        {
            Response<List<UnitOfMeasurementDto>> response = new();
            response.Data = new();
            try
            {
                var units = _unitOfMeasurementRepository.GetAll();
                foreach (var unit in units)
                {
                   response.Data.Add(_mapper.Map<UnitOfMeasurementDto>(unit));
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
        public Response<UnitOfMeasurementDto> GetUnitByName(string name)
        {
            Response<UnitOfMeasurementDto> response = new();
            try
            {
                var unit = _unitOfMeasurementRepository.GetAll().FirstOrDefault(u => u.Name == name);
                response.Data = _mapper.Map<UnitOfMeasurementDto>(unit);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get unit by name failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении единицы измерения по названию.";
                return response;
            }
        }
        public Response<bool> AddUnit(UnitOfMeasurementDto unitOfMesurementDto)
        {
            Response<bool> response = new();
            try
            {
                var producerEntity = _mapper.Map<UnitOfMeasurementEntity>(unitOfMesurementDto);

                _unitOfMeasurementRepository.Insert(producerEntity);
                _unitOfMeasurementRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add unit failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении единицы измерения.";
                return response;
            }
        }
        public Response<bool> DeleteUnit(Guid id)
        {
            Response<bool> response = new();
            try
            {
                _unitOfMeasurementRepository.Delete(id);
                _unitOfMeasurementRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Remove unit failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении единицы измерения.";
                return response;
            }
        }
        public Response<bool> UpdateUnit(UnitOfMeasurementDto unitOfMesurementDto)
        {
            Response<bool> response = new();
            try
            {
                var unitEntity = _mapper.Map<UnitOfMeasurementEntity>(unitOfMesurementDto);
                _unitOfMeasurementRepository.Update(unitEntity);
                _unitOfMeasurementRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update unit failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при изменении единицы измерения.";
                return response;
            }
        }
    }
}
