using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("/getdevices")]
        [ProducesResponseType(typeof(Response<List<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevices()
        {

            var result = _deviceService.GetDevices();

            return Ok(result);
        }
        [HttpPut("/adddevice")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDevice([FromBody] DeviceDto deviceDto)
        {

            var result = _deviceService.AddDevice(deviceDto);

            return Ok(result);
        }
        [HttpPost("/updatedevice")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDevice([FromBody] DeviceDto deviceDto)
        {

            var result = _deviceService.UpdateDevice(deviceDto);

            return Ok(result);
        }
        [HttpDelete("/deletedevice/{deviceId}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCharacteristic(Guid deviceId)
        {

            var result = _deviceService.DeleteDevice(deviceId);

            return Ok(result);
        }
    }
}
