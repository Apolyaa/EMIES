using AutoMapper;
using Client.Contracts;
using Server.Repositories;

namespace Server.Services
{
    public class TypeCharacteristicService : ITypeCharacteristicService
    {
        private readonly ITypeCharacteristicRepository _typeCharacteristicRepository;
        private readonly IDictionaryOfCharacteristicService _dictionaryOfCharacteristicService;
        protected readonly IMapper _mapper;

        public TypeCharacteristicService(ITypeCharacteristicRepository typeCharacteristicRepository,
            IMapper mapper,
            IDictionaryOfCharacteristicService dictionaryOfCharacteristicService)
        {
            _typeCharacteristicRepository = typeCharacteristicRepository;
            _mapper = mapper;
            _dictionaryOfCharacteristicService = dictionaryOfCharacteristicService;
        }

        public Response<List<DictionaryOfCharacteristicDto>> GetMainCharacteristicByTypeId(Guid typeId)
        {
            Response<List<DictionaryOfCharacteristicDto>> response = new();

            try
            {
                var mainCharacteristics = _typeCharacteristicRepository.GetAll();
                var characteristicsGuids = new HashSet<Guid>();
                foreach (var characteristic in mainCharacteristics)
                {
                    if (characteristic.TypeId == typeId)
                    {
                        var typeCharacteristicDto = _mapper.Map<TypeCharacteristicDto>(characteristic);
                        characteristicsGuids.Add(characteristic.CharacteristicId);
                    }       
                }

                var responseCharacteristics = _dictionaryOfCharacteristicService.GetMainCharacteristicsByCharacteristicId(characteristicsGuids);
                if (!responseCharacteristics.Success)
                    throw new Exception("Error get main characteristics for type of devices.");
                response.Data = responseCharacteristics.Data!;

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get type characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении основных характеристик для типа приборов.";
                return response;
            }
        }
    }
}
