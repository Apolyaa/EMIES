using Client.Contracts;

namespace Server.Services
{
    public interface IResultService
    {
        Response<ResultDto> FindDevices(RequestFind request);
    }
}
