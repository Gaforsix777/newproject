using System.ComponentModel.DataAnnotations;

namespace practicamvc.ViewModels
{
    public class LoginVM
    {
        [Required, Display(Name = "Usuario o Email")]
        public string UserOrEmail { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
