using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
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
        public Response<bool> ChangeTypesCharacteristics(List<DictionaryOfCharacteristicDto> characteristics, Guid typeId)
        {
            Response<bool> response = new();
            try
            {
                var existTypesCharacteristics = _typeCharacteristicRepository.GetAll().Where(t => t.TypeId == typeId);
                if (!existTypesCharacteristics.Any() || existTypesCharacteristics is null)
                {
                    foreach (var characteristic in characteristics)
                        _typeCharacteristicRepository.Insert(new TypeCharacteristicEntity()
                        {
                            Id = Guid.NewGuid(),
                            CharacteristicId = characteristic.Id,
                            TypeId = typeId
                        });
                    _typeCharacteristicRepository.Save();
                    response.Data = true;
                    return response;
                }
                var newSet = characteristics.Select(t => t.Id).ToHashSet();
                var deleteTypesCharacteristics = existTypesCharacteristics.Where(t => !newSet.Contains(t.CharacteristicId));
                if (deleteTypesCharacteristics.Any() || deleteTypesCharacteristics is not null)
                {
                    foreach(var deleteTypeCharacteristic in deleteTypesCharacteristics)
                    {
                        var deleteId = existTypesCharacteristics
                            .FirstOrDefault(t => t.CharacteristicId == deleteTypeCharacteristic.CharacteristicId)?.Id;
                        if (deleteId is not null)
                            _typeCharacteristicRepository.Delete(deleteId.Value);
                    }
                    _typeCharacteristicRepository.Save();
                }
                var existSet = existTypesCharacteristics.Select(t => t.CharacteristicId).ToHashSet();
                var addCharacteristics = characteristics.Where(t => !existSet.Contains(t.Id));
                if (addCharacteristics.Any() || addCharacteristics is not null)
                {
                    foreach (var addCharacteristic in addCharacteristics)
                        _typeCharacteristicRepository.Insert(new TypeCharacteristicEntity()
                        {
                            Id = Guid.NewGuid(),
                            CharacteristicId = addCharacteristic.Id,
                            TypeId = typeId
                        });
                    _typeCharacteristicRepository.Save();
                }
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change type characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при изменении связей между типами и основными характеристиками.";
                return response;
            }
        }
        public Response<bool> AddTypesCharacterictics(List<DictionaryOfCharacteristicDto> characteristics, Guid typeId)
        {
            Response<bool> response = new();
            try
            {
                foreach (var characteristic in characteristics)
                    _typeCharacteristicRepository.Insert(new TypeCharacteristicEntity()
                    {
                        Id = Guid.NewGuid(),
                        CharacteristicId = characteristic.Id,
                        TypeId = typeId
                    });
                _typeCharacteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add type characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении связей между типами и основными характеристиками.";
                return response;
            }
        }
        public Response<bool> DeleteTypesCharacteristics(Guid typeId)
        {
            Response<bool> response = new();
            try
            {
                var deleteTypesCharacteristics = _typeCharacteristicRepository.GetAll().Where(t => t.TypeId == typeId);
                foreach(var deleteCharacteristic in deleteTypesCharacteristics)
                    _typeCharacteristicRepository.Delete(deleteCharacteristic.Id);
                _typeCharacteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete type characteristic failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении связей между типами и основными характеристиками.";
                return response;
            }
        }
    }
}
