using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/createuser")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                Console.WriteLine("User is null");
                return BadRequest("User is null");
            }
              
            var result = _userService.Create(user);
          
            return Ok(result);
        }

        [HttpPost("/entry")]
        [ProducesResponseType(typeof(Response<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Entry([FromBody] UserDto user)
        {
            if (user == null)
            {
                Console.WriteLine("User is null");
                return BadRequest("User is null");
            }

            var result = _userService.Entry(user);

            return Ok(result);
        }

    }
}
