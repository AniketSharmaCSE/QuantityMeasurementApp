using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Api.Models;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.Model.DTOs;

namespace QuantityMeasurement.Api.Controllers
{
    // All measurement operations in one controller.
    // Category field selects which measurement type: Length, Weight, Volume, Temperature.
    // Operation field (on /calculate) selects: Add, Subtract, Divide.
    // Temperature only supports compare and convert.
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/quantities")]
    public class QuantitiesController : ControllerBase
    {
        private readonly IQuantityService _service;

        public QuantitiesController(IQuantityService service)
        {
            _service = service;
        }

        // Compare two quantities of the same category.
        // Category: Length, Weight, Volume, Temperature
        // Returns Result.Value = 1 if equal, 0 if not.
        // <response code="200">Comparison result.
        // <response code="400">Invalid unit, value, or unknown category.
        // <response code="401">Missing or invalid JWT token.
        [HttpPost("compare")]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<QuantityResponseDTO> Compare([FromBody] QuantityRequest req)
        {
            var result = req.Category?.ToLower() switch
            {
                "length"      => _service.CompareLength(req.Value1, req.Unit1, req.Value2, req.Unit2),
                "weight"      => _service.CompareWeight(req.Value1, req.Unit1, req.Value2, req.Unit2),
                "volume"      => _service.CompareVolume(req.Value1, req.Unit1, req.Value2, req.Unit2),
                "temperature" => _service.CompareTemperature(req.Value1, req.Unit1, req.Value2, req.Unit2),
                _             => QuantityResponseDTO.ForError("Compare", $"Unknown category '{req.Category}'.")
            };
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Convert a quantity from one unit to another.
        // Category: Length, Weight, Volume, Temperature
        // <response code="200">Converted value.
        // <response code="400">Invalid unit or unknown category.
        // <response code="401">Missing or invalid JWT token.
        [HttpPost("convert")]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<QuantityResponseDTO> Convert([FromBody] ConvertRequest req)
        {
            var result = req.Category?.ToLower() switch
            {
                "length"      => _service.ConvertLength(req.Value, req.FromUnit, req.ToUnit),
                "weight"      => _service.ConvertWeight(req.Value, req.FromUnit, req.ToUnit),
                "volume"      => _service.ConvertVolume(req.Value, req.FromUnit, req.ToUnit),
                "temperature" => _service.ConvertTemperature(req.Value, req.FromUnit, req.ToUnit),
                _             => QuantityResponseDTO.ForError("Convert", $"Unknown category '{req.Category}'.")
            };
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Perform arithmetic on two quantities.
        // Operation: Add, Subtract, Divide  (not supported for Temperature)
        // Category: Length, Weight, Volume
        // TargetUnit is optional - only used with Add for Length to express result in a different unit.
        // <response code="200">Arithmetic result.
        // <response code="400">Invalid unit, divide by zero, unsupported operation/category.
        // <response code="401">Missing or invalid JWT token.
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QuantityResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<QuantityResponseDTO> Calculate([FromBody] QuantityRequest req)
        {
            var op  = req.Operation?.ToLower() ?? "";
            var cat = req.Category?.ToLower()  ?? "";

            QuantityResponseDTO result = (op, cat) switch
            {
                ("add",      "length")  when !string.IsNullOrWhiteSpace(req.TargetUnit)
                                        => _service.AddLengthWithTarget(req.Value1, req.Unit1, req.Value2, req.Unit2, req.TargetUnit!),
                ("add",      "length")  => _service.AddLength(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("add",      "weight")  => _service.AddWeight(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("add",      "volume")  => _service.AddVolume(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("subtract", "length")  => _service.SubtractLength(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("subtract", "weight")  => _service.SubtractWeight(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("subtract", "volume")  => _service.SubtractVolume(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("divide",   "length")  => _service.DivideLength(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("divide",   "weight")  => _service.DivideWeight(req.Value1, req.Unit1, req.Value2, req.Unit2),
                ("divide",   "volume")  => _service.DivideVolume(req.Value1, req.Unit1, req.Value2, req.Unit2),
                _                       => QuantityResponseDTO.ForError("Calculate",
                                            $"Operation '{req.Operation}' is not supported for category '{req.Category}'. " +
                                            "Valid combinations: Add/Subtract/Divide with Length, Weight, or Volume.")
            };

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
