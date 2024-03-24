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
        [HttpPut("/addtype")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddType([FromBody] TypeOfDeviceDto typeOfDeviceDto)
        {

            var result = _typeOfDeviceService.AddType(typeOfDeviceDto);

            return Ok(result);
        }
        [HttpPost("/updatetype")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateType([FromBody] TypeOfDeviceDto typeOfDeviceDto)
        {

            var result = _typeOfDeviceService.UpdateType(typeOfDeviceDto);

            return Ok(result);
        }
        [HttpDelete("/deletetype/{typeId}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteType(Guid typeId)
        {

            var result = _typeOfDeviceService.DeleteType(typeId);

            return Ok(result);
        }

    }
}
