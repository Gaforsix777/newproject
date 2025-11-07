using System.ComponentModel.DataAnnotations;

namespace Pizza.Models.DTTOs
{
    public class RegiserUserDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
