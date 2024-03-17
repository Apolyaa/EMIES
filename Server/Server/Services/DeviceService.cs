using AutoMapper;
using Client.Contracts;
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
    }
}
