using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required, Range(0.01, 9999999)]
        public decimal Precio { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [StringLength(100)]
        public string? Categoria { get; set; }
    }
}
