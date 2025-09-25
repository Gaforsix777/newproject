using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class UserCreateVM
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Rol { get; set; } = UserRole.cliente;
    }
}
