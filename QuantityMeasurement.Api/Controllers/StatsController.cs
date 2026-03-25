using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.BusinessLayer.Interfaces;

namespace QuantityMeasurement.Api.Controllers
{
    // Returns a summary of all operations stored so far.
    // Reuses GetOperationStats() and GetTotalCount() from IQuantityService.
    [ApiController]
    [Authorize]
    [Route("api/v1/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IQuantityService _service;

        public StatsController(IQuantityService service)
        {
            _service = service;
        }

        // Return total record count and a breakdown of counts per operation type.
        // Example response: { "total": 10, "byOperation": { "Add": 5, "Compare": 3, "Convert": 2 } }
        // <response code="200">Stats summary.
        // <response code="401">Missing or invalid JWT token.
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult GetStats()
        {
            return Ok(new
            {
                total       = _service.GetTotalCount(),
                byOperation = _service.GetOperationStats()
            });
        }
    }
}
