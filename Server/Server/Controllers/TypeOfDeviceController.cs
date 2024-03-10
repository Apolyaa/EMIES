using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class TypeOfDeviceController : Controller
    {
        private readonly ITypeOfDeviceService _typeOfDeviceService;

        public TypeOfDeviceController(ITypeOfDeviceService typeOfDeviceService)
        {
            _typeOfDeviceService = typeOfDeviceService;
        }

        [HttpGet("/gettypes")]
        [ProducesResponseType(typeof(Response<List<TypeOfDeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypesOfDevices()
        {

            var result = _typeOfDeviceService.GetTypes();

            return Ok(result);
        }

    }
}
