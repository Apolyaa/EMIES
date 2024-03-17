using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;
        public ProducerService(IProducerRepository producerRepository, IMapper mapper)
        {
            _producerRepository = producerRepository;
            _mapper = mapper;
        }
        public Response<ProducerDto> GetProducerById(Guid id)
        {
            Response<ProducerDto> response = new();
            try
            {
                var producer = _producerRepository.GetAll().FirstOrDefault(s => s.Id == id);
                response.Data = _mapper.Map<ProducerDto>(producer);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get source failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении источника.";
                return response;
            }
        }
        public Response<List<ProducerDto>> GetProducers()
        {
            Response<List<ProducerDto>> response = new();
            response.Data = new();
            try
            {
                var sources = _producerRepository.GetAll();
                foreach (var source in sources)
                {
                    var sourceDto = _mapper.Map<ProducerDto>(source);

                    response.Data.Add(sourceDto);
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get producers failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении производителей.";
                return response;
            }
        }
        public Response<bool> AddProducer(ProducerDto producerDto)
        {
            Response<bool> response = new();
            try
            {
                var producerEntity = _mapper.Map<ProducerEntity>(producerDto);

                _producerRepository.Insert(producerEntity);
                _producerRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add producer failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении производителя.";
                return response;
            }
        }
        public Response<bool> DeleteProducer(Guid id)
        {
            Response<bool> response = new();
            try
            {
                _producerRepository.Delete(id);
                _producerRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Remove producer failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении производителя.";
                return response;
            }
        }
        public Response<bool> UpdateProducer(ProducerDto producerDto)
        {
            Response<bool> response = new();
            try
            {
                var producerEntity = _mapper.Map<ProducerEntity>(producerDto);
                _producerRepository.Update(producerEntity);
                _producerRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update producer failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при изменении производителя.";
                return response;
            }
        }
    }
}
