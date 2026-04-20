using QuantityMeasurement.Model.DTOs.Auth;

namespace QuantityMeasurement.BusinessLayer.Auth
{
    public interface IAuthService
    {
        AuthResponseDTO Register(RegisterRequestDTO request);
        AuthResponseDTO Login(LoginRequestDTO request);

        // UC18: Google OAuth2 - called after Google redirects back with a verified email
        AuthResponseDTO LoginOrRegisterWithGoogle(string email, string name);
    }
}
