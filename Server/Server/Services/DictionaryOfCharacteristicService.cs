using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class DictionaryOfCharacteristicService : IDictionaryOfCharacteristicService
    {
        private readonly IDictionaryOfCharacteristicRepository _dictionaryOfCharacteristicRepository;
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        protected readonly IMapper _mapper;

        public DictionaryOfCharacteristicService(
            IDictionaryOfCharacteristicRepository dictionaryOfCharacteristic, 
            IMapper mapper,
            IUnitOfMeasurementService unitOfMeasurementService)
        {
            _dictionaryOfCharacteristicRepository = dictionaryOfCharacteristic;
            _mapper = mapper;
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
        public Response<bool> AddCharacteristic(DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto)
        {
            Response<bool> response = new();
            try
            {
                var existCharacteristic = _dictionaryOfCharacteristicRepository.GetAll().
                    FirstOrDefault(s => s.Name.ToLower() == dictionaryOfCharacteristicDto.Name.ToLower());
                if (existCharacteristic is not null)
                {
                    response.Message = "Характеристика с таким названием уже существует.";
                    throw new Exception("Characteristic is already exist");
                }

                var dictionaryOfCharacteristicEntity = _mapper.Map<DictionaryOfCharacteristicEntity>(dictionaryOfCharacteristicDto);
                dictionaryOfCharacteristicEntity.Synonyms = dictionaryOfCharacteristicDto.Synonyms.ToArray();

                _dictionaryOfCharacteristicRepository.Insert(dictionaryOfCharacteristicEntity);
                _dictionaryOfCharacteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                if (string.IsNullOrEmpty(response.Message))
                    response.Message = "Ошибка при добавлении характеристики.";
                return response;
            }
        }
        public Response<bool> DeleteCharacteristic(Guid characteristicId)
        {
            Response<bool> response = new();
            try
            {
                _dictionaryOfCharacteristicRepository.Delete(characteristicId);
                _dictionaryOfCharacteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Remove characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении характеристики.";
                return response;
            }
        }
        public Response<bool> UpdateCharacteristic(DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto)
        {
            Response<bool> response = new();
            try
            {
                var existCharacteristic = _dictionaryOfCharacteristicRepository.GetAll().
                    FirstOrDefault(s => s.Name.ToLower() == dictionaryOfCharacteristicDto.Name.ToLower() &&
                    s.Id != dictionaryOfCharacteristicDto.Id);
                if (existCharacteristic is not null)
                {
                    response.Message = "Характеристика с таким названием уже существует.";
                    throw new Exception("Characteristic is already exist");
                }

                var dictionaryOfCharacteristicEntity = _mapper.Map<DictionaryOfCharacteristicEntity>(dictionaryOfCharacteristicDto);
                dictionaryOfCharacteristicEntity.Synonyms = dictionaryOfCharacteristicDto.Synonyms.ToArray();
                _dictionaryOfCharacteristicRepository.Update(dictionaryOfCharacteristicEntity);
                _dictionaryOfCharacteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                if (string.IsNullOrEmpty(response.Message))
                    response.Message = "Ошибка при изменении характеристики.";
                return response;
            }
        }
    }
}
