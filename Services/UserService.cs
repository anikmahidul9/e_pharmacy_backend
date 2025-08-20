using e_pharmacy.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;

namespace e_pharmacy.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(CreateUserDto createUserDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                Username = createUserDto.Username,
                Password = hashedPassword,
                Role = createUserDto.Role,
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                Occupation = createUserDto.Occupation,
                Image = createUserDto.Image
            };

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                if (updateUserDto.Username != null)
                {
                    existingUser.Username = updateUserDto.Username;
                }
                if (updateUserDto.Name != null)
                {
                    existingUser.Name = updateUserDto.Name;
                }
                if (updateUserDto.Email != null)
                {
                    existingUser.Email = updateUserDto.Email;
                }
                if (updateUserDto.Occupation != null)
                {
                    existingUser.Occupation = updateUserDto.Occupation;
                }
                if (updateUserDto.Image != null)
                {
                    existingUser.Image = updateUserDto.Image;
                }

                await _users.ReplaceOneAsync(user => user.Id == id, existingUser);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
    }
}