using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required] public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required] public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required, Range(1, 1000000)]
        public int Cantidad { get; set; }

        [Precision(18, 2)]
        [Range(0.01, 99999999)]
        public decimal Subtotal { get; set; }
    }
}