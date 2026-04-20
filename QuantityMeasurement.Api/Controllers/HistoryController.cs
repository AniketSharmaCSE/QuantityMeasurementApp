using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Api.Controllers
{
    // All history access goes through a single endpoint with optional query filters.
    [ApiController]
    [Authorize]
    [Route("api/v1/history")]
    public class HistoryController : ControllerBase
    {
        private readonly IQuantityService _service;

        public HistoryController(IQuantityService service)
        {
            _service = service;
        }

        // Return stored operation records with optional filters.
        // ?operation=Add|Subtract|Divide|Compare|Convert  (case-insensitive)
        // ?category=Length|Weight|Volume|Temperature       (case-insensitive)
        // ?count=N  returns the N most recent records
        // With no parameters returns everything, newest first.
        // <response code="200">Matching records.
        // <response code="401">Missing or invalid JWT token.
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<QuantityResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IReadOnlyList<QuantityResponseDTO>> Get(
            [FromQuery] string? operation,
            [FromQuery] string? category,
            [FromQuery] int? count)
        {
            IReadOnlyList<QuantityResponseDTO> results;

            if (!string.IsNullOrWhiteSpace(operation))
                results = _service.GetHistoryByOperation(operation);
            else if (!string.IsNullOrWhiteSpace(category))
                results = _service.GetHistoryByCategory(category);
            else if (count.HasValue)
                results = _service.GetRecentHistory(count.Value);
            else
                results = _service.GetHistory();

            return Ok(results);
        }

        // Delete all stored records.
        // <response code="204">All records deleted.
        // <response code="401">Missing or invalid JWT token.
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Clear()
        {
            _service.ClearHistory();
            return NoContent();
        }
    }
}
