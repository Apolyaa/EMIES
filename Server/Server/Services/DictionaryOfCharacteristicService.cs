using AutoMapper;
using Client.Contracts;
using Server.Repositories;

namespace Server.Services
{
    public class DictionaryOfCharacteristicService : IDictionaryOfCharacteristicService
    {
        private readonly IDictionaryOfCharacteristicRepository _dictionaryOfCharacteristicRepository;
        private readonly ISynonymService _synonymService;
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        protected readonly IMapper _mapper;

        public DictionaryOfCharacteristicService(
            IDictionaryOfCharacteristicRepository dictionaryOfCharacteristic, 
            IMapper mapper,
            ISynonymService synonymService,
            IUnitOfMeasurementService unitOfMeasurementService)
        {
            _dictionaryOfCharacteristicRepository = dictionaryOfCharacteristic;
            _mapper = mapper;
            _synonymService = synonymService;
            _unitOfMeasurementService = unitOfMeasurementService;
        }

        public Response<List<DictionaryOfCharacteristicDto>> GetMainCharacteristicsByCharacteristicId(HashSet<Guid> characteristicsId)
        {
            Response<List<DictionaryOfCharacteristicDto>> response = new();
            response.Data = new();
            try
            {
                var characteristics = _dictionaryOfCharacteristicRepository.GetAll();
                foreach (var characteristic in characteristics)
                {
                    if (characteristicsId.Contains(characteristic.Id))
                    {
                        var charact = _mapper.Map<DictionaryOfCharacteristicDto>(characteristic);

                        var responseSynonyms = _synonymService.GetSynonymsByCharacteristicId(characteristic.Id);
                        if (!responseSynonyms.Success)
                            throw new Exception("Error get synonyms for characteristic");
                        charact.Synonyms = responseSynonyms.Data!;

                        response.Data.Add(charact);
                    }
                        
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get main characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении основных характеристик для типа приборов.";
                return response;
            }
        }

        public Response<List<DictionaryOfCharacteristicDto>> GetCharacteristics()
        {
            Response<List<DictionaryOfCharacteristicDto>> response = new();
            response.Data = new();
            try
            {
                var characteristics = _dictionaryOfCharacteristicRepository.GetAll();
                foreach (var characteristic in characteristics)
                {
                    var charact = _mapper.Map<DictionaryOfCharacteristicDto>(characteristic);

                    var responseSynonyms = _synonymService.GetSynonymsByCharacteristicId(characteristic.Id);
                    if (!responseSynonyms.Success)
                        throw new Exception("Error get synonyms for characteristic");
                    charact.Synonyms = responseSynonyms.Data!;

                    response.Data.Add(charact);
                }
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Get characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении характеристик.";
                return response;
            }
        }
    }
}
