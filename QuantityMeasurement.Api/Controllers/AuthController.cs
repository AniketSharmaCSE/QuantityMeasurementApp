using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.BusinessLayer.Auth;
using QuantityMeasurement.Model.DTOs.Auth;

namespace QuantityMeasurement.Api.Controllers
{
    // User registration and login – both endpoints return a JWT bearer token.
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Register a new user. Password is stored as a BCrypt salted hash
        // Username must be 3–100 chars. Password must be at least 6 chars
        // <response code="201">Registration successful – token returned.
        // <response code="400">Validation error or username/email already taken.
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AuthResponseDTO> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return StatusCode(201, _authService.Register(request));
        }

        // Login with username and password to receive a JWT bearer token.
        // Use the returned token in the Authorization header as: Bearer {token}
        // <response code="200">Login successful – token returned.
        // <response code="401">Invalid credentials.
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AuthResponseDTO> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_authService.Login(request));
        }
    }
}
