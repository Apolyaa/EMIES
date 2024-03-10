using AutoMapper;
using Client.Contracts;
using Server.Repositories;

namespace Server.Services
{
    public class SynonymService : ISynonymService
    {
        private readonly ISynonymRepository _synonymRepository;
        protected readonly IMapper _mapper;

        public SynonymService(ISynonymRepository synonymRepository, IMapper mapper)
        {
            _synonymRepository = synonymRepository;
            _mapper = mapper;
        }

        public Response<List<SynonymDto>> GetSynonymsByCharacteristicId(Guid characteristicId)
        {
            Response<List<SynonymDto>> response = new();
            response.Data = new();
            try
            {
                var synonyms = _synonymRepository.GetAll();
                foreach (var synonym in synonyms)
                {
                    if (synonym.CharacteristicId == characteristicId)
                        response.Data.Add(_mapper.Map<SynonymDto>(synonym));
                }
                return response;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Get synonyms failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при получении синонимов.";
                return response;
            }
        }
    }
}
