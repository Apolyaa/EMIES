using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class UnitOfMeasurementController : Controller
    {
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;

        public UnitOfMeasurementController(IUnitOfMeasurementService unitOfMeasurementService)
        {
            _unitOfMeasurementService = unitOfMeasurementService;
        }

        [HttpGet("/getunits")]
        [ProducesResponseType(typeof(Response<List<UnitOfMesurementDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnitsOfMeasurement()
        {

            var result = _unitOfMeasurementService.GetUnitsOfMeasurement();

            return Ok(result);
        }
    }
}
