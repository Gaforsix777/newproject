using System.ComponentModel.DataAnnotations;

namespace PrimerParcial.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
