using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class DictionaryOfCharacteristicController : Controller
    {
        private readonly IDictionaryOfCharacteristicService _dictionaryOfCharacteristicService;

        public DictionaryOfCharacteristicController(IDictionaryOfCharacteristicService dictionaryOfCharacteristicService)
        {
            _dictionaryOfCharacteristicService = dictionaryOfCharacteristicService;
        }

        [HttpGet("/getcharacteristics")]
        [ProducesResponseType(typeof(Response<List<DictionaryOfCharacteristicDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCharacteristics()
        {

            var result = _dictionaryOfCharacteristicService.GetCharacteristics();

            return Ok(result);
        }
        [HttpPut("/addcharacteristic")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCharacteristic([FromBody] DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto)
        {

            var result = _dictionaryOfCharacteristicService.AddCharacteristic(dictionaryOfCharacteristicDto);

            return Ok(result);
        }
        [HttpPost("/updatecharacteristic")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCharacteristic([FromBody] DictionaryOfCharacteristicDto dictionaryOfCharacteristicDto)
        {

            var result = _dictionaryOfCharacteristicService.UpdateCharacteristic(dictionaryOfCharacteristicDto);

            return Ok(result);
        }
        [HttpDelete("/deletecharacteristic/{characteristicId}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCharacteristic(Guid characteristicId)
        {

            var result = _dictionaryOfCharacteristicService.DeleteCharacteristic(characteristicId);

            return Ok(result);
        }
    }
}
