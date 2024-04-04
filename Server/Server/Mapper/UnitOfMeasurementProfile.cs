using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class UnitOfMeasurementProfile : Profile
    {
        public UnitOfMeasurementProfile()
        {
            CreateMap<UnitOfMeasurementEntity, UnitOfMeasurementDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.MultiplicationFactor, opt => opt.MapFrom(src => src.MultiplicationFactor));
            CreateMap<UnitOfMeasurementDto, UnitOfMeasurementEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.MultiplicationFactor, opt => opt.MapFrom(src => src.MultiplicationFactor));
        }
    }
}
