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

            var newUser = await _userService.CreateAsync(userDto);

            return CreatedAtAction(nameof(Register), new { id = newUser.Id }, newUser);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserDto updatedUserDto)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.UpdateAsync(id, updatedUserDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByUsernameAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}