// Models/ClienteModel.cs
using System.ComponentModel.DataAnnotations;

namespace practicamvc.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; }

        [EmailAddress, StringLength(180)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        public ICollection<PedidoModel>? Pedidos { get; set; }
    }
}
