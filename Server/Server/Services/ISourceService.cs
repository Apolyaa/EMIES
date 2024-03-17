using Client.Contracts;

namespace Server.Services
{
    public interface ISourceService
    {
        Response<SourceDto> GetSourceById(Guid id);
        Response<List<SourceDto>> GetSources();
        Response<bool> AddSource(SourceDto source);
        Response<bool> DeleteSource(Guid id);
        Response<bool> UpdateSource(SourceDto sourceDto);
    }
}
