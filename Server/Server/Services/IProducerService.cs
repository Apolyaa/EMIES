using Client.Contracts;

namespace Server.Services
{
    public interface IProducerService
    {
        Response<ProducerDto> GetProducerById(Guid id);
        Response<List<ProducerDto>> GetProducers();
        Response<bool> AddProducer(ProducerDto producerDto);
        Response<bool> DeleteProducer(Guid id);
        Response<bool> UpdateProducer(ProducerDto producerDto);
    }
}
