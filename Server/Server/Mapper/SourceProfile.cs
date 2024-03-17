using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<SourceEntity, SourceDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<SourceDto, SourceEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
