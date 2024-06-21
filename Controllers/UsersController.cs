using Cargo_FinalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Final_CargoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public UsersController(FinalCargoDbContext context)
        {
            _context = context;
        }

 
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Include(u => u.UserDetails)
                .ToList();

            return Ok(users);
        }

     
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

     
        [HttpPost]
        public IActionResult PostUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

     
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            var existingUser = _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefault(u => u.UserId == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = user.Username;
            existingUser.Role = user.Role;

      
            var userDetailsToRemove = existingUser.UserDetails
                .Where(ud => !user.UserDetails.Any(udDto => udDto.UserDetailId == ud.UserDetailId))
                .ToList();

            _context.UserDetails.RemoveRange(userDetailsToRemove);

            foreach (var userDetails in user.UserDetails)
            {
                var existingUserDetails = existingUser.UserDetails.FirstOrDefault(ud => ud.UserDetailId == userDetails.UserDetailId);
                if (existingUserDetails == null)
                {
                    existingUser.UserDetails.Add(userDetails);
                }
                else
                {
                    existingUserDetails.FullName = userDetails.FullName;
                    existingUserDetails.Email = userDetails.Email;
                    existingUserDetails.PhoneNumber = userDetails.PhoneNumber;
                }
            }

            _context.Entry(existingUser).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

  
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

       
        [HttpPost("register")]
        public IActionResult RegisterUser(UserRegistrationsTable registerUserDto)
        {
            if (registerUserDto == null)
            {
                return BadRequest("User data is null");
            }

          
            var userRegistration = new UserRegistrationsTable
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                PasswordHash = registerUserDto.PasswordHash,
                RegistrationDate = registerUserDto.RegistrationDate
            };

           
            _context.UserRegistrationsTables.Add(userRegistration);
            _context.SaveChanges();

            
            return Ok(userRegistration);
        }



    }
}
