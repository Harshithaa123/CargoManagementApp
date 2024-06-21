
using Cargo_FinalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cargo_FinalApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationsTables : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public UserRegistrationsTables(FinalCargoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.UserRegistrationsTables.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.UserRegistrationsTables.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(UserRegistrationsTable user)
        {
            _context.UserRegistrationsTables.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = user.RegistrationId }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserRegistrationsTable user)
        {
            if (id != user.RegistrationId)
            {
                return BadRequest();
            }

            var existingUser = _context.UserRegistrationsTables.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.RegistrationDate = user.RegistrationDate;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.UserRegistrationsTables.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserRegistrationsTables.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPost("login/{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email);

            if (admin == null || admin.Password != password)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { message = "Login successful" });
        }

        [HttpGet("getUserName/{email}/{passwordHash}")]
        public IActionResult GetUserName(string email, string passwordHash)
        {
            var user = _context.UserRegistrationsTables
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}
