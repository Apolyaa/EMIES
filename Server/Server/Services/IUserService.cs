
using Server.EfCore.Model;
using Client.Contracts;

namespace Server.Services
{
    public interface IUserService
    {
        Response<bool> Create(UserDto userDto);
        Response<UserDto> Entry(UserDto userDto);   
    }
}
