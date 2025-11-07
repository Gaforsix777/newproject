// Models/DtoUser/UserDtos.cs
using System.ComponentModel.DataAnnotations;

namespace WebAPI1.Models.DTOs
{
    public class RegisterUserDto
    {
        [Required, MaxLength(100)] public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        [Required] public int Id { get; set; }
        [Required, MaxLength(100)] public string UserName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
    }

    public class UpdateEmailDto
    {
        [Required] public int Id { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    }

    public class DeleteUserDto
    {
        [Required] public int Id { get; set; }
    }
}
