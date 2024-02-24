using AutoMapper;
using Client.Contracts;
using Server.EfCore.Model;
using Server.Repositories;

namespace Server.Services
{
    public class UserService : IUserService
    {
        protected readonly IUserRepository _userRepository;
        protected readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Response<bool> Create(UserDto userDto)
        {
            Response<bool> response = new();
            try 
            {
                var userEntity = _mapper.Map<UserEntity>(userDto);
                var existUser = _userRepository.GetAll().Where(t => t.Login == userEntity.Login);
                if (existUser.Any())
                {
                    response.Success = false;
                    response.Message = "Пользователь уже существует.";
                    return response;
                }

                _userRepository.Insert(userEntity);
                _userRepository.Save();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create user failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при создании пользователя.";
                return response;
            }
        } 
        public Response<UserDto> Entry(UserDto userDto)
        {
            Response<UserDto> response = new();
            try
            {
                var userEntity = _mapper.Map<UserEntity>(userDto);
                var existUser = _userRepository.GetAll()
                    .FirstOrDefault(t => t.Login == userEntity.Login && t.Password == userEntity.Password);
                if (existUser is null)
                {
                    response.Success = false;
                    response.Message = "Пользователь не найден.";
                    return response;
                }
                
                response.Data = _mapper.Map<UserDto>(existUser);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get users failed.");
                Console.WriteLine(ex.ToString());
                response.Success = false;
                response.Message = "Ошибка при проверке пользователя.";
                return response;
            }
        }
    }
}
