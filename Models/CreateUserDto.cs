using System.Collections.Generic;

namespace e_pharmacy.Models
{
    public record CreateUserDto(string Username, string Password, List<string> Roles);
}
