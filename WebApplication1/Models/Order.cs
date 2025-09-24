using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }  // Relacionado con la tabla Users
        public User Cliente { get; set; }  // Relación con User

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Estado { get; set; } = OrderStatus.Pendiente;

        [Range(0.01, 9999999)]
        public decimal Total { get; set; }  // Total calculado del pedido

        public List<OrderItem> Items { get; set; } = new();  // Relación con OrderItem
    }

    public enum OrderStatus { Pendiente, Procesado, Enviado, Entregado }
}
