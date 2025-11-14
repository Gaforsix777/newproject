using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
    }
}
// lo que tendria que hacer es listar produtos, crear productos, editar productos, eliminar productos, ordenar los productos por categoria, buscar productos por nombre y listar productos