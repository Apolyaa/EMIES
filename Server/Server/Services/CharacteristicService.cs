using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;
using System.Text;

namespace Server.Services
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
        private readonly IDictionaryOfCharacteristicRepository _dictionaryCharacteristicRepository;
        private readonly IMapper _mapper;

        public CharacteristicService(ICharacteristicRepository characteristicRepository, 
            IUnitOfMeasurementService unitOfMeasurementService,
            IMapper mapper,
            IDictionaryOfCharacteristicRepository dictionaryOfCharacteristicRepository,
            IUnitOfMeasurementRepository unitOfMeasurementRepository) 
        {
            _characteristicRepository = characteristicRepository;
            _unitOfMeasurementService = unitOfMeasurementService;
            _mapper = mapper;
            _dictionaryCharacteristicRepository = dictionaryOfCharacteristicRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
        }
        public Response<List<CharacteristicEntity>> GetCharacteristicsByDeviceId(Guid deviceId)
        {
            Response<List<CharacteristicEntity>> response = new();
            try
            {
                response.Data = _characteristicRepository.GetAll().Where(c => c.DeviceId == deviceId).ToList();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get characteristic by deviceId failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении характеристик прибора.";
                return response;
            }
        }
        public Response<ResultCompareCharacteristicsDto> CompareCharacteristicsOfDevice(Guid deviceId, 
            List<CharacteristicForFindDto> characteristicForFinds)
        {
            Response<ResultCompareCharacteristicsDto> response = new();
            response.Data = new();
            try
            {
                var responseCharacteristics = GetCharacteristicsByDeviceId(deviceId);
                if (!responseCharacteristics.Success)
                    throw new Exception("Get characteristics by deviceId failed.");
                var characteristicsOfDevice = responseCharacteristics.Data;

                foreach (var characteristicFind in characteristicForFinds)
                {
                    var characteristicForCompare = characteristicsOfDevice
                        .FirstOrDefault(c => c.Name == characteristicFind.Name);
                    if (characteristicForCompare is null)
                    {
                        response.Data.CountNotFoundCharacteristics++;
                        continue;
                    }
                    if (characteristicForCompare.Type == "Boolean")
                    {
                        var value = characteristicFind.Value.ToLower() == "да" ? true : false;
                        var characteristicDto = new CharacteristicDto()
                        {
                            Id = characteristicForCompare.Id,
                            Name = characteristicForCompare.Name,
                            Value = characteristicForCompare.BooleanValue ? "Да" : "Нет",
                            IsEssential = characteristicFind.IsEssential,
                            Type = characteristicForCompare.Type
                        };
                        if (value == characteristicForCompare.BooleanValue)
                        {
                            if (characteristicFind.IsEssential)
                                response.Data.CountSuitableEssential++;
                            else
                                response.Data.CountSuitableUnessential++;
                            characteristicDto.IsMatch = true;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                        characteristicDto.IsMatch = false;
                        response.Data.Characteristics.Add(characteristicDto);
                        continue;
                    }
                    if (characteristicForCompare.Type == "ArrayOfValues")
                    {
                        StringBuilder stringBuilder = new();
                        foreach (var characteristic in characteristicForCompare.ArrayOfValues)
                            stringBuilder.Append(characteristic + ", ");
                        var characteristicDto = new CharacteristicDto()
                        {
                            Id = characteristicForCompare.Id,
                            Name = characteristicForCompare.Name,
                            Value = stringBuilder.ToString().TrimEnd(' ').TrimEnd(','),
                            IsEssential = characteristicFind.IsEssential,
                            Type = characteristicForCompare.Type
                        };
                        if (characteristicFind.TypeCharacteristic == "String")
                        {
                            var value = characteristicForCompare.ArrayOfValues.FirstOrDefault(c => c == characteristicFind.Value);

                            if (value is not null)
                            {
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                                characteristicDto.IsMatch = true;
                                response.Data.Characteristics.Add(characteristicDto);
                                continue;
                            }
                            characteristicDto.IsMatch = false;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                        if (characteristicFind.TypeCharacteristic == "ArrayOfValues")
                        {
                            var values = characteristicFind.Value.Split(',');
                            var valuesCompare = characteristicForCompare.ArrayOfValues.ToHashSet();
                            bool notFound = false;
                            foreach (var value in values)
                            {
                                if (!valuesCompare.Contains(value))
                                    notFound = true;
                            }
                            if (!notFound)
                            {
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                                characteristicDto.IsMatch = true;
                                response.Data.Characteristics.Add(characteristicDto);
                            }
                            characteristicDto.IsMatch = false;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                    }
                    if (characteristicForCompare.Type == "String")
                    {
                        var characteristicDto = new CharacteristicDto()
                        {
                            Id = characteristicForCompare.Id,
                            Name = characteristicForCompare.Name,
                            Value = characteristicForCompare.StringValue,
                            IsEssential = characteristicFind.IsEssential,
                            Type = characteristicForCompare.Type
                        };
                        if (characteristicFind.Value == characteristicForCompare.StringValue)
                        {
                            if (characteristicFind.IsEssential)
                                response.Data.CountSuitableEssential++;
                            else
                                response.Data.CountSuitableUnessential++;
                            characteristicDto.IsMatch = true;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                        characteristicDto.IsMatch = false;
                        response.Data.Characteristics.Add(characteristicDto);
                        continue;
                    }
                    if (characteristicForCompare.Type == "Number")
                    {
                            if (GetValueInCI(double.Parse(characteristicFind.Value), 
                                characteristicFind.UnitOfMeasurement.Id) == GetValueInCI(
                                    characteristicForCompare.NumberValue, 
                                    characteristicForCompare.UnitOfMeasurementId) + double.Epsilon)
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                            continue;
                    }
                    if (characteristicForCompare.Type == "Range")
                    {
                        var characteristicDto = new CharacteristicDto()
                        {
                            Id = characteristicForCompare.Id,
                            Name = characteristicForCompare.Name,
                            Value = characteristicForCompare.MinValue.ToString() + " - " + 
                                characteristicForCompare.MaxValue.ToString(),
                            IsEssential = characteristicFind.IsEssential,
                            Type = characteristicForCompare.Type
                        };
                        double minValue;
                        double maxValue;
                        if (characteristicFind.Value.Contains('-'))
                        {
                            var values = characteristicFind.Value.Split('-');
                            minValue = double.Parse(values[0].Trim(' '));
                            maxValue = double.Parse(values[1].Trim(' '));
                            if (CompareRange(characteristicForCompare.MinValue, characteristicForCompare.MaxValue, 
                                minValue, maxValue, characteristicFind.UnitOfMeasurement.Id, characteristicForCompare.UnitOfMeasurementId))
                            {
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                                characteristicDto.IsMatch = true;
                                response.Data.Characteristics.Add(characteristicDto);
                                continue;
                            }
                            characteristicDto.IsMatch = false;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                        if (characteristicFind.Value.Contains('>'))
                        {
                            var value = characteristicFind.Value.Trim('>');
                            minValue = double.Parse(value.Trim(' '));
                            if (CompareRange(characteristicForCompare.MinValue, characteristicForCompare.MaxValue, 
                                minValue, null, characteristicFind.UnitOfMeasurement.Id, 
                                characteristicForCompare.UnitOfMeasurementId))
                            {
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                                characteristicDto.IsMatch = true;
                                response.Data.Characteristics.Add(characteristicDto);
                                continue;
                            }
                            characteristicDto.IsMatch = false;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                        if (characteristicFind.Value.Contains('<'))
                        {
                            var value = characteristicFind.Value.Trim('<');
                            maxValue = double.Parse(value.Trim(' '));
                            if (CompareRange(characteristicForCompare.MinValue, characteristicForCompare.MaxValue, 
                                null, maxValue, characteristicFind.UnitOfMeasurement.Id, 
                                characteristicForCompare.UnitOfMeasurementId))
                            {
                                if (characteristicFind.IsEssential)
                                    response.Data.CountSuitableEssential++;
                                else
                                    response.Data.CountSuitableUnessential++;
                                characteristicDto.IsMatch = true;
                                response.Data.Characteristics.Add(characteristicDto);
                                continue;
                            }
                            characteristicDto.IsMatch = false;
                            response.Data.Characteristics.Add(characteristicDto);
                            continue;
                        }
                    }
                }
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compare characteristics of device failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при сравнении характеристик прибора.";
                return response;
            }
        }
        private bool CompareRange(double minValueSource, double maxValueSource, double? minValue, double? maxValue, 
            Guid? unitId, Guid? unitIdSource)
        {
            bool notSuitable = true;
            if (minValue is not null)
            {
                if (GetValueInCI(minValueSource, unitIdSource) > GetValueInCI(minValue.Value, unitId))
                    notSuitable = false;
            }
            if (maxValue is not null)
            {
                if (GetValueInCI(maxValueSource, unitIdSource) < GetValueInCI(maxValue.Value, unitId))
                    notSuitable = false;
            }
            return notSuitable;
        }
        private double GetValueInCI(double value, Guid? unitId)
        {
            if (unitId is null)
                return value;
            var response = _unitOfMeasurementRepository.GetAll().FirstOrDefault(t => t.Id == unitId);
            if (response is null)
                throw new Exception($"Not found unit by id {unitId}");
            var valueInCI = value * response.MultiplicationFactor;
            return valueInCI;
        }
        public Response<bool> AddCharacteristics(List<CharacteristicDto> characteristicDtos, Guid deviceId)
        {
            Response<bool> response = new();
            try
            {
                var nullUnit = _unitOfMeasurementRepository.GetAll().FirstOrDefault(t => t.Name == "");
                if (nullUnit is null)
                {
                    nullUnit = new UnitOfMeasurementEntity() { Id = Guid.NewGuid(), Name = "", MultiplicationFactor = 0 };
                    _unitOfMeasurementRepository.Insert(nullUnit);
                    _unitOfMeasurementRepository.Save();
                }
                foreach (var characteristicDto in characteristicDtos)
                {
                    var characteristicEntity = _mapper.Map<CharacteristicEntity>(characteristicDto);
                    characteristicEntity.DeviceId = deviceId;
                    if (characteristicDto.Unit.Id != Guid.Empty)
                        characteristicEntity.UnitOfMeasurementId = characteristicDto.Unit.Id;
                    else
                        characteristicEntity.UnitOfMeasurementId = null;
                    characteristicEntity.CharacteristicId = characteristicDto.DictionaryOfCharacteristic.Id;
                    characteristicEntity = SetValueCharacteristic(characteristicEntity, characteristicDto);
                    _characteristicRepository.Insert(characteristicEntity);
                }
                _characteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при сравнении характеристик прибора.";
                return response;
            }
        }
        private CharacteristicEntity SetValueCharacteristic(CharacteristicEntity characteristicEntity, CharacteristicDto characteristicDto)
        {
            switch (characteristicDto.Type)
            {
                case TypeCharacteristicConstants.NUMBER:
                    {
                        characteristicEntity.NumberValue = double.Parse(characteristicDto.Value);
                        break;
                    }
                case TypeCharacteristicConstants.STRING:
                    {
                        characteristicEntity.StringValue = characteristicDto.Value;
                        break;
                    }
                case TypeCharacteristicConstants.BOOLEAN:
                    {
                        characteristicEntity.BooleanValue = characteristicDto.Value.ToLower() == "да" ? true : false;
                        break;
                    }
                case TypeCharacteristicConstants.ARRAYOFVALUES:
                    {
                        var arrayOfValues = characteristicDto.Value.Split(',');
                        for (var i = 0; i < arrayOfValues.Length; i++)
                        {
                            arrayOfValues[i] = arrayOfValues[i].Trim(' ');
                        }
                        characteristicEntity.ArrayOfValues = arrayOfValues;
                        break;
                    }
                case TypeCharacteristicConstants.RANGE:
                    {
                        if (characteristicDto.Value.Contains('-'))
                        {
                            var values = characteristicDto.Value.Split('-');
                            characteristicEntity.MinValue = double.Parse(values[0].Trim(' '));
                            characteristicEntity.MaxValue = double.Parse(values[1].Trim(' '));
                        }
                        break;
                    }
                }
            return characteristicEntity;
            }
        public Response<bool> UpdateCharacteristics(List<CharacteristicDto> newCharacteristicDtos, Guid deviceId)
        {
            Response<bool> response = new();
            try
            {
                var existCharacteristics = _characteristicRepository.GetAll().Where(t => t.DeviceId == deviceId);
                var newSet = newCharacteristicDtos.Select(t => t.DictionaryOfCharacteristic.Id).ToHashSet();
                var deleteCharacteristics = existCharacteristics.Where(t => !newSet.Contains(t.CharacteristicId));
                if (deleteCharacteristics.Any() || deleteCharacteristics is not null)
                {
                    foreach (var deleteCharacteristic in deleteCharacteristics)
                    {
                        var deleteId = existCharacteristics
                            .FirstOrDefault(t => t.CharacteristicId == deleteCharacteristic.CharacteristicId)?.Id;
                        if (deleteId is not null)
                            _characteristicRepository.Delete(deleteId.Value);
                    }
                    _characteristicRepository.Save();
                }
                var existSet = existCharacteristics.Select(t => t.CharacteristicId).ToHashSet();
                var addCharacteristics = newCharacteristicDtos.Where(t => !existSet.Contains(t.DictionaryOfCharacteristic.Id));
                if (addCharacteristics.Any() || addCharacteristics is not null)
                {
                    foreach (var addCharacteristic in addCharacteristics)
                    {
                        var characteristicEntity = _mapper.Map<CharacteristicEntity>(addCharacteristic);
                        characteristicEntity.DeviceId = deviceId;
                        characteristicEntity.UnitOfMeasurementId = addCharacteristic.Unit.Id;
                        characteristicEntity.CharacteristicId = addCharacteristic.DictionaryOfCharacteristic.Id;
                        characteristicEntity = SetValueCharacteristic(characteristicEntity, addCharacteristic);
                        _characteristicRepository.Insert(characteristicEntity);
                    }
                    _characteristicRepository.Save();
                }
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при обновлении характеристик прибора.";
                return response;
            }
        }
        public Response<bool> DeleteCharacteristics(Guid deviceId)
        {
            Response<bool> response = new();
            try
            {
                var deleteCharacteristics = _characteristicRepository.GetAll().Where(t => t.DeviceId == deviceId)
                    .Select(t => t.Id);
                foreach (var deleteCharacteristic in deleteCharacteristics)
                    _characteristicRepository.Delete(deleteCharacteristic);
                _characteristicRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении характеристик прибора.";
                return response;
            }
        }
        public Response<List<CharacteristicDto>> GetCharacteristics(Guid deviceId)
        {
            Response<List<CharacteristicDto>> response = new();
            response.Data = new();
            try
            {
                var characteristics = _characteristicRepository.GetAll().Where(t => t.DeviceId == deviceId);
                foreach(var characteristic in characteristics)
                {
                    var characteristicDto = _mapper.Map<CharacteristicDto>(characteristic);
                    characteristicDto.Unit = _mapper.Map<UnitOfMeasurementDto>(_unitOfMeasurementRepository.GetAll()
                        .FirstOrDefault(t => t.Id == characteristic.UnitOfMeasurementId));
                    characteristicDto.DictionaryOfCharacteristic = _mapper.Map<DictionaryOfCharacteristicDto>(_dictionaryCharacteristicRepository
                        .GetAll().FirstOrDefault(t => t.Id == characteristic.CharacteristicId));
                    response.Data.Add(characteristicDto);
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get characteristics failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении характеристик прибора.";
                return response;
            }
        }
    }
}
