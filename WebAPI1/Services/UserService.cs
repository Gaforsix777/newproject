// Services/UserService.cs
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebAPI1.Data;
using WebAPI1.Models;
using WebAPI1.Models.DTOs;

namespace WebAPI1.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        public UserService(AppDbContext db) => _db = db;

        public async Task<List<User>> GetAllUserAsync() =>
            await _db.Users.AsNoTracking().ToListAsync();

        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email ya registrado");
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = HashPassword(dto.Password)
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) throw new Exception("Usuario no encontrado");
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = HashPassword(dto.Password);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateEmailAsync(UpdateEmailDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) throw new Exception("Usuario no encontrado");
            user.Email = dto.Email;
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(DeleteUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) throw new Exception("Usuario no encontrado");
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
