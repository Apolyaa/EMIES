using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceEntity, DeviceDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.Url));
            CreateMap<DeviceDto, DeviceEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.Url, opt => opt.MapFrom(src => src.Url));
        }
    }
}
