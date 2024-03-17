using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<ResultEntity, ResultDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.InitialData, opt => opt.MapFrom(src => src.InitialData));
            CreateMap<ResultDto, ResultEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.InitialData, opt => opt.MapFrom(src => src.InitialData));
        }
    }
}
