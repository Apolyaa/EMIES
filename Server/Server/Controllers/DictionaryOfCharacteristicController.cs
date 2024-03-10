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
    }
}
