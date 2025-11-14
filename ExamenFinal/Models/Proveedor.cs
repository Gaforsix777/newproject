using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Contacto { get; set; }
    }
}
