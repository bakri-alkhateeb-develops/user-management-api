using Microsoft.AspNetCore.Mvc;
using user_management_api.Models;

namespace user_management_api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private static List<User> _users = new List<User>();

        [HttpGet]
        public IActionResult GetUsers() => Ok(_users);

        [HttpGet("{id}")]
        public IActionResult GetUser(int id) {
            try {
                var user = _users.FirstOrDefault(u => u.Id == id);
                if (user == null) throw new KeyNotFoundException("User not found");
                return Ok(user);
            } catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user) {
            if (string.IsNullOrWhiteSpace(user.Name) || !user.Email.Contains("@")) {
                return BadRequest(new { error = "Invalid user data" });
            }
            try {
                user.Id = _users.Count + 1;
                _users.Add(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            } catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser) {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { error = "User not found" });
            if (string.IsNullOrWhiteSpace(updatedUser.Name) || !updatedUser.Email.Contains("@")) {
                return BadRequest(new { error = "Invalid user data" });
            }
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id) {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { error = "User not found" });
            _users.Remove(user);
            return NoContent();
        }
    }
}