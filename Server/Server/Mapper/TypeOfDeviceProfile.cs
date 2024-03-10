using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class TypeOfDeviceProfile : Profile
    {
        public TypeOfDeviceProfile()
        {
            CreateMap<TypeOfDevicesEntity, TypeOfDeviceDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<TypeOfDeviceDto, TypeOfDevicesEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
