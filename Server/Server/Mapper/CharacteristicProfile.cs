using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class CharacteristicProfile : Profile
    {
        public CharacteristicProfile()
        {
            CreateMap<CharacteristicEntity, CharacteristicDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type));
            CreateMap<CharacteristicDto, CharacteristicEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
