using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class ProducerProfile : Profile
    {
        public ProducerProfile()
        {
            CreateMap<ProducerEntity, ProducerDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<ProducerDto, ProducerEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Contacts, opt => opt.MapFrom(src => src.Contacts))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
