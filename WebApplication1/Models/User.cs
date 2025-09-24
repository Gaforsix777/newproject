using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public enum UserRole { admin, cliente, empleado }

    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        // Guardamos el hash de la contraseña
        [Required, StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Rol { get; set; } = UserRole.cliente;
    }
}
