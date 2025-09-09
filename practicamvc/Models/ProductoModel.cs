// Models/ProductoModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicamvc.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; }

        [StringLength(400)]
        public string Descripcion { get; set; }

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Precio (Bs)")]
        public decimal Precio { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }

        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}
