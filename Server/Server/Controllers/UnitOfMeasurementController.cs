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
        [HttpPut("/addunit")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUnit([FromBody] UnitOfMesurementDto unitOfMesurementDto)
        {

            var result = _unitOfMeasurementService.AddUnit(unitOfMesurementDto);

            return Ok(result);
        }
        [HttpPost("/updateunit")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUnit([FromBody] UnitOfMesurementDto unitOfMesurementDto)
        {

            var result = _unitOfMeasurementService.UpdateUnit(unitOfMesurementDto);

            return Ok(result);
        }
        [HttpDelete("/deleteunit")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUnit([FromBody] Guid unitId)
        {

            var result = _unitOfMeasurementService.DeleteUnit(unitId);

            return Ok(result);
        }
    }
}
