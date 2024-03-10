using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class SynonymProfile : Profile
    {
        public SynonymProfile()
        {
            CreateMap<SynonymEntity, SynonymDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.CharacteristicId, opt => opt.MapFrom(src => src.CharacteristicId));
            CreateMap<SynonymDto, SynonymEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.CharacteristicId, opt => opt.MapFrom(src => src.CharacteristicId));
        }
    }
}
