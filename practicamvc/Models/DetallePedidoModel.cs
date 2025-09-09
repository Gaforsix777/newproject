// Models/DetallePedidoModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicamvc.Models
{
    public class DetallePedidoModel
    {
        public int Id { get; set; }

        [Required]
        public int IdPedido { get; set; }

        [Required, Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Range(1, 100000)]
        public int Cantidad { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99), Display(Name = "Precio Unitario (Bs)")]
        public decimal PrecioUnitario { get; set; }

        public PedidoModel? Pedido { get; set; }
        public ProductoModel? Producto { get; set; }
    }
}
