using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly IMapper _mapper;

        public SourceService(ISourceRepository repository, IMapper mapper)
        {
            _sourceRepository = repository;
            _mapper = mapper;
        }
        public Response<SourceDto> GetSourceById(Guid id)
        {
            Response<SourceDto> response = new();
            try
            {
                var source = _sourceRepository.GetAll().FirstOrDefault(s => s.Id == id);
                response.Data = _mapper.Map<SourceDto>(source);
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
        public Response<List<SourceDto>> GetSources()
        {
            Response<List<SourceDto>> response = new();
            response.Data = new();
            try
            {
                var sources = _sourceRepository.GetAll();
                foreach (var source in sources)
                {
                    var sourceDto = _mapper.Map<SourceDto>(source);

                    response.Data.Add(sourceDto);
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get sources failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении источников.";
                return response;
            }
        }
        public Response<bool> AddSource(SourceDto source)
        {
            Response<bool> response = new();
            try
            {
                var sourceEntity = _mapper.Map<SourceEntity>(source);

                _sourceRepository.Insert(sourceEntity);
                _sourceRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add source failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при добавлении источника.";
                return response;
            }
        }
        public Response<bool> DeleteSource(Guid id)
        {
            Response<bool> response = new();
            try
            {
                _sourceRepository.Delete(id);
                _sourceRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Remove source failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при удалении источника.";
                return response;
            }
        }
        public Response<bool> UpdateSource(SourceDto sourceDto)
        {
            Response<bool> response = new();
            try
            {
                var sourceEntity = _mapper.Map<SourceEntity>(sourceDto);
                _sourceRepository.Update(sourceEntity);
                _sourceRepository.Save();
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update source failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при изменении источника.";
                return response;
            }
        }
    }
}
