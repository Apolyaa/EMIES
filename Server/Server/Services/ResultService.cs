using Client.Contracts;
using Server.Repositories;

namespace Server.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IDeviceService _deviceService;

        public ResultService(IResultRepository resultRepository, IDeviceService deviceService)
        {
            _resultRepository = resultRepository;
            _deviceService = deviceService;
        }
        public Response<ResultDto> FindDevices(RequestFind request)
        {
            Response<ResultDto> response = new();
            try
            {
                var responseResult = _deviceService.GetSuitableDevices(request.TypeId, request.Characteristics);
                if (!responseResult.Success)
                    throw new Exception(responseResult.Message);
                response.Data = responseResult.Data;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get result failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении результата подбора приборов.";
                return response;
            }
        }
    }
}
