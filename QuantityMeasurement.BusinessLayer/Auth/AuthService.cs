using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurement.Model.DTOs.Auth;
using QuantityMeasurement.Model.Entities.EF;
using QuantityMeasurement.Repository.Data;

namespace QuantityMeasurement.BusinessLayer.Auth
{
    // Handles registration (BCrypt salted hash), login (JWT issuance),
    // and Google OAuth2 sign-in / auto-registration.
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db     = db;
            _config = config;
        }

        public AuthResponseDTO Register(RegisterRequestDTO request)
        {
            if (_db.Users.Any(u => u.Username == request.Username))
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

            if (_db.Users.Any(u => u.Email == request.Email))
                throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

            string hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserEntity
            {
                Username     = request.Username,
                Email        = request.Email,
                PasswordHash = hash,
                Name         = request.Name,
                CreatedAt    = DateTime.UtcNow
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return BuildResponse(user, "Registration successful.");
        }

        public AuthResponseDTO Login(LoginRequestDTO request)
        {
            // Allow login by username or email
            var user = _db.Users.FirstOrDefault(u => u.Username == request.Username || u.Email == request.Username)
                ?? throw new UnauthorizedAccessException("Invalid username or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            return BuildResponse(user, "Login successful.");
        }

        // UC18: Google OAuth2
        // If the user already exists (matched by email) we just log them in.
        // If they are new we auto-register them with a random password hash
        public AuthResponseDTO LoginOrRegisterWithGoogle(string email, string name)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                // Auto-register: generate a username from the email prefix
                var baseUsername = email.Split('@')[0];
                var username     = baseUsername;
                int suffix       = 1;

                // Make sure the username is unique
                while (_db.Users.Any(u => u.Username == username))
                    username = baseUsername + suffix++;

                user = new UserEntity
                {
                    Username     = username,
                    Email        = email,
                    Name         = name,
                    // Random password hash – Google users never use this
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    CreatedAt    = DateTime.UtcNow
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            }

            return BuildResponse(user, "Google login successful.");
        }

        private AuthResponseDTO BuildResponse(UserEntity user, string message)
        {
            var key       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds     = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,  user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             _config["Jwt:Issuer"],
                audience:           _config["Jwt:Audience"],
                claims:             claims,
                expires:            expiresAt,
                signingCredentials: creds);

            return new AuthResponseDTO
            {
                Token     = new JwtSecurityTokenHandler().WriteToken(token),
                Username  = user.Username,
                Email     = user.Email,
                Name      = user.Name,
                ExpiresAt = expiresAt,
                Message   = message
            };
        }
    }
}
