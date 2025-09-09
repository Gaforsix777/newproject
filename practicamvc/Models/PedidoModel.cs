// Models/PedidoModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practicamvc.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pedido")]
        public DateTime FechaPedido { get; set; } = DateTime.Today;

        [Required, Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Total (Bs)")]
        public decimal MontoTotal { get; set; }

        public ClienteModel? Cliente { get; set; }
        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}
