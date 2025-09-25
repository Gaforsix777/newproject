using System.ComponentModel.DataAnnotations;

namespace PrimerParcial.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // FK -> Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // 1 -> N Ingredients
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
