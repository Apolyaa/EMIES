using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;

namespace Server.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id)) 
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(d => d.Role, opt => opt.MapFrom(src => src.Role));
            CreateMap<UserDto, UserEntity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(d => d.Role, opt => opt.MapFrom(src => src.Role));
        }
    }
}
