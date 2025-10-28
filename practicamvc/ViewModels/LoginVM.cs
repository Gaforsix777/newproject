using System.ComponentModel.DataAnnotations;

namespace practicamvc.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El usuario o email es requerido")]
        [Display(Name = "Usuario o Email")]
        public string UserOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
