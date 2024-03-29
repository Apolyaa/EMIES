﻿using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;
using System.Data;

namespace Server.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICharacteristicService _characteristicService;
        private readonly ISourceService _sourceService;
        private readonly IProducerService _producerService;
        private readonly IMapper _mapper;
        public DeviceService(IDeviceRepository deviceRepository, ICharacteristicService characteristicService,
            ISourceService sourceService, IProducerService producerService, IMapper mapper)
        {
            _deviceRepository = deviceRepository;
            _characteristicService = characteristicService;
            _sourceService = sourceService;
            _producerService = producerService;
            _mapper = mapper;
        }
        public Response<ResultDto> GetSuitableDevices(Guid typeId, List<CharacteristicForFindDto> characteristicForFinds)
        {
            Response<ResultDto> response = new();
            try
            {
                var devices = _deviceRepository.GetAll().Where(d => d.TypeId == typeId);
                List<DeviceDto> suitableDevices = new();
                List<ResultDeviceDto> resultDevices = new();
                foreach (var device in devices)
                {
                    var responseResultCompareCharacteristics = _characteristicService
                        .CompareCharacteristicsOfDevice(device.Id, characteristicForFinds);
                    if (!responseResultCompareCharacteristics.Success)
                        throw new Exception(responseResultCompareCharacteristics.Message);
                    var percentEssential = responseResultCompareCharacteristics.Data.CountSuitableEssential * 
                        100.0 / characteristicForFinds.Count;
                    var persentUnessential = responseResultCompareCharacteristics.Data.CountSuitableUnessential * 
                        100.0 / characteristicForFinds.Count;
                    ResultDeviceDto resultDeviceDto = new()
                    {
                        DeviceId = device.Id,
                        PercentEssential = percentEssential,
                        PercentUnessential = persentUnessential,
                        CharacteristicsResults = responseResultCompareCharacteristics.Data.Characteristics
                    };
                    resultDevices.Add(resultDeviceDto);

                    var responseSource = _sourceService.GetSourceById(device.SourceId);
                    if (!responseSource.Success)
                        throw new Exception(responseSource.Message);
                    var responseProducer = _producerService.GetProducerById(device.ProducerId);
                    if (!responseProducer.Success)
                        throw new Exception(responseProducer.Message);
                    var deviceDto = _mapper.Map<DeviceDto>(device);
                    deviceDto.Source = responseSource.Data;
                    deviceDto.Producer = responseProducer.Data;
                    deviceDto.Characteristics = responseResultCompareCharacteristics.Data.Characteristics;

                    suitableDevices.Add(deviceDto);
                }
                ResultDto resultDto = new()
                {
                    InitialData = characteristicForFinds,
                    ResultsDevices = resultDevices,
                    Devices = suitableDevices
                };
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get suitable devices failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении списка подобранных приборов.";
                return response;
            }
        }
        public Response<bool> AddDevice(DeviceDto deviceDto)
        {
            Response<bool> response = new();
            try
            {
                var deviceEntity = _mapper.Map<DeviceEntity>(deviceDto);
                deviceEntity.SourceId = deviceDto.Source.Id;
                deviceEntity.ProducerId = deviceDto.Producer.Id;
                deviceEntity.TypeId = deviceDto.Type.Id;
                _deviceRepository.Insert(deviceEntity);
                _deviceRepository.Save();
                //add characteristics
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add device failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении прибора.";
                return response;
            }
        }
        public Response<bool> DeleteDevice(Guid deviceId)
        {
            Response<bool> response = new();
            try
            {
                var existDevice = _deviceRepository.GetAll().FirstOrDefault(x => x.Id == deviceId);
                if (existDevice == null)
                    throw new Exception($"Not found device by id {deviceId}");
                _deviceRepository.Delete(deviceId);
                _deviceRepository.Save();
                //delete characteristics
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete device failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении прибора.";
                return response;
            }
        }
        public Response<bool> UpdateDevice(DeviceDto deviceDto)
        {
            Response<bool> response = new();
            try
            {
                var existDevice = _deviceRepository.GetAll().FirstOrDefault(x => x.Id == deviceDto.Id);
                if (existDevice is null)
                    throw new Exception($"Not found device by id {deviceDto.Id}");
                existDevice.Price = deviceDto.Price;
                existDevice.SourceId = deviceDto.Source.Id;
                existDevice.ProducerId = deviceDto.Producer.Id;
                existDevice.Url = deviceDto.Url;
                existDevice.TypeId = deviceDto.Type.Id;
                existDevice.Name = deviceDto.Name;
                _deviceRepository.Update(existDevice);
                _deviceRepository.Save();
                //update characteristics
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update device failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при обновлении прибора.";
                return response;
            }
        } 
    }
}
