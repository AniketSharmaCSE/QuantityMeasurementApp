using QuantityMeasurement.Model.DTOs.Auth;

namespace QuantityMeasurement.BusinessLayer.Auth
{
    public interface IAuthService
    {
        AuthResponseDTO Register(RegisterRequestDTO request);
        AuthResponseDTO Login(LoginRequestDTO request);
    }
}
