// Models/ProductoModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// Si usas EF Core 6+ puedes usar [Precision], descomenta la línea siguiente:
// using Microsoft.EntityFrameworkCore;

namespace practicamvc.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(400)]
        public string? Descripcion { get; set; }

        // Límite superior solo a modo de validación; ajusta si necesitas más
        [Range(0, 999999.99, ErrorMessage = "El precio debe estar entre 0 y 999999,99")]
        // Para SQL Server: asegura 2 decimales en BD
        [Column(TypeName = "decimal(10,2)")]
        // Si usas EF Core 6+ puedes usar Precision en vez de Column (o además):
        // [Precision(10, 2)]
        [Display(Name = "Precio (Bs)")]
        // Mostrar SIEMPRE dos decimales en pantalla
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Precio { get; set; }

        [Range(0, 100000, ErrorMessage = "El stock debe ser un número entero ? 0")]
        public int Stock { get; set; }

        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}
