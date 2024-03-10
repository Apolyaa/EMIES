using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class DictionaryOfCharacteristicProfile : Profile
    {
        public DictionaryOfCharacteristicProfile()
        {
            CreateMap<DictionaryOfCharacteristicEntity, DictionaryOfCharacteristicDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<DictionaryOfCharacteristicDto, DictionaryOfCharacteristicEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
