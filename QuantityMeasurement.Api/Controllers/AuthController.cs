using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.BusinessLayer.Auth;
using QuantityMeasurement.Model.DTOs.Auth;
using System.Security.Claims;

namespace QuantityMeasurement.Api.Controllers
{
    // User registration, login, and Google OAuth2 – all return a JWT bearer token.
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Register a new user. Password must have min 6 chars, 1 uppercase, 1 digit, 1 special char.
        // Username and email must both be unique.
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

        // Login with username and password.
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

        // UC18: Google OAuth2 – Step 1
        // Redirect the user to Google's login page.
        // Call this from the browser: GET /api/v1/auth/google
        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
            var properties  = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // UC18: Google OAuth2 – Step 2 (callback)
        // Google redirects here after the user logs in.
        // We read the verified email + name from Google's claims,
        // then create/find the user and return a JWT.
        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            // Google signed the user in temporarily using the ExternalCookie scheme we registered in Program.cs
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");

            if (!result.Succeeded)
                return BadRequest("Google authentication failed.");

            var email = result.Principal?.FindFirstValue(ClaimTypes.Email);
            var name  = result.Principal?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Could not retrieve email from Google.");

            var response = _authService.LoginOrRegisterWithGoogle(email, name);

            // Clear the temporary cookie since we now have our own JWT
            await HttpContext.SignOutAsync("ExternalCookie");

            // Redirect back to the frontend with the JWT in the URL query parameters
            var frontendUrl = "http://localhost:4200/";
            return Redirect($"{frontendUrl}?token={response.Token}&username={response.Username}&email={response.Email}&name={response.Name}");
        }
    }
}
