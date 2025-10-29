using System.ComponentModel.DataAnnotations;
using practicamvc.Models;

namespace practicamvc.ViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(100), Display(Name = "Usuario")]
        [RegularExpression(@"^\S+$", ErrorMessage = "El nombre de usuario no puede contener espacios")]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(6), Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(Password)), Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, Display(Name = "Rol")]
        public string Role { get; set; } = UserModel.RolCliente;
    }
}
