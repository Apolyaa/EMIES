using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class TypeCharacteristicProfile : Profile
    {
        public TypeCharacteristicProfile()
        {
            CreateMap<TypeCharacteristicEntity, TypeCharacteristicDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(d => d.CharacteristicId, opt => opt.MapFrom(src => src.CharacteristicId));
            CreateMap<TypeCharacteristicDto, TypeCharacteristicEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(d => d.CharacteristicId, opt => opt.MapFrom(src => src.CharacteristicId));
        }
    }
}
