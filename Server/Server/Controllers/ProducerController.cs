using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class ProducerController : Controller
    {
        private readonly IProducerService _producerService;

        public ProducerController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpGet("/getproducers")]
        [ProducesResponseType(typeof(Response<List<ProducerDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducers()
        {

            var result = _producerService.GetProducers();

            return Ok(result);
        }
        [HttpPut("/addproducer")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddProducer([FromBody] ProducerDto producerDto)
        {
            var result = _producerService.AddProducer(producerDto);

            return Ok(result);
        }
        [HttpPost("/updateproducer")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProducer([FromBody] ProducerDto producerDto)
        {

            var result = _producerService.UpdateProducer(producerDto);

            return Ok(result);
        }
        [HttpDelete("/deleteproducer")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProducer([FromBody] Guid producerId)
        {

            var result = _producerService.DeleteProducer(producerId);

            return Ok(result);
        }
    }
}
