using e_pharmacy.Models;
using e_pharmacy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace e_pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto userDto)
        {
            var existingUser = await _userService.GetByUsernameAsync(userDto.Username);
            if (existingUser != null)
            {
                return Conflict("User with this username already exists.");
            }

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Roles = userDto.Roles
            };

            await _userService.CreateAsync(user);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }
    }
}
