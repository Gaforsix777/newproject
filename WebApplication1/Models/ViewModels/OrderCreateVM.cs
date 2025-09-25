using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class OrderCreateVM
    {
        [Required] public int ClienteId { get; set; }
        public List<Item> Items { get; set; } = new()
        {
            new(), new(), new() // 3 filas vacías (puedes ajustar)
        };

        public class Item
        {
            [Required] public int? ProductId { get; set; }
            [Required, Range(1, 1000000)] public int? Cantidad { get; set; }
        }
    }
}
