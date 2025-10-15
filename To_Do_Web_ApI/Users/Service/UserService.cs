using To_Do_Web_ApI.Data;
using Microsoft.EntityFrameworkCore;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Model.Entity;
namespace To_Do_Web_ApI.Users.Service
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> FindUserByUsernameAsync(CreateUserDto userDto)
        {
            var existingUser = await _db.Users
                .FirstOrDefaultAsync(u => u.username == userDto.username);

            return existingUser; 
        }
    }
}