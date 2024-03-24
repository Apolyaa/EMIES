using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class TypeOfDeviceService : ITypeOfDeviceService
    {
        private readonly ITypeOfDeviceRepository _typeOfDeviceRepository;
        private readonly ITypeCharacteristicService _characteristicService;
        protected readonly IMapper _mapper;

        public TypeOfDeviceService(ITypeOfDeviceRepository typeOfDeviceRepository,
            IMapper mapper,
            ITypeCharacteristicService typeCharacteristicService)
        {
            _typeOfDeviceRepository = typeOfDeviceRepository;
            _mapper = mapper;
            _characteristicService = typeCharacteristicService;
        }
        public Response<List<TypeOfDeviceDto>> GetTypes()
        {
            Response<List<TypeOfDeviceDto>> response = new();
            try
            {
                var types = _typeOfDeviceRepository.GetAll();
                response.Data = new();
                foreach (var type in types)
                {
                    var typeDevice = _mapper.Map<TypeOfDeviceDto>(type);

                    var responseCharacteristics = _characteristicService.GetMainCharacteristicByTypeId(type.Id);
                    if (!responseCharacteristics.Success)
                        throw new Exception("Error get main characteristics for type of devices.");
                    typeDevice.MainCharacteristics = responseCharacteristics.Data!;

                    response.Data.Add(typeDevice);
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get types failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении типов.";
                return response;
            }
        }
        public Response<bool> AddType(TypeOfDeviceDto typeOfDeviceDto)
        {
            Response<bool> response = new();
            try
            {
                _typeOfDeviceRepository.Insert(_mapper.Map<TypeOfDevicesEntity>(typeOfDeviceDto));
                _typeOfDeviceRepository.Save();
                var addResponse = _characteristicService.AddTypesCharacterictics(typeOfDeviceDto.MainCharacteristics,
                    typeOfDeviceDto.Id);
                if (!addResponse.Success)
                    throw new Exception(addResponse.Message);
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add type failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении типа.";
                return response;
            }
        }
        public Response<bool> UpdateType(TypeOfDeviceDto typeOfDeviceDto)
        {
            Response<bool> response = new();
            try
            {
                _typeOfDeviceRepository.Update(_mapper.Map<TypeOfDevicesEntity>(typeOfDeviceDto));
                _typeOfDeviceRepository.Save();
                var updateResponse = _characteristicService.ChangeTypesCharacteristics(typeOfDeviceDto.MainCharacteristics,
                    typeOfDeviceDto.Id);
                if (!updateResponse.Success)
                    throw new Exception(updateResponse.Message);
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update type failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при изменении типа.";
                return response;
            }
        }
        public Response<bool> DeleteType(Guid typeId)
        {
            Response<bool> response = new();
            try
            {
                _typeOfDeviceRepository.Delete(typeId);
                _typeOfDeviceRepository.Save();
                var deleteResponse = _characteristicService.DeleteTypesCharacteristics(typeId);
                if (!deleteResponse.Success)
                    throw new Exception(deleteResponse.Message);

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete type failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении типа.";
                return response;

            }
        }
    }
}
