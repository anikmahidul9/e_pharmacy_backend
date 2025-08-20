using System.Collections.Generic;

namespace e_pharmacy.Models
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Occupation { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
