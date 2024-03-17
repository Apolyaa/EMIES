using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class ResultDeviceProfile : Profile
    {
        public ResultDeviceProfile()
        {
            CreateMap<ResultDeviceEntity, ResultDeviceDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ResultId, opt => opt.MapFrom(src => src.ResultId))
                .ForMember(d => d.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(d => d.PercentEssential, opt => opt.MapFrom(src => src.PercentEssential))
                .ForMember(d => d.PercentUnessential, opt => opt.MapFrom(src => src.PercentUnessential));
            CreateMap<ResultDeviceDto, ResultDeviceEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(d => d.PercentEssential, opt => opt.MapFrom(src => src.PercentEssential))
                .ForMember(d => d.PercentUnessential, opt => opt.MapFrom(src => src.PercentUnessential));
        }
    }
}
