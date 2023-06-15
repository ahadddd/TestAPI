using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using TestAPI.Data;
using TestAPI.Dto;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController: Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser(RegisterDto register)
        {
            if (register.Username != null && await UserExists(register.Username))
                return BadRequest("User already exists.");
            using var hmac = new HMACSHA512();
            if(register.Username != null)
            {
                var user = new User()
                {
                    Username = register.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                    PasswordSalt = hmac.Key
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(LoginDto login)
        {
            var user = await _context.Users.Where(u => u.Username == login.Username).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }
            else if (user.PasswordSalt != null && user.PasswordHash != null)
            {
                var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                    {
                        return Unauthorized("Invalid password.");
                    }
                }
                return user;
            }
            return NoContent();
        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

    }
}
