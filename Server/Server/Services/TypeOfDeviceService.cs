using AutoMapper;
using Client.Contracts;
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
    }
}
