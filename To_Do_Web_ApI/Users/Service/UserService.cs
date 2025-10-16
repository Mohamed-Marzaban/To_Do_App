using To_Do_Web_ApI.Data;
using Microsoft.EntityFrameworkCore;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Model.Entity;
using BCrypt.Net;
namespace To_Do_Web_ApI.Users.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            var existingUser = await _db.Users
                .FirstOrDefaultAsync(u => u.username == username);

            return existingUser; 
        }

        public async Task<String> CreateUserAsync(CreateUserDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.password,workFactor:12);

            User user = new()
            {
                username = userDto.username,
                password = passwordHash
            };
            
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user.username;
        }

        public async Task<User?> AuthenticateUserAsync(CreateUserDto userDto)
        {
            User? user = await this.FindUserByUsernameAsync(userDto.username);

            if (user is null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(userDto.password, user.password);
            
            return isValid ? user : null;



        }
    }
}