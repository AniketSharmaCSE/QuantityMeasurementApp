using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Model.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Min 6 chars, at least one uppercase, one digit, one special character
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{6,}$",
            ErrorMessage = "Password must be at least 6 characters and include one uppercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }

    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
