using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Repository.Data;

namespace QuantityMeasurement.Api.Controllers
{
    // Exposes user management endpoints to power the Admin Panel
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /api/v1/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            // We specifically map out the exact fields the admin panel requires in JSON
            var users = _db.Users
                .Select(u => new 
                {
                    id = u.Id.ToString(),
                    username = u.Username,
                    email = u.Email,
                    name = u.Name,
                    createdAt = u.CreatedAt
                })
                .ToList();

            return Ok(users);
        }

        // DELETE: /api/v1/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                if (!int.TryParse(id, out int userId))
                    return BadRequest($"Invalid user ID format: {id}");

                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    return NotFound($"User with id {userId} not found.");

                _db.Users.Remove(user);
                _db.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during delete: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        // GET: /api/v1/users/total-history
        [HttpGet("total-history")]
        public IActionResult GetTotalHistory()
        {
            return Ok(_db.Measurements.Count());
        }
    }
}
