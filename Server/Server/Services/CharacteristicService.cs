using Client.Contracts;
using Microsoft.AspNetCore.Server.IIS.Core;
using Server.EfCore.Model;
using Server.Repositories;
using System.Text;

namespace Server.Services
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;

        public CharacteristicService(ICharacteristicRepository characteristicRepository, 
            IUnitOfMeasurementService unitOfMeasurementService) 
        {
            _characteristicRepository = characteristicRepository;
            _unitOfMeasurementService = unitOfMeasurementService;
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
                            if (GetValueInCI(double.Parse(characteristicFind.Value), characteristicFind.UnitOfMeasurement) == characteristicForCompare.NumberValue + double.Epsilon)
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
                                minValue, maxValue, characteristicFind.UnitOfMeasurement))
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
                                minValue, null, characteristicFind.UnitOfMeasurement))
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
                                null, maxValue, characteristicFind.UnitOfMeasurement))
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
        private bool CompareRange(double minValueSource, double maxValueSource, double? minValue, double? maxValue, string unitName)
        {
            bool notSuitable = true;
            if (minValue is not null)
            {
                if (minValueSource > GetValueInCI(minValue.Value, unitName))
                    notSuitable = false;
            }
            if (maxValue is not null)
            {
                if (maxValueSource < GetValueInCI(maxValue.Value, unitName))
                    notSuitable = false;
            }
            return notSuitable;
        }
        private double GetValueInCI(double value, string unitName)
        {
            var response = _unitOfMeasurementService.GetUnitByName(unitName);
            if (!response.Success)
                throw new Exception(response.Message);
            var valueInCI = value * response.Data.MultiplicationFactor;
            return valueInCI;
        }
    }
}
