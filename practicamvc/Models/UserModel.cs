using System.ComponentModel.DataAnnotations;

namespace practicamvc.Models
{
    public class UserModel
    {
        public const string RolCliente = "Cliente";
        public const string RolProveedor = "Proveedor";

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Salt { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = RolCliente;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
