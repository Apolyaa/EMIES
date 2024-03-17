using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class ResultController : Controller
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost("/finddevices")]
        [ProducesResponseType(typeof(Response<ResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FindDevices([FromBody]RequestFind requestFind)
        {

            var result = _resultService.FindDevices(requestFind);

            return Ok(result);
        }
    }
}
