using Microsoft.AspNetCore.Identity;
using System;

namespace NetIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime FechaNacimiento { get; set; }
        public string? NombreCompleto { get; set; }
        public string genero { get; set; } = "Otro";
    }
}
