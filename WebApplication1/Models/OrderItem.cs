using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }  // Relacionado con Order
        public Order Order { get; set; }  // Relación con Order

        [Required]
        public int ProductId { get; set; }  // Relacionado con Product
        public Product Product { get; set; }  // Relación con Product

        [Required, Range(1, 1000000)]
        public int Cantidad { get; set; }  // Cantidad del producto en el pedido

        [Range(0.01, 9999999)]
        public decimal Subtotal { get; set; }  // Calculado como Precio * Cantidad
    }
}
