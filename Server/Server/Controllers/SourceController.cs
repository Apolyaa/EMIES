using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class SourceController : Controller
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [HttpGet("/getsources")]
        [ProducesResponseType(typeof(Response<List<SourceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSources()
        {

            var result = _sourceService.GetSources();

            return Ok(result);
        }
        [HttpPut("/addsource")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSource([FromBody] SourceDto source)
        {

            var result = _sourceService.AddSource(source);

            return Ok(result);
        }
        [HttpPost("/updatesource")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSource([FromBody] SourceDto source)
        {

            var result = _sourceService.UpdateSource(source);

            return Ok(result);
        }
        [HttpDelete("/deletesource")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveSource([FromBody] Guid sourceId)
        {

            var result = _sourceService.DeleteSource(sourceId);

            return Ok(result);
        }
    }
}
