using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Order
    {
        public int Id { get; set; }

        [Required] public int ClienteId { get; set; }
        public User? Cliente { get; set; }

        [Required] public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required] public OrderStatus Estado { get; set; } = OrderStatus.Pendiente;

        [Precision(18, 2)]
        [Range(0, 99999999)]
        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public enum OrderStatus { Pendiente, Procesado, Enviado, Entregado }
}