using Pizza.Data;
using Pizza.Models;
using Pizza.Models.DTTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Pizza.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        public UserService(AppDbContext appDbContext)
        {
            _appDbContext= appDbContext;
        }
        public async Task<User> RegisterUserAsync(RegiserUserDto dto)
        {
            throw new NotImplementedException();
            if(await _appDbContext.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("User with this email already exists");
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password
            };
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;

        }
        
        public string HashPassword(string password)
        {
            return "";
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }
    }
}
