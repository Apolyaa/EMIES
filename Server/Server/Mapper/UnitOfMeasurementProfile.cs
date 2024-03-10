using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class UnitOfMeasurementProfile : Profile
    {
        public UnitOfMeasurementProfile()
        {
            CreateMap<UnitOfMeasurementEntity, UnitOfMesurementDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.MultiplicationFactor, opt => opt.MapFrom(src => src.MultiplicationFactor));
            CreateMap<UnitOfMesurementDto, UnitOfMeasurementEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.MultiplicationFactor, opt => opt.MapFrom(src => src.MultiplicationFactor));
        }
    }
}
